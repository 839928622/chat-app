using System.ComponentModel.DataAnnotations;

namespace Application.Attributes
{
    public class AgeMustAfterOrEqualTo : ValidationAttribute
    {
        private int MinAge { get; set; }
        public AgeMustAfterOrEqualTo(int minAge)
        {
            this.MinAge = minAge;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var dateTimeOffset = (DateTimeOffset)value;
            return dateTimeOffset <= DateTimeOffset.Now.AddYears(-MinAge);
        }
    }
}
