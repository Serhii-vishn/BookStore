using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace BookStore.BookManager
{
    public class CustomDateTimeConverter : DateTimeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text.ToLower().Contains("century bc"))
            {
                int century = int.Parse(text.ToLower().Replace("th century bc", "").Replace("st century bc", "").Replace("nd century bc", "").Replace("rd century bc", ""));
                int year = (century - 1) * 100;
                return new DateTime(year, 1, 1, new GregorianCalendar());
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
