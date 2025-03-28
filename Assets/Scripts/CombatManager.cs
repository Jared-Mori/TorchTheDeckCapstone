using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;

public class CombatManager : MonoBehaviour
{
    protected Player player;
    protected Entity attacker;
    int level;
    public bool playerTurn = true;
    public EntityData[] entityDataArray;
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
                EntityData[] entityDataArray = saveData.entityDataArray;

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
    }

    public void SerializeEntities()
    {
        for (int i = 0; i < entityDataArray.Length; i++)
        {
            if (entityDataArray[i].entityType == EntityType.Player)
            {
                entityDataArray[i].entityType = player.entityType;
                entityDataArray[i].xPos = player.gridPosition.x;
                entityDataArray[i].yPos = player.gridPosition.y;
                entityDataArray[i].facing = player.facing;
                entityDataArray[i].health = player.health;
                entityDataArray[i].entityName = player.entityName;
                entityDataArray[i].gear = player.gear;
                entityDataArray[i].deck = player.deck;
            }
            else if (entityDataArray[i].isAttacker)
            {
                entityDataArray[i].entityType = attacker.entityType;
                entityDataArray[i].xPos = attacker.gridPosition.x;
                entityDataArray[i].yPos = attacker.gridPosition.y;
                entityDataArray[i].facing = attacker.facing;
                entityDataArray[i].health = attacker.health;
                entityDataArray[i].entityName = attacker.entityName;
                entityDataArray[i].isAttacker = attacker.isAttacker;
                entityDataArray[i].gear = attacker.gear;
                entityDataArray[i].deck = attacker.deck;
            }
        }
    }

    public void DeserializeEntities(EntityData[] entityDataArray)
    {
        foreach (EntityData data in entityDataArray)
        {
            if (data.entityType == EntityType.Player)
            {
                player.entityType = data.entityType;
                player.entityName = data.entityName;
                player.facing = data.facing;
                player.health = data.health;
                player.gear = data.gear;
                player.deck = data.deck;
                player.isAttacker = data.isAttacker;

                player.SetPosition(new Vector3Int(data.xPos, data.yPos, 0));
            }
            else if (data.isAttacker)
            {
                attacker.entityType = data.entityType;
                attacker.entityName = data.entityName;
                attacker.facing = data.facing;
                attacker.health = data.health;
                attacker.gear = data.gear;
                attacker.deck = data.deck;
                attacker.isAttacker = data.isAttacker;

                attacker.SetPosition(new Vector3Int(data.xPos, data.yPos, 0));
            }
        }
    }

    private void InitializeDefaultLevel()
    {
        // Initialize default level data here
    }
}
