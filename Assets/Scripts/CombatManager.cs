using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Mono.Cecil.Cil;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    }

    void Update()
    {
        if (!stageSetup)
        {
            SetStageDetails();
            stageSetup = true;
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
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                this.level = saveData.level;
                this.entityDataArray = saveData.entityDataArray;

                DeserializeEntities(entityDataArray);

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
                playerDetails = new CombatDetails(data.entityType, data.health, data.maxHealth, data.energy, data.maxEnergy, data.deck, data.gear);
            }
            else if (data.isAttacker)
            {
                enemyDetails = new CombatDetails(data.entityType, data.health, data.maxHealth, data.energy, data.maxEnergy, data.deck, data.gear);
            }
        }
    }

    public void SetStageDetails()
    {
        playerBarMask = UIDoc.rootVisualElement.Q<VisualElement>("PlayerBarMask");
        enemyBarMask = UIDoc.rootVisualElement.Q<VisualElement>("EnemyBarMask");
        playerHealthLabel = UIDoc.rootVisualElement.Q<Label>("PlayerHealth");
        enemyHealthLabel = UIDoc.rootVisualElement.Q<Label>("EnemyHealth");

        float pHealthPercent = (float)playerDetails.health / playerDetails.healthMax;
        float eHealthPercent = (float)enemyDetails.health / enemyDetails.healthMax;
        playerBarMask.style.width = Length.Percent(pHealthPercent * 100);
        enemyBarMask.style.width = Length.Percent(eHealthPercent * 100);

        playerHealthLabel.text = $"{playerDetails.health}/{playerDetails.healthMax}";
        enemyHealthLabel.text = $"{enemyDetails.health}/{enemyDetails.healthMax}";

        CreateEnergyDisplay();
        SetEnergyDisplay();
    }

    public void CreateEnergyDisplay()
    {
        energyBorders = UIDoc.rootVisualElement.Q<VisualElement>("EnergyBorders");
        energyContainer = UIDoc.rootVisualElement.Q<VisualElement>("EnergyContainer");

        energyContainer.style.flexDirection = FlexDirection.Row;
        energyContainer.style.alignItems = Align.Center;
        energyBorders.style.flexDirection = FlexDirection.Row;
        energyBorders.style.alignItems = Align.Center;

        energyContainer.Clear();
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

    public void SetEnergyDisplay()
    {
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
