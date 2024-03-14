using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private AutoTween FadeTween;
    [SerializeField] private CanvasGroup FadeCanvasGroup;

    public static SceneTransition Instance;

    private bool _transitioning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneIndex)
    {
        if (_transitioning)
        {
            return;
        }

        StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        _transitioning = true;

        FadeTween.Play(FadeCanvasGroup.LeanAlpha(1, FadeTween.Time)).setIgnoreTimeScale(true);

        yield return new WaitForSecondsRealtime(FadeTween.Time);

        var load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        load.allowSceneActivation = false;
        while (!load.isDone)
        {
            if (load.progress >= 0.9f)
            {
                load.allowSceneActivation = true;
            }
            yield return null;
        }

        FadeTween.Play(FadeCanvasGroup.LeanAlpha(0, FadeTween.Time)).setIgnoreTimeScale(true);

        _transitioning = false;
    }
}
