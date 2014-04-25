using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicSandBox
{
    public class Installation : Rush.RushObject
    {
        public int Badge { get; set; }
        public string[] Channels { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string InstallationId { get; set; }
        public string TimeZone { get; set; }
    }
}
