/*******************************************************************
 * * 文件名： FTPClient.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-01-08    xwj       新增
 * *******************************************************************/
/*******************************************************************
* * 文件名：
* * 文件作用：
* *-------------------------------------------------------------------
* * 修改历史记录：
* * 修改时间      修改人    修改内容概要
* * 2013-02-23    xwj       新增
* *******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Net.CommonLib.Ftp
{
    /// <summary>
    ///
    /// </summary>
    public class FtpClient
    {
        #region field

        private string hostUrl;

        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;

        private string pwd;

        #endregion field

        #region property

        /// <summary>
        /// Gets or sets the host URL.
        /// </summary>
        /// <value>The host URL.</value>
        public string HostUrl
        {
            get
            {
                if (!hostUrl.StartsWith("ftp://"))
                {
                    hostUrl = "ftp://" + hostUrl;
                }
                return hostUrl;
            }
            set { hostUrl = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        /// <value>The PWD.</value>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        #endregion property

        #region event

        public event DownLoadProgressEventHandler DownloadProgressChangedEvent;

        #endregion event

        #region constructor

        /// <summary>
        /// 初始化 <see cref="FtpClient"/> class的实例.
        /// </summary>
        /// <param name="hostName">ftp主机名.</param>
        public FtpClient(string hostName)
        {
            hostUrl = hostName;
        }

        /// <summary>
        /// 初始化 <see cref="FtpClient"/> class的实例.
        /// </summary>
        /// <param name="hostName">主机名</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        public FtpClient(string hostName, string userName, string pwd)
        {
            this.hostUrl = hostName;
            this.userName = userName;
            this.pwd = pwd;
        }

        #endregion constructor

        #region public Method

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ftpfileName">远程文件</param>
        /// <param name="localFileName">本地文件</param>
        public void Download(string ftpfileName, string localFileName)
        {
            DownloadProgressEventArgs args = new DownloadProgressEventArgs();
            args.TotalBytesToRecevice = GetFileSize(ftpfileName);
            FtpWebRequest req = GetRequest(ftpfileName);
            req.Method = "RETR";
            req.UsePassive = false;
            req.UseBinary = true;
            using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
            {
                using (Stream stream = res.GetResponseStream())
                {
                    //args.TotalBytesToRecevice = stream.Length;

                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(localFileName, FileMode.Create);
                        byte[] buf = new byte[1024];
                        int count = 0;
                        do
                        {
                            count = stream.Read(buf, 0, 1024);
                            args.BytesRecevied += count;
                            if (DownloadProgressChangedEvent != null)
                            {
                                DownloadProgressChangedEvent(this, args);
                            }
                            fs.Write(buf, 0, count);
                        } while (count > 0);
                        args.BytesRecevied = args.TotalBytesToRecevice;
                        if (DownloadProgressChangedEvent != null)
                        {
                            DownloadProgressChangedEvent(this, args);
                        }
                        fs.Flush();
                    }
                    catch (Exception)
                    {
                        if (File.Exists(localFileName))
                        {
                            File.Delete(localFileName);
                        }
                        throw;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 删除FTP文件
        /// </summary>
        /// <param name="ftpFileName">Name of the FTP file.</param>
        /// <returns></returns>
        public bool DeleteFile(string ftpFileName)
        {
            FtpWebRequest req = GetRequest(ftpFileName);
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                req.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">本地文件.</param>
        /// <param name="ftpFile">Ftp文件名.</param>
        public void Upload(string localFile, string ftpFile)
        {
            FtpWebRequest req = GetRequest(ftpFile);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.UseBinary = true;
            using (Stream stream = req.GetRequestStream())
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(localFile, FileMode.Open);
                    byte[] buf = new byte[1024];
                    int count = 0;
                    do
                    {
                        count = fs.Read(buf, 0, 1024);
                        stream.Write(buf, 0, count);
                    } while (count > 0);
                    stream.Flush();
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    stream.Close();
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public string[] GetFileList(string dir)
        {
            FtpWebRequest req = GetRequest(dir);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.UseBinary = true;
            using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
            {
                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    List<string> list = new List<string>();
                    try
                    {
                        string temp = string.Empty;
                        while (true)
                        {
                            temp = sr.ReadLine();
                            if (temp == null)
                                break;
                            list.Add(temp);
                        }
                        return list.ToArray();
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                        }
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public string[] GetDirList(string dir)
        {
            FtpWebRequest req = GetRequest(dir);
            req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            req.UseBinary = true;
            using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
            {
                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    List<string> list = new List<string>();
                    try
                    {
                        string temp = string.Empty;
                        while (true)
                        {
                            temp = sr.ReadLine();
                            if (temp == null)
                                break;
                            list.Add(temp);
                        }
                        return list.ToArray();
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                        }
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified file name is exisit.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// 	<c>true</c> if the specified file name is exisit; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExists(string fileName)
        {
            if (GetFileSize(fileName) != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="dir">目录名</param>
        /// <returns>
        /// 	<c>true</c> if [is dir exists] [the specified dir]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDirExists(string dir)
        {
            if (!CreateDir(dir))
            {
                return true;
            }
            else
            {
                DeleteDir(dir);
                return false;
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="ftpDir">目录名</param>
        /// <returns></returns>
        public bool DeleteDir(string ftpDir)
        {
            FtpWebRequest req = GetRequest(ftpDir);
            req.Method = WebRequestMethods.Ftp.RemoveDirectory;
            try
            {
                req.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="ftpDir">目录名</param>
        /// <returns></returns>
        public bool CreateDir(string ftpDir)
        {
            FtpWebRequest req = GetRequest(ftpDir);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                req.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取文件长度
        /// </summary>
        /// <param name="filaName">文件名</param>
        /// <returns></returns>
        public long GetFileSize(string filaName)
        {
            FtpWebRequest req = GetRequest(filaName);
            req.Method = WebRequestMethods.Ftp.GetFileSize;
            try
            {
                FtpWebResponse res = (FtpWebResponse)req.GetResponse();
                return res.ContentLength;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        #endregion public Method

        #region private Method

        private FtpWebRequest GetRequest(string url)
        {
            string fullUrl = CombinePath(HostUrl, url);
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(fullUrl);
            req.Credentials = new NetworkCredential(userName, pwd);
            req.KeepAlive = false;
            return req;
        }

        private string CombinePath(string path1, string path2)
        {
            return path1.Trim(new char[] { '/' }) + "/" + path2.Trim(new char[] { '/' });
        }

        #endregion private Method
    }
}