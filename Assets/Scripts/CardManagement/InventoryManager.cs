using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryPanel;
    public PileController pileController;
    public Player player;
    public bool inventoryOpen = false;

    void Start()
    {
        inventoryPanel = GameObject.Find("MainInventoryGroup");
        inventoryPanel.SetActive(false);
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
