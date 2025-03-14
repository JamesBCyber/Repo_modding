using MelonLoader;

[assembly: MelonInfo(typeof(Repo_modding.Core), "Repo_modding", "1.0.0", "admin", null)]
[assembly: MelonGame("semiwork", "REPO")]
namespace Repo_modding;
public static class Globals
{
    public static bool UpdatePlayer = false;
}

public class Core : MelonMod
{

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Initialized.");
        CheatManager.Start();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheatManager.OnUpdate();
    }




}
