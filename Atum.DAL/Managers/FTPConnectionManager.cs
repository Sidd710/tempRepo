using System;
using System.Collections.Generic;
using System.Text;
using Atum.DAL.Managers;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Atum.DAL.Managers
{
    public class FTPConnectionManager
    {
        public enum FTPUserType
        {
            PrintQueue = 1,
            RPIUpdate = 2
        }

        public static void UploadFile(FTPUserType ftpUserType, string destinationIP, int destinationPort, string uriTarget, string sourceFile)
        {
            FileInfo fileInf = new FileInfo(sourceFile);
            string uri = string.Format("ftp://{0}:{1}/{2}", destinationIP, destinationPort, uriTarget, new FileInfo(sourceFile).Name);
            Debug.WriteLine(uri);
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
            switch (ftpUserType)
            {
                case FTPUserType.PrintQueue:
                    reqFTP.Credentials = new NetworkCredential("printqueue", "printqueue");
                    break;
                case FTPUserType.RPIUpdate:
                    reqFTP.Credentials = new NetworkCredential("atumupdate", "@tum.Update");
                    break;
            }
            
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;

            // The buffer size is set to 4kb
            int buffLength = 4096;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            using (FileStream fs = fileInf.OpenRead())
            {

                try
                {
                    // Stream to which the file to be upload is written
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);

                        while (contentLen != 0)
                        {
                            // Write Content from the file stream to the FTP Upload Stream
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }

                        strm.Close();
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.WriteToLog("File Connection", "Upload", ex);
                }
            }
        }
    }
}
