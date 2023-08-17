using ProjectM;
using ProjectM.Scripting;
using System;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Bloodstone.API;

namespace DropNonTeleportables 
{
    public class DropNonTeleportablesClient 
    {
        private static DateTime _lastDropTime = DateTime.Now;

        public static void Reset()
        {
            _lastDropTime = DateTime.Now;
        }

        public static void HandleInput(GameplayInputSystem __instance)
        {
            if (!VWorld.IsClient)
            {
                return;
            }

            if ((Input.GetKeyInt(Plugin.configKeybinding.Primary) || Input.GetKeyInt(Plugin.configKeybinding.Secondary)) && DateTime.Now - _lastDropTime > TimeSpan.FromSeconds(0.5))
            {
                _lastDropTime = DateTime.Now;
                Plugin.Logger.LogInfo("You pressed me!");
                Plugin.Logger.LogInfo(VWorld.Client.IsServerWorld());
                DropItems();
            }
        }

        private static void DropItems()
        {
            var entityManager = VWorld.Client.EntityManager;
            var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
            var clientGameManager = VWorld.Client.GetExistingSystem<ClientScriptMapper>()._ClientGameManager;
            var localCharacter = clientGameManager.GetLocalCharacter();
            var itemInfoStuff = gameDataSystem.ItemHashLookupMap;

            var inventoryEntities = new NativeList<Entity>(Allocator.Temp);
            InventoryUtilities.TryGetInventoryEntities(entityManager, localCharacter, ref inventoryEntities);
            foreach (var inventoryEntity in inventoryEntities)
            {
                if(!InventoryUtilities.HasItemOfCategory(entityManager, itemInfoStuff, inventoryEntity, ItemCategory.TeleportBound))
                {
                    return;
                }

                InventoryUtilities._TryGetInventoryFromInventoryEntity(entityManager, inventoryEntity, out NativeArray<InventoryBuffer> inventory);
                for (int i=0; i < inventory.Length; i++) 
                {
                    if(!InventoryUtilities.TryGetItemAtSlot(inventory, i, out InventoryBuffer item))
                    {
                        continue;
                    }

                    if (itemInfoStuff[item.ItemType].ItemCategory.ToString().Contains(ItemCategory.TeleportBound.ToString()))
                    {
                        EventHelper.TryDropInventoryItem(entityManager, inventoryEntity, i);
                    }
                }     

                inventory.Dispose();          
            }

            inventoryEntities.Dispose();
        }
    }
}