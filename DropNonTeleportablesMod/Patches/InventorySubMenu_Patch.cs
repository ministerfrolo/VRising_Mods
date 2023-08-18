using Bloodstone.API;
using ProjectM;
using ProjectM.UI;
using System;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.UI;
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
            var testButtonName = "test button";
            SimpleStunButton smartMergeButton = null;
            foreach(var stunButton in __instance.GetComponentsInChildren<SimpleStunButton>(true).Where(stunButton => stunButton.name == "SmartMergeButton"))
            {
                smartMergeButton = stunButton;
            }
            Plugin.Logger.LogInfo("smartMergeButton is " + smartMergeButton.name);
            var smartMergeButtonGO = smartMergeButton.gameObject;
            Plugin.Logger.LogInfo("smartMergeButtonGO is " + smartMergeButtonGO.name);
            var buttonContainer = smartMergeButton.transform.parent.gameObject;
            Plugin.Logger.LogInfo("buttonContainer is " + buttonContainer.name);
            var testButton = buttonContainer.transform.Find(testButtonName)?.gameObject;
            if (testButton == null)
            {
                Plugin.Logger.LogInfo("testButton was null");
                testButton = GameObject.Instantiate(smartMergeButtonGO);
                testButton.name = "Drop Teleport Bound";
                var textComp = testButton.GetComponentInChildren<TextMeshProUGUI>().text = "Drop Teleport Bound";
                testButton.SetActive(true);
                testButton.GetComponent<SimpleStunButton>().onClick.AddListener((UnityAction)Blargmerp);
                testButton.transform.SetParent(buttonContainer.transform);
            }
            
            //var blarg = new SimpleStunButton.ButtonClickedEvent();
            
            // if(__instance.name == "CharacterInventorySubMenu2")
            // {
            //     GameObject dntButton = CreateButton("DropNonTeleportablesButton");
            //     Text text = CreateText(dntButton.transform);
            //     text.text = "Drop Teleport Bound";
            //     dntButton.GetComponent<SimpleStunButton>().onClick.AddListener((UnityAction)Blargmerp);
            //     dntButton.transform.SetParent(__instance.ArmorParent.transform);
            // }
            
        }

        private static SimpleStunButton CloneButton(string name, GameObject ogButtonGameObject)
        {
            return null;


            //GameObject button_go = new(name) { };
            // button_go.AddComponent<RectTransform>();
            // button_go.AddComponent<CanvasRenderer>();   
            // button_go.AddComponent<Image>();
            // var stunButton = button_go.AddComponent<SimpleStunButton>();
            // button_go.AddComponent<HorizontalLayoutGroup>();
            // button_go.AddComponent<ContentSizeFitter>();

            // var ogStunButton = ogButtonGameObject.GetComponent<SimpleStunButton>();
            
            // ogButtonGameObject.Instantiate(button_go);


        }

        private static GameObject CreateButton(String s)
        {
            GameObject newButton = new(s);
            newButton.AddComponent<RectTransform>();
            newButton.AddComponent<CanvasRenderer>();
            newButton.AddComponent<Image>();
            newButton.AddComponent<SimpleStunButton>();
            newButton.AddComponent<HorizontalLayoutGroup>();
            newButton.AddComponent<ContentSizeFitter>();
            return newButton;
        }

        private static Text CreateText(Transform parent)
        {
            GameObject go = new();
            go.transform.parent = parent;
            return go.AddComponent<Text>();
        }

        private static void Blargmerp()
        {
            Plugin.Logger.LogInfo("pushed drop button");
        }
    }
}
