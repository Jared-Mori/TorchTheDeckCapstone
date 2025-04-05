using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardPile;
using System.Numerics;
using TMPro;
using UnityEngine.UI;

public class PileController : PileBehaviour
{
    List<Card> hand = new List<Card>();
    public SpriteManager spriteManager;
    private void Update()
    {
        UpdatePile();
    }

    protected override void OnNodeAdded(int index)
    {
        
    }

    protected override void OnNodeRemoved(int index)
    {

    }

    protected override void OnNodeRemoving(int index)
    {

    }

    public void AddCard(Card card)
    {
        AddNode();
        UpdatePile();
        hand.Add(card);
        UpdateHand();
    }

    public void UpdateHand()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            CardWrapper cardWrapper = GetNodeObject(i).GetComponent<CardWrapper>();
            if (cardWrapper != null)
            {
                cardWrapper.SetCard(hand[i]);
            }
            SetCardDisplay(GetNodeObject(i), hand[i].cardName, hand[i].description, spriteManager.GetSprite(hand[i].cardName));
        }
    }

    public static void SetCardDisplay(GameObject cardObject, string name, string desc, Sprite sprite)
    {
        TextMeshProUGUI title = cardObject.transform.Find("TopBanner").transform.Find("NameDisplay").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI description = cardObject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>();
        Image image = cardObject.transform.Find("Icon").GetComponent<Image>();

        image.sprite = sprite;
        title.text = name;
        description.text = desc;
    }
}