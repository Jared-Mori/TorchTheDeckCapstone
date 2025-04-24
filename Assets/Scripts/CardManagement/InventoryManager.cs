using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryPanel;
    public PileController pileController;
    public GameObject helmet, chest, boots, weapon, bow, accessory, shield;
    public Player player;
    public bool inventoryOpen = false;

    public void SetInventoryPanel()
    {
        inventoryPanel = GameObject.Find("MainInventoryGroup");
        inventoryPanel.SetActive(false);
        helmet.SetActive(false);
        chest.SetActive(false);
        boots.SetActive(false);
        weapon.SetActive(false);
        bow.SetActive(false);
        accessory.SetActive(false);
        shield.SetActive(false);
    }

    public void OpenInventory(){
        Debug.Log("Opening inventory");
        inventoryOpen = true;
        PauseGame();
        inventoryPanel.SetActive(true);
    }

    public void CloseInventory(){
        Debug.Log("Closing inventory");
        inventoryPanel.SetActive(false);
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
