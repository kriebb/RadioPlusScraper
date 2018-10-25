namespace Uncas.Web
{
    public class FeedEmailAddress
    {
        public FeedEmailAddress(string emailAddress, string realName)
        {
            Feed.CheckRequiredValue(emailAddress, "emailAddress");
            Feed.CheckRequiredValue(realName, "realName");
            EmailAddress = emailAddress;
            RealName = realName;
        }

        public string EmailAddress { get; set; }
        public string RealName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", EmailAddress, RealName);
        }
    }
}