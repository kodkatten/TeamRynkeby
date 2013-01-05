using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Security;

namespace EventBooking.Models
{
    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
