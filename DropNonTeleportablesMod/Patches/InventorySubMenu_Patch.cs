using ProjectM.UI;
using HarmonyLib;
using UnityEngine.Events;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.TextCore;

namespace DropNonTeleportables
{
    [HarmonyPatch]
    public class InventorySubMenu_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(InventorySubMenu), nameof(InventorySubMenu.StartRunning))]
        private static void StartRunning(InventorySubMenu __instance)
        {
            Plugin.Logger.LogInfo("instance name is " + __instance.name);
            var testButtonName = "DropTeleportBound";
            SimpleStunButton smartMergeButton = null;
            foreach(var stunButton in __instance.GetComponentsInChildren<SimpleStunButton>(true).Where(stunButton => stunButton.name == "SmartMergeButton"))
            {
                smartMergeButton = stunButton;
            }

            if(smartMergeButton == null) { return;}

            Plugin.Logger.LogInfo("smartMergeButton is " + smartMergeButton.name);
            var smartMergeButtonGO = smartMergeButton.gameObject;
            Plugin.Logger.LogInfo("smartMergeButtonGO is " + smartMergeButtonGO.name);
            var testButtonParent = __instance.BagsParent;
            var testButtonGO = testButtonParent.transform.Find(testButtonName)?.gameObject;
            if (testButtonGO == null)
            {
                Plugin.Logger.LogInfo("testButton was null");
                testButtonGO = GameObject.Instantiate(smartMergeButtonGO);
                testButtonGO.name = testButtonName;
                var textComp = testButtonGO.GetComponentInChildren<TextMeshProUGUI>().text = "Drop Teleport\nBound";
                var testStunButton = testButtonGO.GetComponent<SimpleStunButton>();
                testStunButton.onClick.AddListener((UnityAction)ClickedDropTeleportBoundItems);
                testButtonGO.transform.SetParent(testButtonParent.transform);
                testButtonGO.transform.SetAsLastSibling();
                var buttonRectTransform = testButtonGO.GetComponent<RectTransform>();
                buttonRectTransform.SetAsLastSibling();
                var buttonHorizontalLayoutGroup = testButtonGO.GetComponent<HorizontalLayoutGroup>();
                buttonRectTransform.SetPivot(PivotPresets.MiddleLeft);
            }
        }

        private static void ClickedDropTeleportBoundItems() 
        {
            DropNonTeleportablesClient.TryDropTeleportBoundItems();
        }
    }
}
