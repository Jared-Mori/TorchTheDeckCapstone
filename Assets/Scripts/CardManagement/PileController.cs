using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class PileController : MonoBehaviour
{
    [SerializeField]
    public List<Card> hand = new List<Card>();
    public SpriteManager spriteManager;
    [SerializeField] private GameObject cardPrefab, enemyCardPrefab; // Prefab for the card display
    [SerializeField] public SplineContainer splineHand, enemySpline; // Parent object for card displays
    [SerializeField] private RectTransform SpawnPoint, enemySpawnPoint; // Prefab for the card display
    private List<GameObject> cardDisplays = new List<GameObject>(); // List to hold card display objects
    private List<GameObject> enemyCardDisplays = new List<GameObject>(); // List to hold enemy card display objects

    public async Task AddCard(Card card)
    {
        hand.Add(card);
        GameObject c = GameObject.Instantiate(cardPrefab, SpawnPoint.position, SpawnPoint.rotation);
        c.GetComponent<RectTransform>().SetParent(splineHand.transform, true); // Set the parent of the card display to the spawn point
        c.transform.localScale = Vector3.one * 2.5f; // Reset the scale of the card display
        cardDisplays.Add(c); // Add the new card display to the list
        await UpdateHandAsync(); // Update the hand display
    }

    public async Task RemoveCard(int index)
    {
        if (cardDisplays[index] != null)
        {
            DOTween.Kill(cardDisplays[index].transform); // Kill all tweens targeting this object
            Destroy(cardDisplays[index]); // Destroy the card display
        }

        hand.RemoveAt(index); // Remove the card from the hand list
        cardDisplays.RemoveAt(index); // Remove the card display from the list
        await UpdateHandAsync(); // Update the hand display
    }

    public async Task AdjustEnemyHand(CombatDetails enemy)
    {
        while (enemyCardDisplays.Count < enemy.deck.Count)
        {
            GameObject c = GameObject.Instantiate(enemyCardPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            c.GetComponent<RectTransform>().SetParent(enemySpline.transform, true); // Set the parent of the card display to the spawn point
            c.transform.localScale = Vector3.one * 2.5f; // Reset the scale of the card display
            enemyCardDisplays.Add(c); // Add the new card display to the list
        }
        while (enemyCardDisplays.Count > enemy.deck.Count)
        {
            Destroy(enemyCardDisplays[enemyCardDisplays.Count - 1]); // Destroy the last card display
            enemyCardDisplays.RemoveAt(enemyCardDisplays.Count - 1); // Remove it from the list
        }

        await UpdateEnemyHandAsync(enemy); // Update the enemy hand display
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
            cardDisplays[i].transform.SetAsLastSibling(); // Move the card display to the last sibling in the hierarchy
            float p = firstCardPosition + i * spacing; // Calculate the normalized position on the spline
            Vector3 position = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculate the rotation based on the forward and up vectors
            cardDisplays[i].GetComponent<Image>().raycastTarget = false;
            CardMove(cardDisplays[i].transform, position, rotation, hand.Count); // Move the card display to the new position and rotation
            cardDisplays[i].GetComponent<Image>().raycastTarget = true;
        }
    }

    public void CardMove(Transform card, Vector3 position, Quaternion rotation, int count)
    {
        if (count <= 15)
        {
            card.DOLocalMove(position, 0.25f).SetUpdate(true).SetAutoKill(true); // Set the position of the card display
            card.DOLocalRotateQuaternion(rotation, 0.25f).SetUpdate(true).SetAutoKill(true); // Set the rotation of the card display
        }
        else
        {
            card.transform.localPosition = position; // Set the position of the card display
            card.transform.localRotation = rotation; // Set the rotation of the card display
        }
    }

    public async Task UpdateHandAsync()
    {
        float spacing = 1f / hand.Count; // Spacing between cards
        float firstCardPosition = 0.5f - (hand.Count - 1) * spacing / 2; // Calculate the first card position
        UnityEngine.Splines.Spline spline = splineHand.Spline; // Get the spline from the SplineContainer

        List<Task> animationTasks = new List<Task>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (cardDisplays[i] == null) continue; // Skip if the object is null or destroyed

            CardWrapper cardWrapper = cardDisplays[i].GetComponent<CardWrapper>();
            cardWrapper.SetCard(hand[i]); // Set the card in the CardWrapper component
            SetCardDisplay(cardDisplays[i]); // Set the card display properties
            cardDisplays[i].transform.SetAsLastSibling(); // Move the card display to the last sibling in the hierarchy
            float p = firstCardPosition + i * spacing; // Calculate the normalized position on the spline
            Vector3 position = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculate the rotation based on the forward and up vectors

            // Add the position and rotation animations to the task list
            if (hand.Count >= 15)
            {
                cardDisplays[i].transform.localPosition = position; // Set the position of the card display
                cardDisplays[i].transform.localRotation = rotation; // Set the rotation of the card display
            }
            else
            {
                animationTasks.Add(cardDisplays[i].transform.DOLocalMove(position, 0.25f).SetUpdate(true).SetAutoKill(true).AsyncWaitForCompletion());
                animationTasks.Add(cardDisplays[i].transform.DOLocalRotateQuaternion(rotation, 0.25f).SetUpdate(true).SetAutoKill(true).AsyncWaitForCompletion());
            }
        }

        // Await all animations to complete
        await Task.WhenAll(animationTasks);
    }

    public void UpdateEnemyHand(CombatDetails enemy)
    {
        float spacing = 1f / enemy.deck.Count; // Spacing between cards
        float firstCardPosition = 0.5f - (enemy.deck.Count - 1) * spacing / 2; // Calculate the first card position
        UnityEngine.Splines.Spline spline = enemySpline.Spline; // Get the spline from the SplineContainer
        for (int i = 0; i < enemy.deck.Count; i++)
        {
            float p = firstCardPosition + i * spacing; // Calculate the normalized position on the spline
            Vector3 position = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculate the rotation based on the forward and up vectors
            enemyCardDisplays[i].transform.DOLocalMove(position, 0.25f).SetUpdate(true).SetAutoKill(true); // Set the position of the card display
            enemyCardDisplays[i].transform.DOLocalRotateQuaternion(rotation, 0.25f).SetUpdate(true).SetAutoKill(true); // Set the rotation of the card display
        }
    }

    public async Task UpdateEnemyHandAsync(CombatDetails enemy)
    {
        float spacing = 1f / enemy.deck.Count; // Spacing between cards
        float firstCardPosition = 0.5f - (enemy.deck.Count - 1) * spacing / 2; // Calculate the first card position
        UnityEngine.Splines.Spline spline = enemySpline.Spline; // Get the spline from the SplineContainer

        List<Task> animationTasks = new List<Task>();

        for (int i = 0; i < enemy.deck.Count; i++)
        {
            float p = firstCardPosition + i * spacing; // Calculate the normalized position on the spline
            Vector3 position = spline.EvaluatePosition(p); // Get the position on the spline
            Vector3 forward = spline.EvaluateTangent(p); // Get the forward direction on the spline
            Vector3 up = spline.EvaluateUpVector(p); // Get the up direction on the spline
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized); // Calculate the rotation based on the forward and up vectors

            // Add the position and rotation animations to the task list
            animationTasks.Add(enemyCardDisplays[i].transform.DOLocalMove(position, 0.25f).SetUpdate(true).SetAutoKill(true).AsyncWaitForCompletion());
            animationTasks.Add(enemyCardDisplays[i].transform.DOLocalRotateQuaternion(rotation, 0.25f).SetUpdate(true).SetAutoKill(true).AsyncWaitForCompletion());
        }

        // Await all animations to complete
        await Task.WhenAll(animationTasks);
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

        if (card.count > 1)
        {
            GameObject countObject = cardObject.transform.Find("Count").gameObject;
            TextMeshProUGUI count = countObject.GetComponent<TextMeshProUGUI>();
            count.text = "x" + card.count.ToString();
            countObject.SetActive(true);
        }
        else
        {
            GameObject countObject = cardObject.transform.Find("Count").gameObject;
            countObject.SetActive(false);
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