using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoaderService : MonoBehaviour
{
    enum STATE { START, LOADING, FINISHED };
    static SceneLoaderService instance;

    public string nextScene;
    public Renderer sphereRender;

    AsyncOperation _sceneAyncHandler;
    STATE _currState = STATE.START;

    float _progress = 0f;

    // Static Actions

    public static Action<float> OnProgress;
    public static Action<string, Action> OnError;
    public static Action<float, Action> OnFadeCanvas;
    public static Action<string, Action> OnNewQuote;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        HttpLoader.OnResultQuotes += (quotes) => {

            // if (quotes == null)
            // {           
            //     OnError?.Invoke("Network Error", () => {
            //         HttpLoader.LoadQuotes();
            //     });            
            // }
            // else
            {
                AnimateSlider(Progress());
                StartSceneLoading();
            }
        };

        OnFadeCanvas?.Invoke(1f, () => {
            HttpLoader.LoadQuotes();
        });       
    }


    // Graphics

    public void HideCanvas()
    {
        OnFadeCanvas?.Invoke(0f, () => {
            Util.ExecuteValue(1f, 0f, 5f, () => {}, (f) => {
                sphereRender.material.SetFloat("_Progress", f);
            });
        });
    }

    float Progress()
    {
        _progress += 0.1f;
        return _progress;
    }

    void AnimateSlider(float newvalue)
    {
        OnProgress?.Invoke(newvalue);
    }

    // Scene Loading

    void StartSceneLoading()
    {
        _sceneAyncHandler = SceneManager.LoadSceneAsync(nextScene);
        _sceneAyncHandler.allowSceneActivation = false;
        InvokeRepeating(nameof(CheckSceneProgress), 0.2f, 0.2f);
        _sceneAyncHandler.completed += (async) => {
            Debug.Log("Completed");
            
            Util.ExecuteAfter(0.5f, () => {
                _sceneAyncHandler.allowSceneActivation = true;
            });
        };
    }

    void CheckSceneProgress()
    {
        var tempNewValue = _progress + (_sceneAyncHandler.progress * 0.6f);
        AnimateSlider(tempNewValue);

        if (_sceneAyncHandler.progress >= 0.90f)
        {
            CancelInvoke(nameof(CheckSceneProgress));
            AnimateSlider(0.7f);
            _sceneAyncHandler.allowSceneActivation = true;
        }
    }

    // Static Interface

    public static void HideLoader()
    {
        Debug.Log("H1");
        instance.HideCanvas();
    }
    public static void SetProgress(float newvalue)
    {
        instance.AnimateSlider(0.7f + (newvalue * 0.3f));
    }
  
}



