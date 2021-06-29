using System;

namespace Pay.Verification.Domain
{
    public class DateOfBirth
    {
        public DateTime Value { get; set; }
        public static DateOfBirth FromParts(int year, int month, int day)
        {
            var dob = new DateTime(year, month, day);
            return new DateOfBirth(dob);
        }

        public static DateOfBirth FromString(string strDob)
        {
            if (!DateTime.TryParse(strDob, out DateTime dob))
            {
                throw new Exception("Date of birth is invalid");
            }
            return new DateOfBirth(dob);
        }

        public DateOfBirth(DateTime date)
        {
            Value = date;
            if (DateOfBirth.GetAge(Value) < 18)
            {
                throw new Exception("Minimum age is 18");
            }
        }

        public static implicit operator string(DateOfBirth dob) => dob.Value.ToShortDateString();

        private static int GetAge(DateTime dob)
        {
            DateTime today = DateTime.Now;
            int age = today.Year - dob.Year;
            if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day)) { age--; }
            return age;        
        }
    }
}