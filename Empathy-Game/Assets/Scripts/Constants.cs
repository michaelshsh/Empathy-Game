using System;

namespace Constants
{
    public static class Labels
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