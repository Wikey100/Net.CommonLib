/*******************************************************************
 * * 文件名： EnumerableExtension.cs
 * * 文件作用： 
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;

namespace Net.CommonLib.Util.Extensions
{
    public static class EnumerableExtension
    {       
        public static void ForEach<T>( this IEnumerable<T> enumerable, Action<T> action )
        {
            foreach (T item in enumerable)
            {
                action( item );
            }
        }

        //public static bool IsNullOrEmpty<T>( this IEnumerable<T> items, out IEnumerable<T> newItems ) 
        //{
        //    newItems = items;

        //    if (items == null)
        //    {
        //        return false;
        //    }

        //    var enumerator = items.GetEnumerator();

        //    if (enumerator.MoveNext() == false) 
        //    {
        //        return false;
        //    }

        //    newItems = new[]{enumerator.Current }

        //        return true;
        //}

    }
}
