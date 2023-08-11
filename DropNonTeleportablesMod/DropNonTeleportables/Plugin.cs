﻿using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Bloodstone.API;

namespace DropNonTeleportables
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.Bloodstone")]
    [BepInDependency("gg.deca.VampireCommandFramework", BepInDependency.DependencyFlags.SoftDependency)]
    [Reloadable]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;
        public static bool VCF;

        public static Keybinding configKeybinding;
        private Harmony _hooks;

        private static void InitConfig()
        {
            configKeybinding = KeybindManager.Register(new()
            {
                Id = "ministerfrolo.vrisingmods.dropnonteleportables",
                Category = "DropNonTeleportables",
                Name = "Drop",
                DefaultKeybinding = KeyCode.G,
            });
        }

        public override void Load()
        {
            Logger = Log;
            VCF = IL2CPPChainloader.Instance.Plugins.ContainsKey("gg.deca.VampireCommandFramework");
            Logger.LogInfo($"***** VCF : {VCF}");

            InitConfig();
            if (VCFWrapper.Enabled) VCFWrapper.RegisterAll();
            DropNonTeleportablesClient.Reset();

            _hooks = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        public override bool Unload()
        {
            Config.Clear();
            KeybindManager.Unregister(configKeybinding);
            _hooks.UnpatchSelf();
            return true;
        }
    }
}
