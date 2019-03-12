// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class HttpLoader
// Utility to download Quotes from URL
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class HttpLoader
{
    public static Action<List<string>> OnResultQuotes; 

    static UnityWebRequest _handler = null;

    public static void LoadQuotes()
    {
        _handler = UnityWebRequest.Get("http://paralagames.public.cloudvps.com/quotes/quotes.txt");
        var swb = _handler.SendWebRequest();
        swb.completed += (async) => {
            
            if (_handler.isNetworkError || _handler.isHttpError)
            {
                OnResultQuotes?.Invoke(null);
                Debug.LogError("Network error");
            }
            else
            {
                var json = _handler.downloadHandler.text;
                var quotes = JsonConvert.DeserializeObject<List<string>>(json);
                OnResultQuotes?.Invoke(quotes);
            }
        }; 
    }

    public static void Cancel()
    {
        if (_handler != null)
        {
            _handler.Abort();
        }
    }
}
