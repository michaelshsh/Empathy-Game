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
            HR,
            QA
        };

        public static string EnumToString(LabelEnum label)
        {
            switch (label)
            {
                case LabelEnum.Dev: return Dev;
                case LabelEnum.IT: return IT;
                case LabelEnum.PM: return PM;
                case LabelEnum.HR: return HR;
                case LabelEnum.QA: return QA;
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
        private const string QA = "QA";
    }

    public static class CardText
    {
        public static List<string> GeneralText = new List<string>
        {
            "Finish a PR reveiw",
            "Talk to my manager about my salary",
            "Update my Linked-In profile"
        };
        public static List<string> DevReqText = new List<string>
        {
            "Get your code review",
            "Complain about a bug",
            "Create a new feature",
        };
        public static List<string> ITReqText = new List<string>
        {
            "Fix a bug",
            "Install a new software",
            "Fix the printer",
            "Fix the computer",
        };
        public static List<string> PMReqText = new List<string>
        {
            "Meet with a client",
            "Meeting 1:1",
            "Retrospective meeting",
        };
        public static List<string> HRReqText = new List<string>
        {
            "Talk about my vacation",
            "Talkabout my promotion",
            "Learn about the company values",
        };
        public static List<string> QAReqText = new List<string>
        {
            "Test a new feature",
            "Test a new product",
        };

        public static List<string> EnumToTextList(LabelEnum label)
        {
            switch (label)
            {
                case LabelEnum.Dev: return DevReqText;
                case LabelEnum.IT: return ITReqText;
                case LabelEnum.PM: return PMReqText;
                case LabelEnum.HR: return HRReqText;
                case LabelEnum.QA: return QAReqText;
            }
            return null;
        }
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
                case TimeEnum.HalfAnHour: return HalfAnHourString;
                case TimeEnum.OneHour: return OneHourString;
                case TimeEnum.HourAndaHalf: return HourAndaHalfString;
            }
            return "";
        }

        // we might wanna add some logic here, so 2 players cant get the same role same round
        public static TimeEnum GetRandomTimeEnum() 
        {
            return RandomEnum.Of<TimeEnum>();
        }

        private const string HalfAnHourString = "0:30";
        private const string OneHourString = "1:00";
        private const string HourAndaHalfString = "1:30";
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
