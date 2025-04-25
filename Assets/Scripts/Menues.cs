using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement; // For runtime scene management

public class Menues : MonoBehaviour
{
    public static void DeleteSaveFile()
    {
        string path = Application.dataPath + "/levelData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at " + path);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + path);
        }
    }

    public static bool SaveGame()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "ExplorationScene":
                GameObject.Find("Level Manager").GetComponent<LevelManager>().SaveLevel(); // Hide the pause menu
                return true; // Saved the game
            case "CombatScene":
                Debug.Log("Cant save in combat"); // Show the pause menu
                return false; // Failed to save the game
            default:
                Debug.Log("Cant save in this scene"); // Show the pause menu
                return false; // Failed to save the game
        }
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        GameObject.Find("PauseMenu").transform.GetChild(0).gameObject.SetActive(true); // Show the pause menu
        Debug.Log("Game paused");
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        GameObject.Find("PauseMenu").transform.GetChild(0).gameObject.SetActive(false); // Show the pause menu
        Debug.Log("Game resumed");
    }

    public static void SaveAndQuit()
    {
        SaveGame(); // Save the game
        QuitGame(); // Quit the game
    }

    public static void MainMenu()
    {
        Time.timeScale = 1f; // Resume the game
        SaveGame(); // Save the game
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    public static void Forfeit()
    {
        Time.timeScale = 1f; // Resume the game
        DeleteSaveFile(); // Delete the save file
        SceneManager.LoadScene("MainMenu"); // Load the exploration scene
    }

    public static void NewGame()
    {
        Time.timeScale = 1f; // Resume the game
        DeleteSaveFile(); // Delete the save file
        SceneManager.LoadScene("ExplorationScene"); // Load the exploration scene
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene("ExplorationScene"); // Load the exploration scene
    }

    public static void Tutorial()
    {
        Time.timeScale = 1f; // Resume the game
        DeleteSaveFile(); // Delete the save file
        SceneManager.LoadScene("ExplorationScene"); // Load the tutorial scene
    }

    public static void VictoryScreen()
    {
        Time.timeScale = 1f; // Resume the game
        DeleteSaveFile(); // Delete the save file
        SceneManager.LoadScene("VictoryScene"); // Load the victory scene
    }

    public static void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Game quit"); // Log the quit action
    }
}
