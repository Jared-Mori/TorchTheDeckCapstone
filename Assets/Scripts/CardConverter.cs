using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class CardConverter : JsonConverter
{
    private static readonly Dictionary<string, Func<Card>> cardFactory = new Dictionary<string, Func<Card>>
    {
        { "Health Potion", () => new HealthPotion() },
        { "Stone", () => new Stone() },
        { "Iron Helmet", () => new IronHelmet() },
        { "Iron Chestpiece", () => new IronChestpiece() },
        { "Iron Boots", () => new IronBoots() },
        { "Iron Shield", () => new IronShield() },
        { "Iron Sword", () => new IronSword() },
        { "Shield", () => new Shield() },
        { "Reinforced Shield", () => new IronShield() },
        { "Longbow", () => new Longbow() },
        { "Wooden Arrow", () => new WoodArrow() },
        // Add other cards here...
    };

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Debug.Log("WriteJson called");
        Card card = (Card)value;
        JObject obj = new JObject
        {
            { "cardName", card.cardName },
            { "description", card.description },
            { "uses", card.uses },
            { "rarity", card.rarity },
            { "isStackable", card.isStackable },
            { "count", card.count },
            { "itemType", card.itemType.ToString() } // Explicitly convert ItemType to string
        };

        Debug.Log($"Serializing card: {card.cardName}");
        obj.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        Debug.Log("ReadJson called");

        // Check if the current token is null
        if (reader.TokenType == JsonToken.Null)
        {
            Debug.Log("Encountered null value in JSON for a Card.");
            return null; // Return null explicitly for null tokens
        }

        JObject obj = JObject.Load(reader);
        string cardName = obj["cardName"]?.ToString();

        if (string.IsNullOrEmpty(cardName))
        {
            Debug.LogError("Missing or invalid cardName in JSON.");
            return null;
        }

        Debug.Log($"Deserializing card: {cardName}");
        Card card = CreateCardFromCardName(cardName);
        if (card == null)
        {
            Debug.LogError($"Failed to create card for cardName: {cardName}");
            return null;
        }

        // Populate common properties
        card.cardName = cardName;
        card.description = obj["description"]?.ToString();
        card.uses = obj["uses"]?.ToObject<int>() ?? 0;
        card.rarity = obj["rarity"]?.ToObject<int>() ?? 0;
        card.isStackable = obj["isStackable"]?.ToObject<bool>() ?? false;
        card.count = obj["count"]?.ToObject<int>() ?? 1;

        // Explicitly convert itemType from string to enum
        if (obj.TryGetValue("itemType", out JToken itemTypeToken))
        {
            string itemTypeString = itemTypeToken.ToString();
            if (Enum.TryParse(itemTypeString, out ItemType itemType))
            {
                card.itemType = itemType;
            }
            else
            {
                Debug.LogWarning($"Unknown ItemType: {itemTypeString}");
            }
        }

        return card;
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(Card).IsAssignableFrom(objectType);
    }

    private Card CreateCardFromCardName(string cardName)
    {
        if (cardFactory.TryGetValue(cardName, out var createCard))
        {
            return createCard();
        }

        Debug.LogWarning($"Unknown cardName: {cardName}");
        return null;
    }
}
