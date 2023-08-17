using BepInEx.Unity.IL2CPP;
using ProjectM.Network;
using VampireCommandFramework;

namespace DropNonTeleportables
{
    public static class VCFWrapper
    {
        public static bool Enabled
        {
            get
            {
                return IL2CPPChainloader.Instance.Plugins.ContainsKey("gg.deca.VampireCommandFramework");
            }
        }

        public static void RegisterAll()
        {
            CommandRegistry.RegisterAll();
        }
    }
}