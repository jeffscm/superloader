// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class Util
// Utility to encapsulate LeanTween
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Util
{
    public static void Fade(CanvasGroup cg, float value, Action onCompleted, float delay = 0f)
    {
        LeanTween.cancel(cg.gameObject);
        LeanTween.alphaCanvas(cg, value, 0.5f).setDelay(delay).setOnComplete( () => { onCompleted?.Invoke();} );
    }

    public static void ExecuteAfter(float timer, Action onCompleted)
    {       
        LeanTween.value(0f, 1f, timer).setOnComplete( () => { onCompleted?.Invoke();} );
    }

    public static void ExecuteValue(float from, float to, float timer, Action onCompleted, Action<float> onUpdated)
    {       
        LeanTween.value(from, to, timer).setOnComplete( () => { onCompleted?.Invoke();} ).setOnUpdate( (f) => { onUpdated?.Invoke(f); });
    }
}