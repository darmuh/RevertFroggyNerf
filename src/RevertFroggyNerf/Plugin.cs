using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Mirror;
using YAPYAP;

namespace RevertFroggyNerf
{
    [BepInAutoPlugin]
    public partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }
        internal static ConfigEntry<int> MaxJumpsOverride { get; private set; } = null!;

        private void Awake()
        {
            Log = Logger;

            Log.LogInfo($"Plugin {Name} is loaded with version {Version}!!");
            MaxJumpsOverride = Config.Bind("Settings", "Additional Jumps Cap", 3, new ConfigDescription("Set the maxmium additional jumps with this configuration value.\nCapped at 99 for sanity reasons", new AcceptableValueRange<int>(1, 99)));

            GameManager.OnPlayerSpawned += ModifyMaxJumps;
        }

        private static void ModifyMaxJumps(Pawn pawn)
        {
            if (!NetworkServer.active)
                return;
            
            pawn.maxAdditiveExtraAirJumps = MaxJumpsOverride.Value;
            Log.LogInfo($"Maximum jumps set to {MaxJumpsOverride.Value}");
        }
    }
}
