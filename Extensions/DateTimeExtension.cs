using System;

namespace FitbyteServer.Extensions {

    public static class DateTimeExtension {

        const DayOfWeek START_OF_WEEK = DayOfWeek.Monday;

        public static DateTime StartOfWeek(this DateTime datetime) {
            int diff = (7 + (datetime.DayOfWeek - START_OF_WEEK)) % 7;
            return datetime.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime datetime) {
            return StartOfWeek(datetime).AddDays(6);
        }

    }

}
