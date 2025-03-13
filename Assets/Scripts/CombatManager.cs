using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    protected Player player;
    protected Entity attacker;
    public bool playerTurn = true;
    LevelManager lm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        string json = File.ReadAllText(path);
        lm = JsonUtility.FromJson<LevelManager>(json);
        player = lm.playerInstance;
    }

    public void ReturnToLevel()
    {
        // Return to the level scene
        lm.playerInstance = player;
        string json = JsonConvert.SerializeObject(lm, Formatting.Indented);
        string path = Application.persistentDataPath + "/levelData.json";
        File.WriteAllText(path, json);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
    }
}
