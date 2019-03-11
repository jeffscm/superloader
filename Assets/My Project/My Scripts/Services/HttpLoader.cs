using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class HttpLoader
{
    public static Action<Quotes> OnResultQuotes; 

    static UnityWebRequest _handler = null;

    public static void LoadQuotes()
    {
        _handler = UnityWebRequest.Get("http://www.error.error");
        var swb = _handler.SendWebRequest();
        swb.completed += (async) => {
            
            if (_handler.isNetworkError || _handler.isHttpError)
            {
                OnResultQuotes?.Invoke(null);
            }
            else
            {
                var json = _handler.downloadHandler.text;
                var quotes = JsonConvert.DeserializeObject<Quotes>(json);
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

public class Quotes
{
    public List<Quote> listQuotes { get; set; }
}
public class Quote
{
    public string itemQuote { get; set; }
}