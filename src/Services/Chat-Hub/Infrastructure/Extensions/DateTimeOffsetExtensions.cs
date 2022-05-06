namespace Infrastructure.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// calculate Age base on birthday(DateTimeOffset)
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static int CalculateAge(this DateTimeOffset dateTimeOffset)
        {
            var today = DateTimeOffset.Now;
            var age = today.Year - dateTimeOffset.Year;
            if (today.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
