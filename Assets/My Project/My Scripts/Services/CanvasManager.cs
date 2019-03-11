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
    void Start()
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
            LeanTween.value(progressSlider.gameObject, progressSlider.value, newprogress, 0.08f).setOnUpdate( f => { progressSlider.value = f;});
        };

        SceneLoaderService.OnNewQuote += (newtext, callback) => {
            quoteText.text = newtext;
            Util.Fade(quoteCg, 1f, () => {
                Util.Fade(quoteCg, 0f, () => {
                    callback?.Invoke();
                }, 3f);
            });
        };

    }

}
