/*******************************************************************
 * * 文件名： SocketCommLog.cs
 * * 文件作用： 日志记录
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2018-03-15    xwj       新增
 * *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.CommonLib.Log;

namespace Net.CommonLib.SocketComm
{
    public class SocketCommLog : ILog
    {
        public void Debug(string msg)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Debug(string msg, Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Error(string msg)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Error(string msg, Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Fatal(string msg)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Fatal(string msg, Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Info(string msg)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Info(string msg, Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Warn(string msg)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public void Warn(string msg, Exception e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }
    }
}
