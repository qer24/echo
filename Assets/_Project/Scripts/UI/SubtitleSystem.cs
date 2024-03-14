using System;
using System.Collections;
using System.Collections.Generic;
// using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubtitleSystem : MonoBehaviour
{
    [SerializeField] private CanvasGroup SubtitleCanvasGroup;
    [SerializeField] private TextMeshProUGUI SubtitleText;

    [Space]
    [SerializeField] private AutoTween AppearTween;
    [SerializeField] private AutoTween DisappearTween;

    [Space]
    [SerializeField] private Subtitle[] IntroSubtitles;

    [Serializable]
    private struct Subtitle
    {
        public string Text;
        // [HorizontalGroup]
        public float AppearTime;
        // [HorizontalGroup]
        public float DisappearTime;
        public UnityEvent OnDisappear;
    }

    private void Start()
    {
        SubtitleCanvasGroup.alpha = 0f;
    }

    public void StartSubtitles()
    {
        StartCoroutine(SubtitlesCoroutine());
    }

    private IEnumerator SubtitlesCoroutine()
    {
        var timer = 0f;
        var subtitleQueue = new Queue<Subtitle>(IntroSubtitles);

        var currentSubtitle = subtitleQueue.Dequeue();

        while (true)
        {
            if (timer >= currentSubtitle.AppearTime)
            {
                DisappearTween.Cancel();
                SubtitleText.text = currentSubtitle.Text;
                AppearTween.Play(SubtitleCanvasGroup.LeanAlpha(1, AppearTween.Time));
            }

            if (timer >= currentSubtitle.DisappearTime)
            {
                AppearTween.Cancel();
                DisappearTween.Play(SubtitleCanvasGroup.LeanAlpha(0, DisappearTween.Time));

                currentSubtitle.OnDisappear?.Invoke();

                if (subtitleQueue.Count == 0) break;

                currentSubtitle = subtitleQueue.Dequeue();
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
