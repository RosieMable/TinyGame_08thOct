using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mainMenu, gameOver, gamePlay, victoryMusic;

    float initialVolume;

    [SerializeField] float fadeTime;

    [SerializeField] GameObject player;
    [SerializeField] GameObject tar;

    [SerializeField] bool gamePlaying = false;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        initialVolume = audioSource.volume;

        player = FindObjectOfType<PlayerController>().gameObject;


        GameManager.instance.OnGameOvercallback += GameOverMusic;
        GameManager.instance.OnLevelCompletecallback += VictoryMusic;

    }


    private void Update()
    {
        if (GameManager.instance.OnGameOvercallback == null)
            GameManager.instance.OnGameOvercallback += GameOverMusic;

        if (GameManager.instance.OnLevelCompletecallback == null)
            GameManager.instance.OnLevelCompletecallback += VictoryMusic;
    }

    public void FromStartToGame()
    {
        StartCoroutine(FromMainMenuToGame());
    }

    public void Retry()
    {
        StartCoroutine(ToMainMenu());
    }

    public void GameOverMusic()
    {
        StartCoroutine(ToGameOver());
    }

    public void VictoryMusic()
    {
        StartCoroutine(ToVictory());
    }

    IEnumerator FromMainMenuToGame()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, gamePlay));

        //do something here
        gamePlaying = true;

    }

    IEnumerator ToMainMenu()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, mainMenu));

    }

    IEnumerator ToGameOver()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, gameOver));


    }

    IEnumerator ToVictory()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, victoryMusic));
    }




    IEnumerator FadeAudio(AudioSource audio, float startVolume, float endvolume, float duration, AudioClip newMusic)
    {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        audio.volume = startVolume;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if (startVolume > endvolume)
            {
                audio.volume = startVolume - percentage;
            }
            else
            {
                audio.volume = startVolume + percentage;
            }
            yield return new WaitForEndOfFrame();
        }

        audio.volume = endvolume;
        audio.clip = newMusic;
        audio.Play();
        audio.volume = initialVolume;


    }

    private void OnDisable()
    {
        GameManager.instance.OnGameOvercallback -= GameOverMusic;
        GameManager.instance.OnLevelCompletecallback -= VictoryMusic;
    }
}
