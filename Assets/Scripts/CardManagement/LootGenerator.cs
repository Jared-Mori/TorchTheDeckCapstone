using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LootGenerator
{
    static Dictionary<Card, int> CommonLoot = new Dictionary<Card, int>()
    {
        { new HealthPotion(), 3 },
        { new EnergyTalisman(), 1 },
        { new IronHelmet(), 1 },
        { new IronChestpiece(), 1 },
        { new IronBoots(), 1 },
        { new Shield(), 1 },
        { new WoodArrow(), 10 },
        { new Longbow(), 1 },
        { new IronSword(), 1 },
        { new IronAxe(), 1 }
    };

    static Dictionary<Card, int> UncommonLoot = new Dictionary<Card, int>()
    {
        { new GreatHealthPotion(), 3 },
        { new QuiverTalisman(), 1 },
        { new MythrilHelmet(), 1 },
        { new MythrilChestpiece(), 1 },
        { new MythrilBoots(), 1 },
        { new IronShield(), 1 },
        { new SteelArrow(), 15 },
        { new Crossbow(), 1 },
        { new AdamantiteSword(), 1 },
        { new OrichalcumAxe(), 1 }
    };

    static Dictionary<Card, int> RareLoot = new Dictionary<Card, int>()
    {
        { new HastePotion(), 2 },
        { new RingOfTheForge(), 1 },
        { new ScorpionMedalion(), 1 },
        { new RamHelm(), 1 },
        { new InvigoratingChestplate(), 1 },
        { new FrogskinBoots(), 1 },
        { new ThornedShield(), 1 },
        { new PoisonArrow(), 5 },
        { new ChargeBow(), 1 },
        { new FlamingSword(), 1 },
        { new CrimsonCutter(), 1 }
    };

    static Dictionary<Card, int> LegendaryLoot = new Dictionary<Card, int>()
    {
        { new SuperHealthPotion(), 3 },
        { new DragonKingsScale(), 1 },
        { new PhilosophersStone(), 1 },
        { new FangsOfTheVampire(), 1 },
        { new WerewolfsGlare(), 1 },
        { new BoneLordHelmet(), 1 },
        { new BoneLordBreastplate(), 1 },
        { new BoneLordGreaves(), 1 },
        { new ArchmagesGall(), 1 },
        { new DragonKingsCuriass(), 1 },
        { new FalconSabatons(), 1 },
        { new DivineShield(), 1 },
        { new LightningArrow(), 5 },
        { new Stratus(), 1 },
        { new ObsidianBlade(), 1 }
    };


    public static List<Card> GenerateLoot(int count, int rarity)
    {
        List<Card> loot = new List<Card>();
        Dictionary<Card, int> lootTable = CommonLoot;

        switch (rarity)
        {
            case Rarity.Common:
                lootTable = CommonLoot;
                break;
            case Rarity.Uncommon:
                lootTable = UncommonLoot;
                break;
            case Rarity.Rare:
                lootTable = RareLoot;
                break;
            case Rarity.Legendary:
                lootTable = LegendaryLoot;
                break;
            default:
                Debug.LogError("Invalid rarity level: " + rarity);
                return loot;
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, lootTable.Count);
            Card card = lootTable.Keys.ElementAt(randomIndex);
            card.count = lootTable[card];
            loot.Add(card);
        }

        return loot;
    }

    public static List<Card> GenerateDrops(int Common, int Uncommon, int Rare, int Legendary)
    {
        List<Card> loot = new List<Card>();
        loot.AddRange(GenerateLoot(Common, Rarity.Common));
        loot.AddRange(GenerateLoot(Uncommon, Rarity.Uncommon));
        loot.AddRange(GenerateLoot(Rare, Rarity.Rare));
        loot.AddRange(GenerateLoot(Legendary, Rarity.Legendary));

        return loot;
    }
}
