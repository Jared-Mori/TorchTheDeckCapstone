using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryPanel;
    public GameObject inventory;
    public Player player;
    public bool inventoryOpen = false;

    void Start()
    {
        inventoryPanel = GameObject.Find("MainInventoryGroup");
        inventory = inventoryPanel.transform.Find("Inventory").gameObject;

        for (int i = 0; i < 60; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventory.transform);
        }

        inventoryPanel.SetActive(false);
    }

    public void OpenInventory(){
        inventoryOpen = true;
        PauseGame();
        inventoryPanel.SetActive(true);
        foreach(Card card in player.deck){
            foreach(Transform slot in inventory.transform){
                if(slot.transform.childCount == 0){
                    GameObject item = Instantiate(inventoryItemPrefab, slot.transform);
                    item.GetComponent<InventoryItem>().SetCard(card);
                    break;
                }
            }
        }
    }

    public void CloseInventory(){
        inventoryPanel.SetActive(false);
        foreach(Transform slot in inventory.transform){
            if(slot.transform.childCount > 0){
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
        ResumeGame();
        inventoryOpen = false;
    }

    public void PauseGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }

    public void SetPlayer(Player player){
        this.player = player;
    }
}
