using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Cloud_Storage.Classes.Handlers
{
    public static class JsonHandler
    {

        private static readonly string _keyPattern = "\"(.*?)\":";
        private static readonly string _valuePattern = ":\"(.)+\"";

        public static Dictionary<object, object> ConvertJsonToDictionary(object inputJSON)
        {

            string? jsonToConvert = inputJSON.ToString();

            if (!string.IsNullOrEmpty(jsonToConvert))
            {

                MatchCollection keys = Regex.Matches(jsonToConvert, _keyPattern);
                MatchCollection values = Regex.Matches(jsonToConvert, _valuePattern);

                return FormDictionaryOfValues(keys, values);

            }

            return new Dictionary<object, object>();

        }

        public static string GetKeyByMatchObject(MatchCollection matches, int index)
        {
            return matches[index].ToString()
                .Substring(1, matches[index].Length - 3);
        }

        public static string GetValueByMatchObject(MatchCollection matches, int index)
        {
            return matches[index].ToString()
                .Substring(2, matches[index].Length - 3);
        }

        public static Dictionary<object, object> FormDictionaryOfValues(params MatchCollection[] args)
        {

            Dictionary<object, object> result = new Dictionary<object, object>();

            for (int i = 0; i < args[0].ToList().Count; i++)
            {

                result[GetKeyByMatchObject(args[0], i)] =
                    GetValueByMatchObject(args[1], i);

            }

            return result;

        }

    }

}
