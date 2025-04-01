using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;
using UnityEngine.SceneManagement;

public struct combatDetails {
    // basic stats
    public int health;
    public int healthMax;
    public int energy;
    public int energyMax;

    // deck info
    public List<Card> deck;
    public Equipment[] gear;

    // Combat states
    public bool isShielded;

    // Other Data


    public combatDetails(int health, int healthMax, int energy, int energyMax, List<Card> deck, Equipment[] gear) {
        this.health = health;
        this.healthMax = healthMax;
        this.energy = energy;
        this.energyMax = energyMax;
        this.deck = deck;
        this.gear = gear;
        this.isShielded = false;
    }
}

public class CombatManager : MonoBehaviour
{
    int level;
    public bool playerTurn = true;
    public EntityData[] entityDataArray;

    public combatDetails playerDetails, enemyDetails;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        Debug.Log("Loading level " + level);

        string path = Application.dataPath + "/levelData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            try
            {
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                this.level = saveData.level;
                this.entityDataArray = saveData.entityDataArray;

                DeserializeEntities(entityDataArray);

                Debug.Log("Level data loaded from " + path);
            }
            catch (JsonReaderException e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
                InitializeDefaultLevel();
            }
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
            InitializeDefaultLevel();
        }
    }

    public void ReturnToLevel()
    {
        Debug.Log("Saving level " + level);

        SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = this.level,
            entityDataArray = this.entityDataArray
        };

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        string path = Application.persistentDataPath + "/levelData.json";

        File.WriteAllText(path, json);

        Debug.Log("Level data saved to " + path);

        SceneManager.LoadScene("Exploration");
    }

    public void SerializeEntities()
    {
        for (int i = 0; i < entityDataArray.Length; i++)
        {
            if (entityDataArray[i].entityType == EntityType.Player)
            {
                entityDataArray[i].health = playerDetails.health;
                entityDataArray[i].gear = playerDetails.gear;
                entityDataArray[i].deck = playerDetails.deck;
            }
            else if (entityDataArray[i].isAttacker)
            {
                entityDataArray[i].health = enemyDetails.health;
                entityDataArray[i].gear = enemyDetails.gear;
                entityDataArray[i].deck = enemyDetails.deck;
            }
        }
    }

    public void DeserializeEntities(EntityData[] entityDataArray)
    {
        foreach (EntityData data in entityDataArray)
        {
            if (data.entityType == EntityType.Player)
            {
                playerDetails = new combatDetails(data.health, data.maxHealth, data.energy, data.maxEnergy, data.deck, data.gear);
            }
            else if (data.isAttacker)
            {
                enemyDetails = new combatDetails(data.health, data.maxHealth, data.energy, data.maxEnergy, data.deck, data.gear);
            }
        }
    }

    private void InitializeDefaultLevel()
    {
        // Initialize default level data here
    }
}
