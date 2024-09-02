using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace ShovelInputBuffer
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class ShovelInputBuffer : BaseUnityPlugin
    {
        public const string GUID = "MasterAli2.ShovelInputBuffer";
        public const string NAME = "ShovelInputBuffer";
        public const string VERSION = "1.0.0";

        public static ShovelInputBuffer Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }



        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            Patch();
            
            Logger.LogInfo($"{GUID} v{VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll(typeof(Patches));

            Logger.LogDebug("Finished patching!");
        }

        internal IEnumerator ShovelBuffer(Shovel shovel)
        {
            yield return new WaitUntil(() => !shovel.reelingUp);

            if (!shovel.isHoldingButton) yield break;
            
            Logger.LogInfo("Buffering shovel input");
            shovel.reelingUp = true;
            shovel.previousPlayerHeldBy = shovel.playerHeldBy;
            if (shovel.reelingUpCoroutine != null) StopCoroutine(shovel.reelingUpCoroutine);
            shovel.reelingUpCoroutine = StartCoroutine(shovel.reelUpShovel());
        }
    }
}
