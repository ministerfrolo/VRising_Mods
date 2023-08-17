using Bloodstone.API;
using ProjectM;
using ProjectM.UI;
using System;
using HarmonyLib;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

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
            var smartMergeButton = AccessTools.FieldRefAccess<InventorySubMenu, SimpleStunButton>(__instance, "SmartMergeButton").gameObject;
            var buttonContainer = smartMergeButton.transform.parent.gameObject;
            var testButton = buttonContainer.transform.Find(testButtonName)?.gameObject;
            if (testButton == null)
            {
                //testButton = CloneButton(testButtonName, smartMergeButton.gameObject);
                testButton = GameObject.Instantiate(smartMergeButton);
                testButton.GetComponent<SimpleStunButton>().onClick.AddListener((UnityAction)Blargmerp);
                testButton.transform.parent = buttonContainer.transform;
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
