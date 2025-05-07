using CounterStrikeSharp.API.Core;

namespace DiscordSuperAdvertisement
{
    public class DiscordSuperAdvertisementConfig : BasePluginConfig
    {
        public float AD_Interval { get; set; } = 30.0f;
        public string InviteCode { get; set; } = "";
    }
}
