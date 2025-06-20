﻿using System;
namespace StreamStore.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ThrowIfMinValue(this DateTime dateTime, string name)
        {
            if (dateTime == DateTime.MinValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return dateTime;
        }
    }
}
