using System;
using System.Collections.Generic;
using static Constants.PlayerLabels;

namespace Constants
{
    public static class PlayerLabels
    {
        public enum LabelEnum
        {
            Dev,
            IT,
            PM,
            HR
        };

        public static string EnumToString(LabelEnum label)
        {
            switch (label)
            {
                case LabelEnum.Dev: return Dev;
                case LabelEnum.IT: return IT;
                case LabelEnum.PM: return PM;
                case LabelEnum.HR: return HR;
            }
            return "";
        }

        // we might wanna add some logic here, so 2 players cant get the same role same round
        public static LabelEnum GetRandomLabelEnum()
        {
            return RandomEnum.Of<LabelEnum>();
        }

        private const string Dev = "Developer";
        private const string IT = "Tech Support";
        private const string PM = "Product Manager";
        private const string HR = "Human Resorces";
    }

    public static class CardText
    {
        public static List<string> GeneralText = new List<string>
        {
            "Finish a PR reveiw",
            "Talk to my manager about my salary",
            "Update my Linked-In profile"
        };
    }

    public static class CardTime
    {
        public enum TimeEnum    
        {
            HalfAnHour,
            OneHour,
            HourAndaHalf
        };

        public static string EnumToString(TimeEnum time)
        {
            switch (time)
            {
                case TimeEnum.HalfAnHour: return HalfAnHour;
                case TimeEnum.OneHour: return OneHour;
                case TimeEnum.HourAndaHalf: return HourAndaHalf;
            }
            return "";
        }

        // we might wanna add some logic here, so 2 players cant get the same role same round
        public static TimeEnum GetRandomTimeEnum() 
        {
            return RandomEnum.Of<TimeEnum>();
        }

        private const string HalfAnHour = "0:30";
        private const string OneHour = "1:00";
        private const string HourAndaHalf = "1:30";
    }
}

public static class RandomEnum
{
    private static Random _Random = new Random(Environment.TickCount);

    public static T Of<T>()
    {
        Array enumValues = Enum.GetValues(typeof(T));
        return (T)enumValues.GetValue(_Random.Next(enumValues.Length));
    }
}
