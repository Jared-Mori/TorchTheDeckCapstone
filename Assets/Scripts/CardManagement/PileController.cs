using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardPile;
using System.Numerics;
using TMPro;
using UnityEngine.UI;

public class PileController : PileBehaviour
{
    public List<Card> hand = new List<Card>();
    public SpriteManager spriteManager;
    private void Update()
    {
        
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

    public void RemoveCard(int index)
    {
        hand.RemoveAt(index); // Remove the card from the hand list
        RemoveNode();             // Remove the corresponding node
        UpdatePile();             // Update the pile layout
        UpdateHand();             // Update the hand display
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
            SetCardDisplay(GetNodeObject(i));
        }
    }

    public static void SetCardDisplay(GameObject cardObject)
    {
        Card card = cardObject.GetComponent<CardWrapper>().card;
        SpriteManager sm = GameObject.Find("Sprite Manager").GetComponent<SpriteManager>();

        TextMeshProUGUI title = cardObject.transform.Find("TopBanner").transform.Find("NameDisplay").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI description = cardObject.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI uses = cardObject.transform.Find("Uses").transform.Find("UsesText").GetComponentInChildren<TextMeshProUGUI>();
        Image image = cardObject.transform.Find("Icon").GetComponent<Image>();

        if (card is Weapon)
        {
            GameObject damage = cardObject.transform.Find("Damage").gameObject;
            TextMeshProUGUI damageText = damage.transform.Find("DamageText").GetComponentInChildren<TextMeshProUGUI>();
            damageText.text = ((Weapon)card).damage.ToString();
            damage.SetActive(true);
            
        }
        else if (card is Arrow)
        {
            GameObject arrow = cardObject.transform.Find("Arrow").gameObject;
            TextMeshProUGUI arrowText = arrow.transform.Find("ArrowText").GetComponentInChildren<TextMeshProUGUI>();
            arrowText.text = ((Arrow)card).damage.ToString();
            arrow.SetActive(true);
        }
        else if (card is HealingItem)
        {
            GameObject healing = cardObject.transform.Find("Healing").gameObject;
            TextMeshProUGUI healingText = healing.transform.Find("HealingText").GetComponentInChildren<TextMeshProUGUI>();
            healingText.text = ((HealingItem)card).healing.ToString();
            healing.SetActive(true);
        }

        if (card is IStatusEffect)
        {
            GameObject status = cardObject.transform.Find("Status").gameObject;
            Image statusImage = status.transform.Find("StatusImage").GetComponent<Image>();
            statusImage.sprite = sm.GetSprite(((IStatusEffect)card).status.statusName);
            status.SetActive(true);
        }

        image.sprite = sm.GetSprite(card.cardName);
        title.text = card.cardName;
        description.text = card.description;
        uses.text = card.uses.ToString();
    }

    public CardWrapper GetEquippedCard(ItemType itemType)
    {
        int equippedIndex;
        foreach (Card card in hand)
        {
            if (card.itemType == itemType)
            {
                equippedIndex = hand.IndexOf(card);
                GameObject equippedCardObject = GetNodeObject(equippedIndex);
                CardWrapper cardWrapper = equippedCardObject.GetComponent<CardWrapper>();
                if (cardWrapper != null)
                {
                    return cardWrapper; // Return the card wrapper of the equipped card
                }
            }
        }
        return null; // No equipped card found of the specified type
    }
}