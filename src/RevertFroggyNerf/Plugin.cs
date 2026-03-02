using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
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
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Log.LogInfo($"Plugin {Name} is loaded with version {Version}! (Now required by all clients)");
            MaxJumpsOverride = Config.Bind("Settings", "Additional Jumps Cap", 3, new ConfigDescription("Set the maxmium additional jumps with this configuration value.\nCapped at 99 for sanity reasons", new AcceptableValueRange<int>(1, 99)));
        }

        // Had to switch to a standard harmony patch so that the value would *actually* change for client pawns.
        // I'm guessing either the event wasn't running for the clients or it was running too early (dont really care to investigate further)
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.Start))]
        public class MirrorNetManHook
        {
            public static void Prefix(Pawn __instance)
            {
                ModifyMaxJumps(__instance);
            }
        }

        private static void ModifyMaxJumps(Pawn pawn)
        {
            // for whatever reason, I can no longer make this a host only mod.
            // local clients now have authority over their maxAdditiveExtraAirJumps (which I think is a mistake)
            // but i'm not sure what even changed for this to be the case now.
            // There's not really any good public resources to see what in the codebase has changed anyway.
            // Anyway, the below will run for every pawn when you are the host, and only your pawn when you are the client
            if (!NetworkServer.active && !pawn.isLocalPlayer)
                return;
            
            pawn.maxAdditiveExtraAirJumps = MaxJumpsOverride.Value;
            Log.LogInfo($"Maximum jumps set to {MaxJumpsOverride.Value}");
        }
    }
}
