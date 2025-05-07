using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Timers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DiscordSuperAdvertisement
{
    [MinimumApiVersion(80)]
    public class DiscordSuperAdvertisement : BasePlugin, IPluginConfig<DiscordSuperAdvertisementConfig>
    {
        public override string ModuleName => "Discord Super Advertisement";
        public override string ModuleDescription => "Advertises the Discord server in the game chat.";
        public override string ModuleAuthor => "E!N";
        public override string ModuleVersion => "v1.0.0";

        public DiscordSuperAdvertisementConfig Config { get; set; } = new();

        private static readonly HttpClient _httpClient = new();

        public void OnConfigParsed(DiscordSuperAdvertisementConfig config)
        {
            Config = config;
        }

        public override void Load(bool hotReload)
        {
            AddTimer(Config.AD_Interval, ADV, TimerFlags.REPEAT);
        }

        private async void ADV()
        {
            string url = $"https://discord.com/api/v9/invites/{Config.InviteCode}?with_counts=true&with_expiration=true";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();

                JObject data = JObject.Parse(responseData);

                int approximatePresenceCount = data.Value<int?>("approximate_presence_count") ?? 0;
                int approximateMemberCount = data.Value<int?>("approximate_member_count") ?? 0;

                Server.NextFrame(() =>
                {
                    Server.PrintToChatAll(Localizer["DisAdv", approximatePresenceCount, approximateMemberCount, Config.InviteCode]);
                });
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError($"UNSUCCESSFUL REQUEST: {ex.Message}");
            }
        }
    }
}
