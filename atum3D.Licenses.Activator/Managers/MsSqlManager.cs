using Atum.DAL.Licenses;
using Atum.DAL.Logging;
using Atum.DAL.Managers;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Licenses.Activator.Managers
{
    public class MsSqlManager
    {
        private static SqlConnection _sqlConnection;

        private static SqlConnection GenerateSqlConnection()
        {
            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlConnection(Properties.Settings.Default.SqlDatabase_ConnectionString);
            }

            return _sqlConnection;
        }

        internal static void SaveLogging(LoggingInfo logging)
        {
            try
            {
                if (logging.Value != null)
                {
                    using (var sqlCommand = new SqlCommand())
                    {
                        GenerateSqlConnection();

                        sqlCommand.CommandText = string.Format("insert into Logging(Text) values({0})", "\'" + logging.Value + "\'");
                        sqlCommand.Connection = _sqlConnection;
                        sqlCommand.CommandType = CommandType.Text;
                        _sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        _sqlConnection.Close();
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                LoggingManager.WriteToLog("SQL", "SaveLogging", exc);
            }
        }

        public static long SaveRAWMessage(EmailMessage message)
        {
            try
            {
                GenerateSqlConnection();

                //convert the email message to a byte array, and save to sql server
                byte[] output = message.MimeContent.Content;
                MemoryStream memorystream = new MemoryStream();
                memorystream.Position = 0;
                memorystream.Read(output, 0, (int)memorystream.Length);
                using (var cmd = new SqlCommand("INSERT INTO Email_License_Request (Message_Blob, Sender_Address) OUTPUT INSERTED.Id values (@Message_Blob, \'" + message.Sender.Address + "\')", _sqlConnection))
                {
                    cmd.Parameters.Add("@Message_Blob", SqlDbType.VarBinary).Value = output;
                    _sqlConnection.Open();
                    var lastId = (int)cmd.ExecuteScalar();
                    _sqlConnection.Close();
                    return lastId;
                }
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("SQL", "SaveRAWMessage", exc);
            }

            return -1;
        }

        public static void UpdateRAWMessageWithActivationCode(long messageId, OnlineCatalogLicenses licenseRequest)
        {
            try
            {
                GenerateSqlConnection();

                //convert the email message to a byte array, and save to sql server
                using (var cmd = new SqlCommand("Update Email_License_Request SET Activation_Code = @ActivationCode where Id = " + messageId, _sqlConnection))
                {
                    cmd.Parameters.Add("@ActivationCode", SqlDbType.Text).Value = licenseRequest.ToLicenseRequest();
                    _sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    _sqlConnection.Close();
                }
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("SQL", "UpdateRAWMessageWithActivationCode", exc);
            }
        }
    }
}
