using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Common.DateTimeEx
{
    public static class DateTimeEx
    {
        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime time)
        {
            return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        /// <summary>
        /// 时间戳转格式化时间
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long unix)
        {
            var timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            var t = (unix + 8 * 60 * 60) * 10000000 + timeStamp.Ticks;
            var dt = new DateTime(t);

            return dt;
        }
    }
}
