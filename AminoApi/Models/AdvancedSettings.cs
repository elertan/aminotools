using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoApi.Models
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
