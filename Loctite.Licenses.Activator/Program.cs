
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.ServiceProcess;
using System.Text;
using Atum.DAL.Managers;
using Loctite.Licenses.Activator.Objects;
using Loctite.Licenses.Activator.Managers;

namespace Loctite.Licenses.Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingManager.Start();
            LoggingManager.WriteToLog("Activation", "Check", "Started");


            //if (!Environment.UserInteractive)
            //    // running as service
            //    using (var service = new srvMain())
            //        ServiceBase.Run(service);
            //else
            //{
            // running as console app
            Start(args);

            // Console.WriteLine("Press any key to stop...");
            // Console.ReadKey(true);

            Stop();
            //}
        }

        private static Boolean AutodiscoverUrl(ExchangeService service, Office365User user)
        {
            Boolean isSuccess = false;

            try
            {
                LoggingManager.WriteToLog("Exchange Online", "Started", string.Empty);
                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Exchange Online", "Started", "Exchange Online started"));
                service.AutodiscoverUrl(user.Account, CallbackMethods.RedirectionUrlValidationCallback);
                LoggingManager.WriteToLog("Exchange Online", "Connected", string.Empty);
                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Exchange Online", "Connected", "Exchange Online connected"));

                isSuccess = true;
            }
            catch (Exception e)
            {
                LoggingManager.WriteToLog("AutodiscoverUrl", "Exception", e.Message);
                Console.WriteLine(e.Message);
            }

            return isSuccess;
        }

        internal static void Start(string[] args)
        {
            try
            {
                var applicationSettings = new Properties.Settings();

                ServicePointManager.ServerCertificateValidationCallback = CallbackMethods.CertificateValidationCallBack;
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

                // Get the information of the account.
                var user = new Office365User();
                service.Credentials = new WebCredentials(user.Account, user.Pwd);

                // Set the url of server.
                if (!AutodiscoverUrl(service, user))
                {
                    return;
                }

                var mailboxMonitorAddress = applicationSettings.Mailbox_Monitor;


                var moveMailFolderId = GetFolderId(service, "Processed");
                var invalidMailFolderId = GetFolderId(service, "Invalid");

                var folderView = new FolderView(2);
                folderView.PropertySet = new PropertySet(BasePropertySet.IdOnly);
                folderView.Traversal = FolderTraversal.Deep;

                //var inboxFilter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Test");
                //var inboxResults = service.FindFolders(WellKnownFolderName.Root, inboxFilter, folderView);
                //                var inboxResults = service.FindFolders(WellKnownFolderName.Inbox, folderView);

                //              if (inboxResults.TotalCount > 0)
                //            {
                Folder inbox = Folder.Bind(service, WellKnownFolderName.Inbox);
                //            var inboxId = inboxResults.Folders[0].Id;
                Folder inboxFolder = Folder.Bind(service, inbox.Id);
                if (inboxFolder.UnreadCount > 0)
                {
                    ItemView unreadView = new ItemView(inboxFolder.UnreadCount);
                    unreadView.PropertySet = PropertySet.IdOnly;
                    FindItemsResults<Item> results = service.FindItems(inbox.Id, unreadView);
                    foreach (Item item in results.Items)
                    {
                        EmailMessage email = EmailMessage.Bind(service, new ItemId(item.Id.UniqueId.ToString()));
                        if (!email.IsRead)
                        {
                            PropertySet ps = new PropertySet(ItemSchema.MimeContent, ItemSchema.Body, EmailMessageSchema.Sender, EmailMessageSchema.IsRead, ItemSchema.DateTimeReceived, ItemSchema.Attachments);
                            email.Load(ps);

                            var savedMySQLMessageId = MsSqlManager.SaveRAWMessage(email);

                            var processedPlainTextBody = ProcessEmailBodyText(email, mailboxMonitorAddress, moveMailFolderId, savedMySQLMessageId);
                            if (!processedPlainTextBody)
                            {
                                var processedAttachment = ProcessEmailAttachments(email, mailboxMonitorAddress, moveMailFolderId, savedMySQLMessageId);
                                if (!processedAttachment)
                                {
                                    ProcessEmailAsInvalid(email, mailboxMonitorAddress, invalidMailFolderId);
                                }
                            }
                        }
                    }
                }
                //          }

            }
            catch (Exception exc)
            {
                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString(), "Start", "Exception", exc.StackTrace));
                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Exchange Online", "Exception", exc.StackTrace));
            }
        }

        internal static void Stop()
        {
            // onstop code here

        }

        private static FolderId GetFolderId(ExchangeService service, string folderName)
        {
            var folderView = new FolderView(2);
            folderView.PropertySet = new PropertySet(BasePropertySet.IdOnly);
            folderView.Traversal = FolderTraversal.Deep;

            var folderFilter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, folderName);
            var folderResults = service.FindFolders(WellKnownFolderName.Root, folderFilter, folderView);
            if (folderResults.TotalCount == 1)
            {
                return folderResults.Folders[0].Id;
            }

            return null;
        }
        private static bool ProcessEmailBodyText(EmailMessage email, string mailboxMonitorAddress, FolderId moveMailFolderId, long savedMySQLMessageId)
        {
            if (email.Body.Text.Contains("---StartOfLicenseCode---"))
            {
                Atum.DAL.Managers.LoggingManager.WriteToLog("Processing Mail", "From", email.Sender.Address);
                Atum.DAL.Managers.LoggingManager.WriteToLog("Processing Mail", "Start of license block", "Found");

                var emailBodyAsHtml = email.Body.Text;
                var emailBodyText = emailBodyAsHtml.Substring(emailBodyAsHtml.IndexOf("---StartOfLicenseCode---"));
                var endLicenseTag = "---EndofLicenseCode---";
                emailBodyText = emailBodyText.Substring(0, emailBodyText.IndexOf(endLicenseTag) + endLicenseTag.Length);
                emailBodyText = Regex.Replace(emailBodyText, "<.*?>", string.Empty); //strip HTML tags
                emailBodyText = WebUtility.HtmlDecode(emailBodyText); //convert html encoding to text encoding

                var licenseFile = Atum.DAL.Licenses.OnlineCatalogLicenses.FromLicenseStream(emailBodyText);

                if (licenseFile.Count == 1)
                {
                    LoggingManager.WriteToLog("Processing Mail", "License Object", "Processed");


                    //debug (use forwarding to monitor registration process)
                    email.Forward(new MessageBody(BodyType.HTML, "Done?"), new EmailAddress[1] { mailboxMonitorAddress });
                    MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Done"));
                    LoggingManager.WriteToLog("Processing Mail", "Forwarding with body", "Done");

                    //preprare
                    var emailReply = new StringBuilder();
                    if (email.Body.BodyType == BodyType.HTML)
                    {
                        emailReply.AppendLine("Dear customer, <br/><br/>Thank you for requesting an activation code.<br/><br/>");
                    }
                    else
                    {
                        emailReply.AppendLine("Dear customer,");
                        emailReply.AppendLine("Thank you for requesting an activation code.");
                        emailReply.AppendLine();
                        emailReply.AppendLine();
                    }

                    //activate license
                    licenseFile[0].Activated = true;
                    licenseFile[0].ExpirationDate = DateTime.Now.AddYears(1);
                    licenseFile[0].ActivationDate = DateTime.Now;
                    licenseFile[0].LicenseType = Atum.DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard;

                    emailReply.AppendLine(licenseFile.ToLicenseRequest());
                    email.Reply(new MessageBody(email.Body.BodyType, emailReply.ToString()), true);

                    MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Done"));
                    LoggingManager.WriteToLog("Processing Mail", "Forwarded with body", "Done");

                    //mark as read
                    email.IsRead = true;
                    email.Update(ConflictResolutionMode.AlwaysOverwrite);
                    LoggingManager.WriteToLog("Processing Mail", "Email marked as", "Unread");
                    MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email marked as", "Unread"));

                    //save to database
                    MsSqlManager.UpdateRAWMessageWithActivationCode(savedMySQLMessageId, licenseFile);

                    //move mail to processed folder
                    email.Move(moveMailFolderId);
                    LoggingManager.WriteToLog("Processing Mail", "Email moved to", "Processed");
                    MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email moved to", "Processed"));

                    //save to path
                    var atumLicenseFile = new HenkelEmailLicenseRequest(email.Sender.Address, email.DateTimeReceived, licenseFile[0]);
                    atumLicenseFile.Save();

                    LoggingManager.WriteToLog("Processing Mail", "Email moved to", "Processed");
                    MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email moved to", "Processed"));

                }
                return true;
            }
            return false;
        }

        private static bool ProcessEmailAttachments(EmailMessage email, string mailboxMonitorAddress, FolderId moveMailFolderId, long savedMySQLMessageId)
        {
            if (email.Attachments.Count > 0)
            {
                foreach (var emailAttachment in email.Attachments.Where(a => a.Name.ToLower().EndsWith("lic")))
                {
                    if (emailAttachment is FileAttachment)
                    {
                        var emailFileAttachment = emailAttachment as FileAttachment;
                        emailFileAttachment.Load();
                        var utfDecodedContent = Encoding.UTF8.GetString(emailFileAttachment.Content);
                        if (utfDecodedContent.Contains("---"))
                        {
                            Atum.DAL.Managers.LoggingManager.WriteToLog("Processing Mail", "From", email.Sender.Address);
                            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "From", email.Sender.Address));
                            Atum.DAL.Managers.LoggingManager.WriteToLog("Processing Mail", "Start of license block", "Found");
                            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Start of license block", "Found"));

                            var activatedLicenseFile = Atum.DAL.Licenses.OnlineCatalogLicenses.FromLicenseStream(utfDecodedContent);

                            if (activatedLicenseFile.Count == 1)
                            {
                                LoggingManager.WriteToLog("Processing Mail", "License Object", "Processed");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "License Object", "Processed"));


                                //debug (use forwarding to monitor registration process)
                                email.Forward(new MessageBody(BodyType.HTML, "Done?"), new EmailAddress[1] { mailboxMonitorAddress });
                                LoggingManager.WriteToLog("Processing Mail", "Forwarding with body", "Done");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Done"));

                                //prepare
                                var emailReply = new StringBuilder();
                                if (email.Body.BodyType == BodyType.HTML)
                                {
                                    emailReply.AppendLine("Dear customer, <br/><br/>Thank you for requesting an activation code.<br/><br/>");
                                }
                                else
                                {
                                    emailReply.AppendLine("Dear customer,");
                                    emailReply.AppendLine("Thank you for requesting an activation code.");
                                    emailReply.AppendLine();
                                    emailReply.AppendLine();
                                }

                                //activate license
                                activatedLicenseFile[0].Activated = true;
                                activatedLicenseFile[0].ExpirationDate = DateTime.Now.AddYears(1);
                                activatedLicenseFile[0].ActivationDate = DateTime.Now;
                                activatedLicenseFile[0].LicenseType = Atum.DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard;

                                var emailFileAttachmentName = emailFileAttachment.Name;
                                var emailFileAttachmentNameActivated = emailFileAttachment.Name.Substring(0, emailFileAttachment.Name.LastIndexOf("."));
                                emailFileAttachmentNameActivated += "-activated.lic";
                                var replyMessage = email.CreateReply(true);
                                replyMessage.BodyPrefix = emailReply.ToString();
                                EmailMessage replyEmailMessage = replyMessage.Save();
                                replyEmailMessage.Attachments.AddFileAttachment(emailFileAttachmentNameActivated, Encoding.UTF8.GetBytes(activatedLicenseFile.ToLicenseRequest()));
                                replyEmailMessage.Update(ConflictResolutionMode.AutoResolve);
                                replyEmailMessage.Send();

                                LoggingManager.WriteToLog("Processing Mail", "Forwarded with body", "Done");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Done"));

                                //mark as read
                                email.IsRead = true;
                                email.Update(ConflictResolutionMode.AlwaysOverwrite);
                                LoggingManager.WriteToLog("Processing Mail", "Email marked as", "Unread");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email marked as", "Unread"));

                                //save to database
                                MsSqlManager.UpdateRAWMessageWithActivationCode(savedMySQLMessageId, activatedLicenseFile);

                                //move mail to processed folder
                                email.Move(moveMailFolderId);
                                LoggingManager.WriteToLog("Processing Mail", "Email moved to", "Processed");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email moved to", "Processed"));

                                //save to path
                                var atumLicenseFile = new HenkelEmailLicenseRequest(email.Sender.Address, email.DateTimeReceived, activatedLicenseFile[0]);
                                atumLicenseFile.Save();

                                LoggingManager.WriteToLog("Processing Mail", "Email moved to", "Processed");
                                MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email moved to", "Processed"));
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static void ProcessEmailAsInvalid(EmailMessage email, string mailboxMonitorAddress, FolderId invalidMailFolderId)
        {
            //forward for debugging
            LoggingManager.WriteToLog("Processing Mail", "Forwarding with body", "Invalid");
            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Invalid"));
            email.Forward(new MessageBody(BodyType.HTML, "INVALID?"), new EmailAddress[1] { mailboxMonitorAddress });

            LoggingManager.WriteToLog("Processing Mail", "Forwarded with body", "Invalid");
            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Forwarding with body", "Invalid"));

            email.IsRead = true;
            email.Update(ConflictResolutionMode.AlwaysOverwrite);
            LoggingManager.WriteToLog("Processing Mail", "Email marked as", "Unread");
            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Marked as", "Unread"));

            //move mail to processed folder
            email.Move(invalidMailFolderId);

            LoggingManager.WriteToLog("Processing Mail", "Email moved to", "Invalid");
            MsSqlManager.SaveLogging(new Atum.DAL.Logging.LoggingInfo(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "Processing Mail", "Email moved to", "Invalid"));
        }

    }
}
