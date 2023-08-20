using ProjectM;
using ProjectM.Scripting;
using System;
using Unity.Entities;
using Unity.Collections;
using Bloodstone.API;

namespace DropTeleportBound 
{
    public class DropTeleportBoundClient
    {
        private static DateTime _lastDropTime = DateTime.Now;

        public static void Reset()
        {
            _lastDropTime = DateTime.Now;
        }

        public static void TryDropTeleportBoundItems(bool reverse)
        {
            // Client-side mod
            if (!VWorld.IsClient)
            {
                return;
            }

            // Short delay between button presses to allow a previous click to finish processing
            if (DateTime.Now - _lastDropTime > TimeSpan.FromSeconds(0.25))
            {
                _lastDropTime = DateTime.Now;
                DropItems(reverse);
            }
        }

        private static void DropItems(bool reverse)
        {
            // Retrieve necessary variables
            EntityManager entityManager = VWorld.Client.EntityManager;
            GameDataSystem gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
            ClientGameManager clientGameManager = VWorld.Client.GetExistingSystem<ClientScriptMapper>()._ClientGameManager;
            var localCharacter = clientGameManager.GetLocalCharacter();
            NativeHashMap<PrefabGUID,ItemData> itemInfo = gameDataSystem.ItemHashLookupMap;

            // Retrieve character inventories
            NativeList<Entity> inventoryEntities = new(Allocator.Temp);
            InventoryUtilities.TryGetInventoryEntities(entityManager, localCharacter, ref inventoryEntities);
            
            foreach (var inventoryEntity in inventoryEntities)
            {
                // If there are no teleport bound items, nothing to do
                if(!reverse && !InventoryUtilities.HasItemOfCategory(entityManager, itemInfo, inventoryEntity, ItemCategory.TeleportBound))
                {
                    return;
                }

                // Turn inventory into list of InventoryBuffers
                InventoryUtilities._TryGetInventoryFromInventoryEntity(entityManager, inventoryEntity, out NativeArray<InventoryBuffer> inventory);
                
                // For each inventory slot, get the item, check if it meets the drop criteria
                for (int i=0; i < inventory.Length; i++) 
                {
                    if(!InventoryUtilities.TryGetItemAtSlot(inventory, i, out InventoryBuffer item))
                    {
                        continue;
                    }

                    if (reverse ^ itemInfo[item.ItemType].ItemCategory.ToString().Contains(ItemCategory.TeleportBound.ToString()))
                    {
                        if (!itemInfo[item.ItemType].ItemCategory.ToString().Contains(ItemCategory.LoseDurabilityOnDeath.ToString()))
                        {
                            EventHelper.TryDropInventoryItem(entityManager, inventoryEntity, i);
                        }
                    }
                }     
                // Cleanup
                inventory.Dispose();          
            }
            // Cleanup
            inventoryEntities.Dispose();
        }
    }
}