namespace Monitoring.Moex.Core.Common
{
    public static class Guard
    {
        public static void NotNull(object value, string valueName)
        {
            if (value == null)
                throw new ArgumentNullException(valueName);
        }
    }
}
