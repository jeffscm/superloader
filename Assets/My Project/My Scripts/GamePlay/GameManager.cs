// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class GameLoaderService
// Manages the GamePlay and loading
// prefabs for the scene and after
// hide de loading screen (this is on
// of the ways to use async loading)
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public List<GameObject> listInitPrefabs;
    public float defaultDelay = 0.1f;

    int idx = 0;

    private void Start()     
    {
        // After loading the Scene, wait until base prefabs are instantiated
        ExecuteDownloadStep();
    }

    void ExecuteDownloadStep()
    {      
        DownloadItem( () => {
            Util.ExecuteAfter(defaultDelay, () => {
                ExecuteDownloadStep();
            });
        },
        () => {
           
            // Completed Event
            SceneLoaderService.HideLoader();
        });
    }

    void DownloadItem(Action onStep, Action onCompleted)
    {
        if (idx == listInitPrefabs.Count)
        {
            SceneLoaderService.SetProgress(1f);
            onCompleted?.Invoke();
        }
        else
        {
            Instantiate(listInitPrefabs[idx]);
            idx++;
            if (listInitPrefabs.Count == 0)
            {
                SceneLoaderService.SetProgress(1f);
            }
            else
            {
                SceneLoaderService.SetProgress((float)idx/(float)listInitPrefabs.Count);    
            }
            onStep?.Invoke();
        }
    }
}
