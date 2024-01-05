namespace Cloud_Storage.Classes.Adapters
{
    public static class MessageAdapter
    {

        public static string ConvertNewlineFromHtmlToCSharp(string message) //Regex.Replace(message, @"(.)*<br>", "\"$1\"")
            => message.Replace("<br>", "\r\n");

    }

}
