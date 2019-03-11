using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLoaderService : MonoBehaviour
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
        Debug.Log("H2");
        DownloadItem( () => {
            Debug.Log("H3");
            Util.ExecuteAfter(defaultDelay, () => {
                Debug.Log("H4");
                ExecuteDownloadStep();
            });
        },
        () => {
            Debug.Log("H5");
            // Completed Event
            SceneLoaderService.HideLoader();
        });
    }

    void DownloadItem(Action onStep, Action onCompleted)
    {
        if (idx == listInitPrefabs.Count)
        {
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
