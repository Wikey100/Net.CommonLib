/*******************************************************************
 * * 文件名： UnityContainerExtensions.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using Microsoft.Practices.Unity;
using System;

namespace Net.CommonLib.Util.Extensions
{
    public static class UnityContainerExtensions
    {
        public static void RegisterNavigationType(this IUnityContainer container, Type type)
        {
            container.RegisterType(typeof(Object), type, type.FullName);
        }
    }
}