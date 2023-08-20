using ProjectM.UI;
using HarmonyLib;
using UnityEngine.Events;
using TMPro;
using UnityEngine;
using System.Linq;

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
                testStunButton.name = "DropTeleportBound";
                testStunButton.onClick.AddListener((UnityAction)Clicked_DropTeleportBoundItems);
                testButtonGO.transform.SetParent(testButtonParent.transform);
                testButtonGO.transform.SetAsLastSibling();
                var buttonRectTransform = testButtonGO.GetComponent<RectTransform>();
                buttonRectTransform.SetAsLastSibling();
                buttonRectTransform.SetPivot(PivotPresets.MiddleLeft);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(InventorySubMenu), nameof(InventorySubMenu.OnUpdateFromParent))]
        private static void OnUpdateFromParent(InventorySubMenu __instance)
        {
            SimpleStunButton dropTeleportBoundButton = null;
            foreach(var stunButton in __instance.GetComponentsInChildren<SimpleStunButton>(true).Where(stunButton => stunButton.name == "DropTeleportBound"))
            {
                dropTeleportBoundButton = stunButton;
            }
            
            if (Input.GetMouseButton(1))
            {
                if (dropTeleportBoundButton != null)
                {
                    dropTeleportBoundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Drop Other\nItems";
                }
            }
            else
            {
                dropTeleportBoundButton.GetComponentInChildren<TextMeshProUGUI>().text = "Drop Teleport\nBound";
            }
        }

        private static void Clicked_DropTeleportBoundItems() 
        {
            if (!Input.GetMouseButton(1))
            {
                DropNonTeleportablesClient.TryDropTeleportBoundItems(false);
            } 
            else
            {
                DropNonTeleportablesClient.TryDropTeleportBoundItems(true);
            }
        }

        
    }
}
