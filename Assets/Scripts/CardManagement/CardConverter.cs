using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class CardConverter : JsonConverter
{
    private static readonly Dictionary<string, Func<Card>> cardFactory = new Dictionary<string, Func<Card>>
    {
        //Consumables
        { "Healing Potion", () => new HealthPotion() },
        { "Greater Healing Potion", () => new GreatHealthPotion() },
        { "Divine Healing Potion", () => new SuperHealthPotion() },
        { "Potion of Haste", () => new HastePotion() },
        { "Stone", () => new Stone() },
        { "Stamina Draught", () => new StaminaDraught() },
        { "Vitality Draught", () => new VitalityDraught() },
        //Weapons
        { "Iron Sword", () => new IronSword() },
        { "Adamantite Sword", () => new AdamantiteSword() },
        { "Flaming Sword", () => new FlamingSword() },
        { "Obsidian Blade", () => new ObsidianBlade() },
        { "Iron Axe", () => new IronAxe() },
        { "Orichalcum Axe", () => new OrichalcumAxe() },
        { "Crimson Cutter", () => new CrimsonCutter() },
        //Armor
        { "Iron Helmet", () => new IronHelmet() },
        { "Iron Chestpiece", () => new IronChestpiece() },
        { "Iron Boots", () => new IronBoots() },
        { "Mythril Helmet", () => new MythrilHelmet() },
        { "Mythril Chestpiece", () => new MythrilChestpiece() },
        { "Mythril Boots", () => new MythrilBoots() },
        { "Ram's Horn Helm", () => new RamHelm() },
        { "Invigorating Chestplate", () => new InvigoratingChestplate() },
        { "Frogz kin Boots", () => new FrogskinBoots() },
        { "Bone Lord's Helmet", () => new BoneLordHelmet() },
        { "Bone Lord's Breastplate", () => new BoneLordBreastplate() },
        { "Bone Lord's Greaves", () => new BoneLordGreaves() },
        { "Archmage's Gall", () => new ArchmagesGall() },
        { "Dragon King's Curiass", () => new DragonKingsCuriass() },
        { "Falcon Sabatons", () => new FalconSabatons() },
        //Shields
        { "Shield", () => new Shield() },
        { "Reinforced Shield", () => new IronShield() },
        { "Thorned Shield", () => new ThornedShield() },
        { "Divine Shield", () => new DivineShield() },
        //Bows
        { "Longbow", () => new Longbow() },
        { "Crossbow", () => new Crossbow() },
        { "Arcane Charged Bow", () => new ChargeBow() },
        { "Stratus", () => new Stratus() },
        // Arrows
        { "Wooden Arrow", () => new WoodArrow() },
        { "Steel Tipped Arrow", () => new SteelArrow() },
        { "Poison Arrow", () => new PoisonArrow() },
        { "Lightning Arrow", () => new LightningArrow() },
        // Accessories
        { "Philosopher's Stone", () => new PhilosophersStone() },
        { "Ring of the Forge", () => new RingOfTheForge() },
        { "Quiver Talisman", () => new QuiverTalisman() },
        { "Energy Talisman", () => new EnergyTalisman() },
        { "Scorpion Medalion", () => new ScorpionMedalion() },
        { "Dragon King's Scale", () => new DragonKingsScale() },
        { "Fangs of the Vampire", () => new FangsOfTheVampire() },
        { "Werewolf's Glare", () => new WerewolfsGlare() },
        // Boss Cards
        { "Necrotic Touch", () => new NecroticTouch() },
        { "Magic Shield", () => new MagicShield() },
        { "Fireball", () => new Fireball() },
        { "Cleanse", () => new Cleanse() },
        { "Conjure Arcane Barrage", () => new ConjureArcaneBarrage() },
        { "Arcane Missile", () => new ArcaneMissile() },
        { "Curse", () => new Curse() },
        { "Necromancer's Crown", () => new NecromancersCrown() },
        { "Necromancer's Cloak", () => new NecromancersCloak() },
        { "Necromancer's Slippers", () => new NecromancersSlippers() },
        { "Ring of the Dead", () => new RingOfTheDead() },
        // Enemy Cards
        { "Slime Burst", () => new SlimeBurst() },
        { "Goblin Dagger", () => new GoblinDagger() },
        { "Skeleton Arrow", () => new SkeletonArrow() },
        { "Skeleton Poison Arrow", () => new SkeletonPoisonArrow() },
        { "Rib Bone", () => new RibBone() },
        { "Vampiric Bite", () => new VampiricBite() },
        { "Vampire's Robe", () => new VampiresRobe() },
        { "Vampire's Boots", () => new VampiresBoots() },
        { "Darkness", () => new Darkness() },
        { "Howl", () => new Howl() },
        { "Clawed Slash", () => new ClawedSlash() },
        { "Werewolf's Mane", () => new WerewolfsMane() },
        { "Werewolf's Hide", () => new WerewolfsHide() },
        { "Werewolf's Pursuit", () => new WerewolfsPursuit() }
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
