using System;
using System.Collections.Generic;
using static Constants.PlayerLabels;

namespace Constants
{
    public static class GameSettings
    {
        public static readonly int RoundsCount = 6;
        public static readonly int RoundTime = 30;
        public static readonly int PostRoundTime = 15;
    }

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
            "Update my Linked-In profile",
            "Mentor an intern",
            "Create technical documentation",
            "Write reports",
            "Conduct market research",
            "Prioritize tasks and create to-do lists",
            "Research and explore new areas of interest",
            "Prioritize tasks and create to-do lists"
        };
        public static List<string> DevReqText = new List<string>
        {
            "Get your code review",
            "Complain about a bug",
            "Help to create a new feature",
            "Ask to assist with implementing a security update"
        };
        public static List<string> ITReqText = new List<string>
        {
            "Fix a bug",
            "Install a new software",
            "Fix the printer",
            "Fix the computer",
            "Request assistance in setting up a new software application",
            "Request a backup of your important files",
            "Ask for training on using a new software tool"
        };
        public static List<string> PMReqText = new List<string>
        {
            "Meet with a client",
            "Meeting 1:1",
            "Retrospective meeting",
            "Brainstorming session",
            "Meet to discuss feature priorities",
            "Request feedback on your product improvements"
        };
        public static List<string> HRReqText = new List<string>
        {
            "Talk about my vacation",
            "Ask for assistance in administering payroll and benefits",
            "Learn about the company values",
            "Request input on implementing diversity and inclusion initiatives",
            "Request assistance in conducting interviews and the hiring process",
            "Seek guidance in developing and updating HR policies and procedures"
        };
        public static List<string> QAReqText = new List<string>
        {
            "Request a review of your test plans and test cases",
            "Test a new product",
            "Seek input on continuous improvement of testing processes and methodologies",
            "Ask for assistance in reporting and tracking software defects"
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
            OneHour,
            TwoHours,
            ThreeHours
        };

        public static string EnumToString(TimeEnum time)
        {
            switch (time)
            {
                case TimeEnum.OneHour: return OneHoursString;
                case TimeEnum.TwoHours: return TwoHoursString;
                case TimeEnum.ThreeHours: return ThreeHoursString;
            }
            return "";
        }

        // we might wanna add some logic here, so 2 players cant get the same role same round
        public static TimeEnum GetRandomTimeEnum() 
        {
            return RandomEnum.Of<TimeEnum>();
        }

        private const string OneHoursString = "1:00";
        private const string TwoHoursString = "2:00";
        private const string ThreeHoursString = "3:00";
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
