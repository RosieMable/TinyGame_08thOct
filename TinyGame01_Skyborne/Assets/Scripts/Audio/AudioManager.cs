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

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        initialVolume = audioSource.volume;

        player = FindObjectOfType<PlayerController>().gameObject;


        GameManager.instance.OnGameOvercallback += GameOverMusic;

    }

    void Start()
    {

       // StartCoroutine(AdjustVolume());

    }

    private void Update()
    {
        if (GameManager.instance.OnGameOvercallback == null)
            GameManager.instance.OnGameOvercallback += GameOverMusic;

    }

    IEnumerator AdjustVolume()
    {

        while (gamePlaying == true)
        {
            tar = FindObjectOfType<Tar>().gameObject;

            if (audioSource.isPlaying)
            { // do this only if some audio is being played in this gameObject's AudioSource

                float distanceToTarget = Vector3.Distance(player.transform.position, tar.transform.position); // Assuming that the target is the player or the audio listener

                if (distanceToTarget < 1) { distanceToTarget = 1; }

                audioSource.volume = 1 / distanceToTarget; // this works as a linear function, while the 3D sound works like a logarithmic function, so the effect will be a little different (correct me if I'm wrong)

                yield return new WaitForSeconds(1); // this will adjust the volume based on distance every 1 second (Obviously, You can reduce this to a lower value if you want more updates per second)

            }
        }

        audioSource.volume = initialVolume;

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

    IEnumerator FromMainMenuToGame()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, gamePlay));

        //do something here
        gamePlaying = true;

    }

    IEnumerator ToMainMenu()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, mainMenu));

        //do something here
        gamePlaying = false;
    }

    IEnumerator ToGameOver()
    {
        yield return StartCoroutine(FadeAudio(audioSource, initialVolume, 0f, fadeTime, gameOver));

        //do something here
        gamePlaying = false;

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
    }
}
