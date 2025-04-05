using UnityEngine;

public class CardWrapper : MonoBehaviour
{
    public Card card;

    public void SetCard(Card newCard)
    {
        card = newCard;
        // Update the UI or other components to reflect the new card
    }
}
