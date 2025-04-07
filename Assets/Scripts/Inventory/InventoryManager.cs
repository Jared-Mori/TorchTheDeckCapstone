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
        pileController = inventoryPanel.transform.Find("Deck").GetComponent<PileController>();

        inventoryPanel.SetActive(false);
    }

    public void OpenInventory(){
        inventoryOpen = true;
        PauseGame();
        inventoryPanel.SetActive(true);
        foreach(Card card in player.deck){
            for(int i = 0; i < card.count; i++){
                pileController.AddCard(card);
            }
        }
    }

    public void CloseInventory(){
        inventoryPanel.SetActive(false);
        for(int i = 0; i < pileController.hand.Count; i++){
            pileController.RemoveNode();
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
