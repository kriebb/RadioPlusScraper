using System;
using System.Collections.Generic;
using System.Text;

namespace Uncas.Web
{
    public class FeedEmailAddress
    {
        public FeedEmailAddress(string emailAddress, string realName)
        {
            Feed.CheckRequiredValue(emailAddress, "emailAddress");
            Feed.CheckRequiredValue(realName, "realName");
            this.EmailAddress = emailAddress;
            this.RealName = realName;
        }

        public string EmailAddress { get; set; }
        public string RealName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.EmailAddress, this.RealName);
        }
    }
}
