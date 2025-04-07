using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Player player = GameObject.Find("Player(Clone)").GetComponent<Player>();
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (this.gameObject.name == "Inventory Slot(Clone)" && transform.childCount == 0) 
        {
            Debug.Log("Dropped item: " + inventoryItem.card.cardName);
            inventoryItem.parentAfterDrag = transform;
            return;
        }
        Equipment equipment = inventoryItem.card as Equipment;
        if (equipment == null || this.gameObject.name == "Inventory Slot(Clone)") {
            return;
        }
        else 
        {
            EquipmentType slotType;
            switch (this.gameObject.name){
                case "Helmet":
                    slotType = EquipmentType.Helmet;
                    break;
                case "Chestpiece":
                    slotType = EquipmentType.Chestpiece;
                    break;
                case "Boots":
                    slotType = EquipmentType.Boots;
                    break;
                case "Shield":
                    slotType = EquipmentType.Shield;
                    break;
                case "Accessory":
                    slotType = EquipmentType.Accessory;
                    break;
                case "Weapon":
                    slotType = EquipmentType.Weapon;
                    break;
                case "Bow":
                    slotType = EquipmentType.Bow;
                    break;
                default:
                    Debug.LogError("Unknown slot type: " + this.gameObject.name);
                    return;
            }
            if (equipment.equipmentType != slotType)
            {
                return;
            }
            else if (transform.childCount == 0){
                inventoryItem.parentAfterDrag = transform;
                player.Equip(equipment);
            }
            else {
                InventoryItem currentInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
                currentInventoryItem.parentAfterDrag = inventoryItem.parentAfterDrag;
                currentInventoryItem.transform.SetParent(inventoryItem.parentAfterDrag);
                inventoryItem.parentAfterDrag = transform;
                player.Equip(equipment);
            }
        }
    }
}
