using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dateOfBirthday){
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dateOfBirthday.Year;

            if (dateOfBirthday > today.AddYears(-age)) age--;

            return age;
        }
    }
}