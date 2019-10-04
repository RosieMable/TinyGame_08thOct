using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public enum Difficulty { Normal, Masochist }
    public Difficulty difficultySetting { get; set; }

    private void Awake()
    {
        if (instance == null) // If instance does not exist...
        {
            instance = this; // Set instance to this
        }
        else if (instance != this) // Otherwise if another instance exists already...
        {
            Destroy(this); // Destroy this class, ensures that only 1 GameManager can exist in scene
        }
    }

    public void LevelComplete()
    {
        // Scene transition/overlay
    }

    public void GameOver()
    {
        // Scene transition/overlay

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Can be removed later, used primarily for testing right now
    }
}
