using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    int level;
    public bool isPlayerTurn = true;
    public bool stageSetup = false;
    public EntityData[] entityDataArray;
    public SpriteManager sm;
    public PileController pileController;
    public UIDocument UIDoc;
    VisualElement energyContainer, energyBorders;
    public Label playerHealthLabel;
    public Label enemyHealthLabel;
    public VisualElement playerBarMask, enemyBarMask;

    public CombatDetails playerDetails, enemyDetails;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
        SetStageDetails();
    }

    void Update()
    {
        // Update Display for Health and Energy Bars
        SetDisplay();

        if (!stageSetup)
        {
            SetStageDetails();
            stageSetup = true;
            PlayerLogic.CombatStart(this);
        }
        if (playerDetails.health <= 0)
        {
            PlayerLogic.Defeat(this);
        }
        if (enemyDetails.health <= 0)
        {
            PlayerLogic.CombatEnd(this);
        }
    }

    public void EndTurn()
    {
        TurnManager.EndTurn(this);
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
                // Enable TypeNameHandling to deserialize with type information
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                this.level = saveData.level;
                this.entityDataArray = saveData.entityDataArray;

                DeserializeEntities(entityDataArray, saveData.deck, saveData.gear);

                Debug.Log("Level data loaded from " + path);
            }
            catch (JsonReaderException e)
            {
                Debug.LogError("Failed to parse JSON: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
        }
    }

    public void ReturnToLevel()
    {
        Debug.Log("Saving level " + level);

        SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = this.level,
            entityDataArray = this.entityDataArray,
            deck = playerDetails.deck.Where(card => card != null).ToList(),
            gear = playerDetails.gear.Where(card => card != null).ToArray()
        };

        // Enable TypeNameHandling to include type information
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() },
            NullValueHandling = NullValueHandling.Include // Include null values
        });

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
            }
        }
    }

    public void DeserializeEntities(EntityData[] entityDataArray, List<Card> cards, Card[] gear)
    {
        foreach (EntityData data in entityDataArray)
        {
            if (data.entityType == EntityType.Player)
            {
                playerDetails = new CombatDetails(
                    data.entityType,
                    data.health,
                    data.maxHealth,
                    data.energy,
                    data.maxEnergy
                );
                
                playerDetails.deck = cards; // The deck will now contain the correct derived types

                // Ensure the gear array is properly initialized
                playerDetails.gear = new Card[gear.Length];
                for (int i = 0; i < gear.Length; i++)
                {
                    playerDetails.gear[i] = gear[i]; // Copy gear, including null values
                }
            }
            else if (data.isAttacker)
            {
                enemyDetails = new CombatDetails(data.entityType, data.health, data.maxHealth, data.energy, data.maxEnergy);
            }
        }
    }

    public void SetStageDetails()
    {
        playerBarMask = UIDoc.rootVisualElement.Q<VisualElement>("PlayerBarMask");
        enemyBarMask = UIDoc.rootVisualElement.Q<VisualElement>("EnemyBarMask");
        playerHealthLabel = UIDoc.rootVisualElement.Q<Label>("PlayerHealth");
        enemyHealthLabel = UIDoc.rootVisualElement.Q<Label>("EnemyHealth");

        CreateEnergyDisplay();
    }

    public void CreateEnergyDisplay()
    {
        energyBorders = UIDoc.rootVisualElement.Q<VisualElement>("EnergyBorders");
        energyContainer = UIDoc.rootVisualElement.Q<VisualElement>("EnergyContainer");

        energyContainer.style.flexDirection = FlexDirection.Row;
        energyContainer.style.alignItems = Align.Center;
        energyBorders.style.flexDirection = FlexDirection.Row;
        energyBorders.style.alignItems = Align.Center;

        energyBorders.Clear();


        for (int i = 0; i < playerDetails.energyMax; i++)
        {
            VisualElement energyBorder = new VisualElement();

            energyBorder.style.width = 32;
            energyBorder.style.height = 32;

            energyBorder.style.backgroundImage = new StyleBackground(sm.GetSprite("Energy Border"));

            energyBorder.style.marginLeft = 10;

            energyBorders.Add(energyBorder);
        }
    }

    public void SetDisplay()
    {
        energyContainer.Clear();

        float pHealthPercent = (float)playerDetails.health / playerDetails.healthMax;
        float eHealthPercent = (float)enemyDetails.health / enemyDetails.healthMax;
        playerBarMask.style.width = Length.Percent(pHealthPercent * 100);
        enemyBarMask.style.width = Length.Percent(eHealthPercent * 100);

        playerHealthLabel.text = $"{playerDetails.health}/{playerDetails.healthMax}";
        enemyHealthLabel.text = $"{enemyDetails.health}/{enemyDetails.healthMax}";


        for (int i = 0; i < playerDetails.energy; i++)
        {
            VisualElement energyFill = new VisualElement();

            energyFill.style.width = 32;
            energyFill.style.height = 32;

            energyFill.style.backgroundImage = new StyleBackground(sm.GetSprite("Energy Fill"));

            energyFill.style.marginLeft = 10;

            
            energyContainer.Add(energyFill);
        }
    }
}
