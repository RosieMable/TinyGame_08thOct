using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] float fadeTime = 2f;

    public static bool paused;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OptionsMenu;

    private void Awake()
    {
        canvasGroup = this.GetComponentInChildren<CanvasGroup>();
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    public void OnStart()
    {
        StartCoroutine(FadeThanDoSomething());
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnPause()
    {
        PauseGame();
        Debug.Log(Time.timeScale);
    }

    public void OnOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
    }

    void PauseGame()
    {
        if (paused == false)
        {
            Time.timeScale = 0f;
            paused = true;
        }
        else
        {
            Time.timeScale = 1f;
            paused = false;
        }
    }

    IEnumerator FadeThanDoSomething()
    {
        yield return StartCoroutine(FadeCanvas(canvasGroup, 1f, 0f, fadeTime));

        //do something here

        ToActivateOnStart();
    }

    IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        canvas.alpha = startAlpha;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime;
            var percentage = 1 / (duration / elapsedTime);
            if (startAlpha > endAlpha)
            {
                canvas.alpha = startAlpha - percentage;
            }
            else
            {
                canvas.alpha = startAlpha + percentage;
            }
            yield return new WaitForEndOfFrame();
        }
        canvas.alpha = endAlpha;
        canvas.gameObject.SetActive(false);
    }

    void ToActivateOnStart()
    {
        PauseMenu.SetActive(true);
    }

   

}
