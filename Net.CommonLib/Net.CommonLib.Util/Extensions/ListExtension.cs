/*******************************************************************
 * * 文件名： ListExtension.cs
 * * 文件作用： 
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.CommonLib.Util.Extensions
{
    public static class ListExtension
    {
        public static IList<TDest> Convert<TSource, TDest>( this IList<TSource> target, Func<TSource, TDest> convert )
        {
            var list = new List<TDest>();

            var destList = target.Select( convert );

            foreach (TDest dest in destList)
            {
                list.Add( dest );
            }

            return list;
        }
    }
}
