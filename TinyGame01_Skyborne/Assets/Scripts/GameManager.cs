using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton
    public float Score { get; set; }
    public float ScoreMultiplier { get; set; }

    private TextMeshProUGUI scoreDisplay;
    private TextMeshProUGUI multiplierDisplay;

    public enum Difficulty { Normal, Masochist }
    public Difficulty difficultySetting { get; set; }

    public delegate void OnLevelComplete();
    public OnLevelComplete OnLevelCompletecallback;
    public delegate void OnGameOver();
    public OnGameOver OnGameOvercallback;

    private Coroutine multiplerCoroutine;

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

        ScoreMultiplier = 1;
        Score = 0;
        scoreDisplay = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        multiplierDisplay = GameObject.Find("Multiplier").GetComponent<TextMeshProUGUI>();
        scoreDisplay.text = "Score: " + Score;
        multiplierDisplay.text = "x" + ScoreMultiplier;
    }

    public void IncreaseScore()
    {
        if (multiplerCoroutine != null)
        {
            StopCoroutine(multiplerCoroutine);
            multiplerCoroutine = StartCoroutine(IncreaseScore(3));
        }
        else
        {
            multiplerCoroutine = StartCoroutine(IncreaseScore(3));
        }
    }

    private IEnumerator IncreaseScore(float timer)
    {
        Score += 25 * ScoreMultiplier;
        ScoreMultiplier++;

        switch (ScoreMultiplier)
        {
            case 1:
                multiplierDisplay.color = Color.white;
                break;
            case 2:
                multiplierDisplay.color = Color.gray;
                break;
            case 3:
                multiplierDisplay.color = Color.blue;
                break;
            case 4:
                multiplierDisplay.color = Color.red;
                break;
            case 5:
                multiplierDisplay.color = Color.yellow;
                break;
            case 6:
                multiplierDisplay.color = Color.black;
                break;
            case 7:
                multiplierDisplay.color = Color.green;
                break;
            case 8:
                multiplierDisplay.color = Color.cyan;
                break;
            case 9:
                multiplierDisplay.color = Color.magenta;
                break;
            default:
                multiplierDisplay.color = Color.magenta;
                break;
        }

        scoreDisplay.text = "Score: " + Score;
        multiplierDisplay.text = "x" + ScoreMultiplier;

        yield return new WaitForSeconds(timer);

        ScoreMultiplier = 1;
        multiplierDisplay.text = "x" + ScoreMultiplier;
        multiplierDisplay.color = Color.white;
        multiplerCoroutine = null;
    }

    public void LevelComplete()
    {
        // Scene transition/UI overlay
        OnLevelCompletecallback.Invoke();
    }

    public void GameOver()
    {
        // Scene transition/UI overlay
        OnGameOvercallback.Invoke();       
    }
}
