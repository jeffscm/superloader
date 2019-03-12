// =====================================
// Author: Jefferson Scomacao (2019)
//
// Progressive Async Scene Loading
// using reactive code pattern
//
// Class CanvasManager
// Scene references and UI management
// this is the best example how RX code
// can be used in Unity
// =====================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{
    public CanvasGroup canvasCg;
    public Slider progressSlider;

    public CanvasGroup quoteCg;
    public Text quoteText;

    // Start is called before the first frame update
    void Awake()
    {
        progressSlider.value = 0f;
        canvasCg.alpha = 0f;
        quoteCg.alpha = 0f;

        SceneLoaderService.OnError += (errMsg, callback) => {


        };

        SceneLoaderService.OnFadeCanvas += (newvalue, callback) => {
            Util.Fade(canvasCg, newvalue, callback);
        };

        SceneLoaderService.OnProgress += (newprogress) => {
            LeanTween.cancel(progressSlider.gameObject);
            LeanTween.value(progressSlider.gameObject, progressSlider.value, newprogress, 0.2f).setOnUpdate( f => { progressSlider.value = f;});
        };

        SceneLoaderService.OnNewQuote += (newtext) => {
            quoteText.text = newtext;
            Util.Fade(quoteCg, 1f, () => {
                Util.Fade(quoteCg, 0f, () => {
                   
                }, 3f);
            });
        };

    }

}
