using MelonLoader;
using HarmonyLib;

namespace Repo_modding
{
    [HarmonyPatch(typeof(LevelGenerator), "Generate", new Type[] { })]
    public class LevelGeneratorHook
    {
        // hook LevelGenerator.PlayerSpawn()
        // change variable to signal CheatManager to update player object upon level generation
        public static void Postfix()
        {
            // runs after level generation and player spawn
            CheatManager.UpdatePlayerSignal = true;
            MelonLogger.Msg("Level Generation Complete");
        }
    }
}
