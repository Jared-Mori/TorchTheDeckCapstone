using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D;
using DG.Tweening;

public class PileController : MonoBehaviour
{
    [SerializeField]
    public List<Card> hand = new List<Card>();
    public SpriteManager spriteManager;
    [SerializeField] private GameObject cardPrefab; // Prefab for the card display
    [SerializeField] public SplineContainer splineHand; // Parent object for card displays
    [SerializeField] private RectTransform SpawnPoint; // Prefab for the card display
    private List<GameObject> cardDisplays = new List<GameObject>(); // List to hold card display objects

    public void AddCard(Card card)
    {
        hand.Add(card);
        GameObject c = GameObject.Instantiate(cardPrefab, SpawnPoint.position, SpawnPoint.rotation);
        c.GetComponent<RectTransform>().SetParent(splineHand.transform, true); // Set the parent of the card display to the spawn point
        c.transform.localScale = Vector3.one * 2.5f; // Reset the scale of the card display
        cardDisplays.Add(c); // Add the new card display to the list
        UpdateHand(); // Update the hand display
    }

    public void RemoveCard(int index)
    {
        hand.RemoveAt(index); // Remove the card from the hand list
        Destroy(cardDisplays[index]);
        cardDisplays.RemoveAt(index);
        UpdateHand();             // Update the hand display
    }

    public void UpdateHand()
    {
        float spacing = 1f / hand.Count; // Spacing between cards
        float firstCardPosition = 0.5f - (hand.Count - 1) * spacing / 2; // Calculate the first card position
        UnityEngine.Splines.Spline spline = splineHand.Spline; // Get the spline from the SplineContainer
        for (int i = 0; i < hand.Count; i++)
        {
            CardWrapper cardWrapper = cardDisplays[i].GetComponent<CardWrapper>();
            cardWrapper.SetCard(hand[i]); // Set the card in the CardWrapper component
            SetCardDisplay(cardDisplays[i]); // Set the card display properties
            float p = firstCardPosition + i * spacing; // Calculate the normalized position on the spline
            Vector3 position = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculate the rotation based on the forward and up vectors
            cardDisplays[i].transform.DOLocalMove(position, 0.25f); // Set the position of the card display
            cardDisplays[i].transform.DOLocalRotateQuaternion(rotation, 0.25f); // Set the rotation of the card display
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
            Image statusImage = status.GetComponent<Image>();
            statusImage.sprite = sm.GetSprite(((IStatusEffect)card).status.statusName);
            status.SetActive(true);
        }
        else
        {
            GameObject status = cardObject.transform.Find("Status").gameObject;
            status.SetActive(false);
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
                GameObject equippedCardObject = cardDisplays[equippedIndex];
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