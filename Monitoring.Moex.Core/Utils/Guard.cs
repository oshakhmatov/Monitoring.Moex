namespace Monitoring.Moex.Core.Utils
{
    public static class Guard
    {
        public static void NotNull(object value, string valueName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(valueName);
            }
        }
    }
}
