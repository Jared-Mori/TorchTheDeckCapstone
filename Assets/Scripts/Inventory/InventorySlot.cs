using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (this.gameObject.name == "Inventory Slot(Clone)" && transform.childCount == 0) 
        {
            Debug.Log("Dropped item: " + inventoryItem.card.cardName);
            inventoryItem.parentAfterDrag = transform;
            return;
        }
        else if (inventoryItem.card is not Equipment || this.gameObject.name == "Inventory Slot(Clone)") {
            Debug.Log("Failed to drop item: " + this.gameObject.name);
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
                case "Accessory":
                    slotType = EquipmentType.Accessory;
                    break;
                default:
                    return;
            }
            Equipment equipment = inventoryItem.card as Equipment;
            if (equipment.equipmentType == slotType)
            {
                Debug.Log("Equipped item: " + inventoryItem.card.cardName);
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }
}
