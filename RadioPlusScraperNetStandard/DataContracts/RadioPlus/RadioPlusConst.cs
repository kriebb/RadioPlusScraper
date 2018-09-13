namespace WebScrapingProject
{
    public class RadioPlusConst
    {
        private const string RadioPlusOnDemandUrl = "http://radioplus.be/#/{0}/herbeluister/";

        public static string GetRadioPlusOnDemandUrl(string radio)
        {
            string ondemandUrl = string.Format(RadioPlusOnDemandUrl, radio);
            return ondemandUrl;
        }

        public static string Radio1 = "radio1";
        public static string Radio2 = "radio2-vlaams-brabant";
        public static string[] AllRadios = { Radio1, Radio2 };

    }
}