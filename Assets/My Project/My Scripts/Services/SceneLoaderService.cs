// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class SceneLoaderService
// Handles Scene and Quotes
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoaderService : MonoBehaviour
{

    // static self reference    
    static SceneLoaderService instance;

    // public and references
    public string nextScene;
    public Renderer sphereRender;

    // private
    AsyncOperation _sceneAyncHandler;
    List<string> _quotes; // cached quotes

    // Static Actions
    public static Action<float> OnProgress;
    public static Action<string, Action> OnError;
    public static Action<float, Action> OnFadeCanvas;
    public static Action<string> OnNewQuote;

    // Unity methods

    void Awake()
    {
        // Thread:Low to avoid hiccups loading even if takes more time (loading async)
        Application.backgroundLoadingPriority = ThreadPriority.Low;        
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LazyFollower.Follow = true; // enable Lazy Camera Follower

        // Quotes are loaded from URL and returnes as list of strings
        // quotes are cached for later use
        HttpLoader.OnResultQuotes += (quotes) => { 

            if (quotes == null)
            {           
                OnError?.Invoke("Network Error", () => {
                    HttpLoader.LoadQuotes();
                });            
            }
            else
            {
                _quotes = quotes;
                AnimateSlider(0.1f);
                InvokeRepeating(nameof(GenerateQuote), 1f, 5f);
                Util.ExecuteAfter(5f, () => {                    
                    StartSceneLoading();
                });
            }
        };
        
        // Here is the start point of loading, 
        // First show the canvas with lader then ask do download quotes
        OnFadeCanvas?.Invoke(1f, () => {
            HttpLoader.LoadQuotes();
        });       
    }


    // Graphics

    public void HideCanvas()
    {
        CancelInvoke(nameof(GenerateQuote));
        OnFadeCanvas?.Invoke(0f, () => {
            Util.ExecuteValue(1f, 0f, 5f, () => {}, (f) => {
                sphereRender.material.SetFloat("_Progress", f);
            });

            LazyFollower.Follow = false;

        });
    }

    void AnimateSlider(float newvalue)
    {       
        OnProgress?.Invoke(newvalue);
    }

    void GenerateQuote()
    {
        var tempStr = _quotes[UnityEngine.Random.Range(0, _quotes.Count)];
        OnNewQuote?.Invoke(tempStr);
    }

    // Scene Loading

    void StartSceneLoading()
    {
        _sceneAyncHandler = SceneManager.LoadSceneAsync(nextScene);
        _sceneAyncHandler.allowSceneActivation = false;
        InvokeRepeating(nameof(CheckSceneProgress), 0.2f, 0.2f);
        _sceneAyncHandler.completed += (async) => {                    
            Util.ExecuteAfter(0.5f, () => {
                _sceneAyncHandler.allowSceneActivation = true;
            });
        };
    }

    void CheckSceneProgress()
    {
        var tempNewValue = 0.1f + (_sceneAyncHandler.progress * 0.6f);
        AnimateSlider(tempNewValue);        
        if (_sceneAyncHandler.progress >= 0.90f)
        {
            CancelInvoke(nameof(CheckSceneProgress));
            AnimateSlider(0.7f);
            Util.ExecuteAfter(0.5f, () => {
                _sceneAyncHandler.allowSceneActivation = true;
            });            
        }
    }

    // Static Interface
    // This is the end point for loading screen
    // When called it will fade out and stop Lazy Camera Follower
    public static void HideLoader()
    {
        instance.HideCanvas();
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }

    // Externally set Progress bar
    public static void SetProgress(float newvalue)
    {
        instance.AnimateSlider(0.7f + (newvalue * 0.3f));
    }
  
}



