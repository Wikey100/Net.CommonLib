/*******************************************************************
 * * 文件名： Helper.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using Net.CommonLib.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;

namespace Net.CommonLib.Util
{
    public class Helper
    {
        /// <summary>
        /// 获取本地所有的IP地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalIPAddress()
        {
            var list = new List<string>();
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(sHostName);
            IPAddress[] IpA = ipE.AddressList;
            for (int i = 0; i < IpA.Length; i++)
            {
                list.Add(IpA[i].ToString());
            }
            return list;
        }

        /// <summary>
        /// 设备代码->设备名称
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public static string GetDeviceTypeName(string deviceType)
        {
            switch (deviceType)
            {
                case "02":
                    return "TVM";

                case "03":
                    return "BOM";

                case "04":
                    return "AGM";

                case "05":
                    return "TCM";
            }
            return "";
        }

        public static DateTime GetDateTimeSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString().ParseDateTime();
            }
            catch
            {
                throw new InvalidOperationException("获取datetime类型配置项{0}失败".FormatWith(key));
            }
        }

        public static int GetIntSetting(string key)
        {
            if (CheckAppSettings(key))
            {
                try
                {
                    return ConfigurationManager.AppSettings[key].ToString().ParseInt();
                }
                catch
                {
                    throw new InvalidOperationException("获取int类型的配置项：{0}失败".FormatWith(key));
                }
            }
            return 0;
        }

        public static string GetStringSetting(string key)
        {
            if (CheckAppSettings(key))
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            return string.Empty;
        }

        public static bool GetBoolSetting(string key)
        {
            if (CheckAppSettings(key))
            {
                try
                {
                    return ConfigurationManager.AppSettings[key].ToString().ParseBool();
                }
                catch
                {
                    throw new InvalidOperationException("获取bool类型的配置项：{0}失败".FormatWith(key));
                }
            }
            return false;
        }

        private static bool CheckAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return true;
            }
            else
            {
                throw new InvalidOperationException("缺少配置项：{0}".FormatWith(key));
            }
        }

        private static long randomSeed = DateTime.Now.Ticks;

        /// <summary>
        /// 生成6位随机密码
        /// </summary>
        /// <returns></returns>
        public static string GetRandomPassword()
        {
            Random r = new Random((int)(System.DateTime.Now.Ticks + randomSeed));
            randomSeed++;
            int it = r.Next(999999);
            string pwd = "000000" + it;
            pwd = pwd.Substring(pwd.Length - 6, 6);
            return pwd;
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDigit(string str)
        {
            if (str == null || str.Trim() == "") return false;
            char[] cs = str.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i] < '0' || cs[i] > '9')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 字典数组转换为Datatable
        /// </summary>
        /// <param name="dictArr">字典数组</param>
        /// <returns></returns>
        public static DataTable DictionaryArrToDatatable(Dictionary<object, object>[] dictArr)
        {
            DataTable dt = new DataTable();

            if (dictArr.Length > 0)
            {
                //添加列
                foreach (var element in dictArr[0])
                {
                    DataColumn column = new DataColumn(element.Key.ToString(), element.Value.GetType());

                    dt.Columns.Add(column);
                }

                //添加行数据
                foreach (Dictionary<object, object> dict in dictArr)
                {
                    DataRow dr = dt.NewRow();

                    foreach (var element in dict)
                    {
                        dr[element.Key.ToString()] = element.Value;
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 检查子模块是否运行正常
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckChildModuleIsNormal(string key, string value)
        {
            //if (GlobalConfig.Instance.ChildModuleStatus.Contains(key))
            //{
            //    //如果属于子模块，则根据状态值返回运行状态
            //    return value == "0";
            //}
            //如果不是子模块，则返回正常
            return true;
        }

        /// <summary>
        /// 检查设备是否已经登录
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckDeviceIsLogined(string key, string value)
        {
            if (key.Equals("022") && value.Equals("1"))
            {
                return true;
            }

            return false;
        }
    }
}