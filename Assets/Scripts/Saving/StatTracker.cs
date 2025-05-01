using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EntityData
{
    public EntityType entityType;
    public float xPos, yPos;
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
    public float xPos, yPos;
    public List<Card> deck = new List<Card>();

    [JsonConstructor]
    public BonePile(int level, float xPos, float yPos, List<Card> deck)
    {
        this.level = level;
        this.xPos = xPos;
        this.yPos = yPos;
        this.deck = deck;
    }

    public BonePile(int level, float xPos, float yPos, List<Card> deck, Card[] gear)
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
    public bool tutorialCompleted = false;
    public bool necromancerDefeated = false;
    public List<Card> CardsCollected;
    public int CardsPlayed;
    public int EnemiesKilled;
    public int ChestsOpened;
    public int FloorsCleared;
    public int BonePilesCollected;
    public int RocksCollected;
    public int RunsCompleted;
}

[System.Serializable]
public class StatTracker : MonoBehaviour
{
    public static StatTracker Instance { get; private set; }
    public Stats stats = new Stats();
    private bool isStatDisplaySet = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign the singleton instance
            DontDestroyOnLoad(this.gameObject); // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        LoadStats();
    }

    public void Update()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "GameOverScene" || currentScene == "VictoryScene")
        {
            if (!isStatDisplaySet)
            {
                SetStatDisplay(); // Call the method to set the stat display
                isStatDisplaySet = true; // Set the flag to true to prevent multiple calls
            }
        }
    }

    public void SetStatDisplay()
    {
        GameObject.Find("Collected (1)").GetComponent<TextMeshProUGUI>().text = stats.CardsCollected.Count.ToString();
        GameObject.Find("Played (1)").GetComponent<TextMeshProUGUI>().text = stats.CardsPlayed.ToString();
        GameObject.Find("Killed (1)").GetComponent<TextMeshProUGUI>().text = stats.EnemiesKilled.ToString();
        GameObject.Find("Chests (1)").GetComponent<TextMeshProUGUI>().text = stats.ChestsOpened.ToString();
        GameObject.Find("Floors (1)").GetComponent<TextMeshProUGUI>().text = stats.FloorsCleared.ToString();
        GameObject.Find("Scavenged (1)").GetComponent<TextMeshProUGUI>().text = stats.BonePilesCollected.ToString();
        GameObject.Find("Rocks (1)").GetComponent<TextMeshProUGUI>().text = stats.RocksCollected.ToString();
        GameObject.Find("Runs (1)").GetComponent<TextMeshProUGUI>().text = stats.RunsCompleted.ToString();
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
    public static void IncrementRunsCompleted()
    {
        Instance.stats.RunsCompleted++;
        SaveStats();
    }
    public static void SetTutorialCompleted(bool completed)
    {
        Instance.stats.tutorialCompleted = completed;
        SaveStats();
    }

    public static bool IsTutorialCompleted()
    {
        return Instance.stats.tutorialCompleted;
    }

    public static void SetNecromancerDefeated(bool defeated)
    {
        Instance.stats.necromancerDefeated = defeated;
        SaveStats();
    }

    public static bool IsNecromancerDefeated()
    {
        return Instance.stats.necromancerDefeated;
    }
}