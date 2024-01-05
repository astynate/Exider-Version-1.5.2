namespace Cloud_Storage.Classes.Handlers
{
    public static class DateHandler
    {
        private static string[] _months = new string[12] {

            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"

        };

        public static string ConvertToUSADateFormat(string? date)
        {

            string result = "";

            if (date != null) 
            {

                try
                {

                    string day = date[0..2];
                    string month = _months[Convert.ToUInt32(date[3..5]) - 1];
                    string year = date[6..10];
                    string time = date[11..16];

                    result = $"{month} {day}, {year}";

                }

                catch (Exception ex)
                { 
                
                    Console.WriteLine(ex.Message);

                }

            }

            return result;

        }

        public static string ConvertToUSATimeFormat(string? date)
        {

            string result = "";

            if (date != null)
            {

                try
                {

                    string time = date.Split(" ")[1];
                    result = time.Split(":")[0] + ":" + time.Split(":")[1];

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return result;

        }

    }

}
