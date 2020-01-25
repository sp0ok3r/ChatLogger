using Newtonsoft.Json;
using System.ComponentModel;

namespace ChatLogger.User2Json
{
    public partial class ChatLoggerSettings
    {
        public bool playsound { get; set; }
        public bool startup { get; set; }

        public bool hideInTaskBar { get; set; }
        public int startupColor{ get; set; }
        public ulong startupAcc { get; set; }
        public bool startMinimized { get; set; }
        public string PathLogs { get; set; }

        [DefaultValue("───────────────────")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Separator { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastTimeCheckedUpdate { get; set; }
    }
}
