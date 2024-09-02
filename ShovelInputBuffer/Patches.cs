using HarmonyLib;

namespace ShovelInputBuffer
{
    internal class Patches
    {

        [HarmonyPatch(typeof(Shovel), nameof(Shovel.ItemActivate))]
        [HarmonyPostfix]
        static void PostFixShovelActivate(Shovel __instance)
        {
            if (__instance.isHoldingButton && __instance.reelingUp)
            {
                ShovelInputBuffer.Instance.StartCoroutine(ShovelInputBuffer.Instance.ShovelBuffer(__instance));
                ShovelInputBuffer.Logger.LogDebug("PostFix on Shovel.ItemActivate()");
            }
        }
    }
}
