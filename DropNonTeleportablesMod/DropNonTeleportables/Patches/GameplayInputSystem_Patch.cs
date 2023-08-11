using HarmonyLib;
using ProjectM;

namespace DropNonTeleportables
{
    [HarmonyPatch]
    public class GameplayInputSystem_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameplayInputSystem), nameof(GameplayInputSystem.HandleInput))]
        private static void HandleInput(GameplayInputSystem __instance)
        {
            DropNonTeleportablesClient.HandleInput(__instance);
        }
    }
}