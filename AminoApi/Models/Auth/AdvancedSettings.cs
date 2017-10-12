using System;
using System.Collections.Generic;

namespace AminoApi.Models.Auth
{
    public class AdvancedSettings : ModelBase
    {
        public bool AmplitudeAnalyticsEnabled { get; set; }
        public string AmplitudeAppId { get; set; }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            AmplitudeAnalyticsEnabled = Convert.ToBoolean(data["amplitudeAnalyticsEnabled"]);
            AmplitudeAppId = Convert.ToString(data["amplitudeAppId"]);
        }
    }
}
