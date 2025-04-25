using UnityEngine;
using System.Collections.Generic;
using System.IO; 

[System.Serializable]
public class EntityData
{
    public EntityType entityType;
    public int xPos, yPos;
    public Direction facing;
    public bool isAttacker;
    public bool isOpenedChest = false;
    public int health, maxHealth;
    public int energy, maxEnergy;
}

[System.Serializable]
public class SaveData
{
    public int level;
    public EntityData[] entityDataArray;
    public List<Card> deck = new List<Card>();
    public Card[] gear = new Card[7]; // 0: Helmet, 1: Chestpiece, 2: Boots, 3: Shield, 4: Accessory, 5: Weapon, 6: Bow
}

[System.Serializable]
public class BonePile
{
    public int level;
    public int xPos, yPos;
    public List<Card> deck = new List<Card>();

    public BonePile(int level, int xPos, int yPos, List<Card> deck, Card[] gear)
    {
        this.level = level;
        this.xPos = xPos;
        this.yPos = yPos;
        this.deck = deck;

        foreach (Card card in gear)
        {
            if (card != null && card is not TempCard)
            {
                this.deck.Add(card);
            }
        }
    }
}

[System.Serializable]
public class Stats
{
    public List<Card> CardsCollected;
    public int CardsPlayed;
    public int EnemiesKilled;
    public int ChestsOpened;
    public int FloorsCleared;
    public int BonePilesCollected;
    public int RocksCollected;
}

[System.Serializable]
public class StatTracker : MonoBehaviour
{
    public static StatTracker Instance { get; private set; }
    public Stats stats = new Stats();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        LoadStats();
    }

    public static void SaveStats()
    {
        string json = JsonUtility.ToJson(Instance.stats);
        File.WriteAllText(Application.persistentDataPath + "/stats.json", json);
    }

    public void LoadStats()
    {
        string path = Application.persistentDataPath + "/stats.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            stats = JsonUtility.FromJson<Stats>(json);
        }
    }

    public static void CollectCard(Card card)
    {
        if (card != null && !Instance.stats.CardsCollected.Contains(card))
        {
            Instance.stats.CardsCollected.Add(card);
        }
        SaveStats();
    }
    public static void IncrementCardsPlayed()
    {
        Instance.stats.CardsPlayed++;
        SaveStats();
    }
    public static void IncrementEnemiesKilled()
    {
        Instance.stats.EnemiesKilled++;
        SaveStats();
    }
    public static void IncrementChestsOpened()
    {
        Instance.stats.ChestsOpened++;
        SaveStats();
    }
    public static void IncrementFloorsCleared()
    {
        Instance.stats.FloorsCleared++;
        SaveStats();
    }
    public static void IncrementBonePilesCollected()
    {
        Instance.stats.BonePilesCollected++;
        SaveStats();
    }
    public static void IncrementRocksCollected()
    {
        Instance.stats.RocksCollected++;
        SaveStats();
    }
}