using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Core.Utils.Utils
{
    public static class Utility
    {
        public static List<SqlParameter> GetSqlParameterList(object model)
        {
            var sqlParameters = new List<SqlParameter>();
            List<PropertyInfo> propertyList = model.GetType().GetProperties().ToList();

            foreach (PropertyInfo property in propertyList)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(model);

                if (propertyValue != null)
                {
                    sqlParameters.Add(new SqlParameter
                    {
                        ParameterName = $"@{propertyName}",
                        DbType = property.GetDbType(),
                        Direction = ParameterDirection.Input,
                        Value = propertyValue
                    });
                }
            }
            return sqlParameters;
        }

        public static DbType GetDbType(this PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return DbType.String;
            }
            else if (property.PropertyType == typeof(byte[])) // || property.PropertyType == typeof(System.Data.Linq.Binary)
            {
                return DbType.Binary;
            }
            else if (property.PropertyType == typeof(byte?) || property.PropertyType == typeof(byte))
            {
                return DbType.Byte;
            }
            else if (property.PropertyType == typeof(sbyte?) || property.PropertyType == typeof(sbyte))
            {
                return DbType.SByte;
            }
            else if (property.PropertyType == typeof(short?) || property.PropertyType == typeof(short))
            {
                return DbType.Int16;
            }
            else if (property.PropertyType == typeof(ushort?) || property.PropertyType == typeof(ushort))
            {
                return DbType.UInt16;
            }
            else if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(int))
            {
                return DbType.Int32;
            }
            else if (property.PropertyType == typeof(uint?) || property.PropertyType == typeof(uint))
            {
                return DbType.UInt32;
            }
            else if (property.PropertyType == typeof(long?) || property.PropertyType == typeof(long))
            {
                return DbType.Int64;
            }
            else if (property.PropertyType == typeof(ulong?) || property.PropertyType == typeof(ulong))
            {
                return DbType.UInt64;
            }
            else if (property.PropertyType == typeof(float?) || property.PropertyType == typeof(float))
            {
                return DbType.Single;
            }
            else if (property.PropertyType == typeof(double?) || property.PropertyType == typeof(double))
            {
                return DbType.Double;
            }
            else if (property.PropertyType == typeof(decimal?) || property.PropertyType == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (property.PropertyType == typeof(bool?) || property.PropertyType == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (property.PropertyType == typeof(char?) || property.PropertyType == typeof(char))
            {
                return DbType.StringFixedLength;
            }
            else if (property.PropertyType == typeof(Guid?) || property.PropertyType == typeof(Guid))
            {
                return DbType.Guid;
            }
            else if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
            {
                return DbType.DateTime;
            }
            else if (property.PropertyType == typeof(DateTimeOffset?) || property.PropertyType == typeof(DateTimeOffset))
            {
                return DbType.DateTimeOffset;
            }

            return DbType.String;
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        public static string GenerateRandomNumber(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static string GenerateRandomTimeNumber()
        {
            return $"{DateTime.Now:yyMMddHHmmssff}";
        }

        public static string GenerateRandomTimeNumber(int length)
        {
            return $"{DateTime.Now:yyMMddHHmmssff}{GenerateRandomNumber(length)}";
        }

        public static string ToCommaSeparatedString(this List<string> list)
        {
            return string.Join(",", list);
        }

        //public static IEnumerable DistinctBy(this IEnumerable list, Func<t, object> propertySelector)
        //{
        //    return list.GroupBy(propertySelector).Select(x => x.First());
        //}

        public static string GetClaimValue(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

            return claim != null ? claim.Value : string.Empty;
        }

        public static List<T> ToObject<T>(this DataTable dataTable) where T : new()
        {
            var objectList = new List<T>();

            foreach (var row in dataTable.AsEnumerable().ToList())
            {
                objectList.Add(CreateItemFromRow<T>(row));
            }

            return objectList;
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();
            SetItemFromRow(item, row);
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        public static string NumberToWords(this decimal doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)}";
            //var afterFloatingPointWord =
            //    $"{SmallNumberToWord((int)((doubleNumber - beforeFloatingPoint) * 100), "")} paisa";
            return $"{beforeFloatingPointWord}";
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 10000000 > 0)
            {
                words += NumberToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }

            if (number / 100000 > 0)
            {
                words += NumberToWords(number / 100000) + " lac ";
                number %= 100000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }

        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if (number % 10 > 0)
                    words += "-" + unitsMap[number % 10];
            }
            return words;
        }

        public static Expression<T> AndAlso<T>(this Expression<T> left, Expression<T> right)
        {
            return Expression.Lambda<T>(Expression.AndAlso(left, right), left.Parameters);
        }

        //public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        //{
        //    return first.Compose(second, Expression.And);
        //}

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public static int GetWeekNo(DateTime date, int startMonth)
        {
            if (date.Month >= startMonth)
            {
                return (int)((date - new DateTime(date.Year, startMonth, 1)).TotalDays / 7) + 1;
            }
            else
            {
                return (int)((date - new DateTime(date.Year - 1, startMonth, 1)).TotalDays / 7) + 1;
            }
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static string ToTimeString(this TimeSpan timeSpan)
        {
            DateTime time = DateTime.Today.Add(timeSpan);
            return time.ToString("hh:mm:ss tt");
        }

        public static string GenerateUserAccessToken()
        {
            int length = 10;
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string specialCharacters = "!@#$%*_+?:";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            result.Insert(random.Next(9), specialCharacters[random.Next(specialCharacters.Length)]);
            return result.ToString();
        }

        public static string ToFinalString(this string query)
        {
            string finalSql = query.Contains("''")
                  ? query.Replace("''", "NULL")
                  : query;
            return finalSql;
        }

        public static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates;
        }

        public static int GetWeekNo(DateTime date)
        {
            date = date.Date;
            int i;
            for (i = 1; i <= 5; i++)
            {
                if (date.Day <= i * 7)
                {

                    break;
                }
            }
            return i;
        }

        public static DateTime GetDateByWeekNoWithDayOfWeek(DateTime startDate, int weekNumOfMonth, int dayOfWeek)
        {
            var fistdayOfNextMonth = new DateTime(startDate.Year, startDate.Month + 1, 1);
            int day = weekNumOfMonth * 7 - 1 + dayOfWeek + 1 - (int)fistdayOfNextMonth.DayOfWeek;
            return fistdayOfNextMonth.AddDays(day >= 0 ? day : day + 7);
        }
    }
}
