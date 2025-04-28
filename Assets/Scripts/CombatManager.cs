using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

public class CombatManager : MonoBehaviour
{
    public int level;
    public bool isPlayerTurn = true;
    public bool stageSetup = false;
    public EntityData[] entityDataArray;
    public SpriteManager sm;
    public PileController pileController;
    public CombatDetails playerDetails, enemyDetails;
    public RectTransform playerBar, enemyBar, energyBar;
    public GameObject EnemyStatusContainer, PlayerStatusContainer;
    public Button endTurnButton;
    public GameObject statusEffectPrefab;
    InputAction menuAction;
    public float playerWidth = 322, enemyWidth = 365f; // Default value for maxWidth


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
        menuAction = InputSystem.actions.FindAction("Menu");
        menuAction.performed += OnMenuAction;
    }

    private void OnDestroy()
    {
        menuAction.performed -= OnMenuAction;
    }

    private void OnMenuAction(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0f)
        {
            Menues.PauseGame();
        }
        else
        {
            Menues.ResumeGame();
        }
    }

    void Update()
    {
        // Update Display for Health and Energy Bars
        UpdateHealthBar(playerBar, playerDetails, playerWidth);
        UpdateHealthBar(enemyBar, enemyDetails, enemyWidth);
        UpdateEnergyBar(energyBar, playerDetails);
        SetStatusEffects(playerDetails, PlayerStatusContainer);
        SetStatusEffects(enemyDetails, EnemyStatusContainer);

        if (isPlayerTurn && endTurnButton.interactable == false)
        {
            endTurnButton.interactable = true;
        }
        else if (!isPlayerTurn && endTurnButton.interactable == true)
        {
            endTurnButton.interactable = false;
        }

        if (!stageSetup)
        {
            stageSetup = true;
            _ = TurnManager.CombatStart(this);
        }
        if (playerDetails.health <= 0)
        {
            TurnManager.Defeat(this);
        }
        if (enemyDetails.health <= 0)
        {
            TurnManager.CombatEnd(this);
        }
    }

    public async Task EndTurn()
    {
        await TurnManager.EndTurn(this);
    }

    public void EndTurnWrapper()
    {
        // Call the async method without awaiting it
        _ = EndTurn();
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

    public void SaveLevel()
    {
        Debug.Log("Saving level " + level);

        SerializeEntities();

        SaveData saveData = new SaveData
        {
            level = this.level,
            entityDataArray = this.entityDataArray,
            deck = playerDetails.deck.Where(card => card != null).ToList(),
            gear = playerDetails.gear.ToArray()
        };

        // Enable TypeNameHandling to include type information
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new CardConverter() },
            NullValueHandling = NullValueHandling.Include // Include null values
        });

        string path = Application.dataPath + "/levelData.json";

        File.WriteAllText(path, json);

        Debug.Log("Level data saved to " + path);
    }

    public void SerializeEntities()
    {
        for (int i = 0; i < entityDataArray.Length; i++)
        {
            if (entityDataArray[i].entityType == EntityType.Player)
            {
                entityDataArray[i].health = playerDetails.health;
                entityDataArray[i].maxHealth = playerDetails.healthMax;
                entityDataArray[i].maxEnergy = playerDetails.energyMax;
            }
            else if (entityDataArray[i].isAttacker)
            {
                entityDataArray[i] = null; // Remove the enemy data from the array if it's not needed
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
    public void UpdateHealthBar(RectTransform mask, CombatDetails entityDetails, float maxWidth)
    {
        float healthPercent = 1f - (float)entityDetails.health / entityDetails.healthMax;

        // Calculate the new width based on health percentage
        float newWidth = healthPercent * maxWidth;

        // Animate the mask's width to expand from the right
        mask.DOSizeDelta(new Vector2(newWidth, mask.sizeDelta.y), 0.5f);
    }

    public void UpdateEnergyBar(RectTransform mask, CombatDetails entityDetails)
    {
        float energyPercent = 1f - (float)entityDetails.energy / entityDetails.energyMax;

        // Calculate the new width based on energy percentage
        float newWidth = energyPercent * 320f; // Adjusted to a smaller width for energy bar

        // Animate the mask's width to expand from the right
        mask.DOSizeDelta(new Vector2(newWidth, mask.sizeDelta.y), 0.5f);
    }

    public void SetStatusEffects(CombatDetails entityDetails, GameObject statusContainer)
    {
        // Clear existing status effects
        foreach (Transform child in statusContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Set the status effects for the entity
        foreach (var effect in entityDetails.statusEffects)
        {
            GameObject statusEffect = Instantiate(statusEffectPrefab, statusContainer.transform);
            Image statusImage = statusEffect.transform.Find("Icon").GetComponent<Image>();
            statusImage.sprite = sm.GetSprite(effect.statusName);
            TextMeshProUGUI statusText = statusEffect.transform.Find("Turns").GetComponent<TextMeshProUGUI>();
            statusText.text = effect.turnsLeft.ToString();
        }
    }
}
