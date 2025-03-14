using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using UnityEngine.UIElements;
using TMPro;


[assembly: MelonInfo(typeof(Repo_modding.Core), "Repo_modding", "1.0.0", "admin", null)]
[assembly: MelonGame("semiwork", "REPO")]
namespace Repo_modding;
public static class Globals
{
    public static bool UpdatePlayer = false;
}

public class Core : MelonMod
{
    public CheatManager Cheats = new CheatManager();

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Cheats.OnUpdate();
    }

    [HarmonyPatch(typeof(LevelGenerator), "Generate", new Type[] { })]
    public class LevelGeneratorHook
    {
        // hook LevelGenerator.PlayerSpawn()
        // change variable to signal CheatManager to update player object upon level generation
        public static void Postfix()
        {
            // runs after level generation and player spawn
            MelonLogger.Msg("Level Generation Complete");
            Globals.UpdatePlayer = true;
        }
    }


}
