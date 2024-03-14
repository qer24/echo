using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ObjectiveTypewriterUI : MonoBehaviour
{
    [SerializeField] private float TypingSpeed = 0.1f;
    [SerializeField] private EventReference TypingSound;

    private TMPro.TextMeshProUGUI _objectiveText;
    private string _objectiveDescription;
    private int _currentCharacterIndex;

    private void Awake()
    {
        _objectiveText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        _objectiveText.text = string.Empty;
    }

    public void SetObjectiveDescription(string objectiveDescription)
    {
        _objectiveDescription = objectiveDescription;
    }

    public void StartTyping()
    {
        StartCoroutine(TypingCoroutine());
    }

    private IEnumerator TypingCoroutine()
    {
        _currentCharacterIndex = 0;

        while (_currentCharacterIndex < _objectiveDescription.Length)
        {
            var character = _objectiveDescription[_currentCharacterIndex];

            if (character != ' ')
            {
                TypingSound.Play();
            }

            _objectiveText.text += character;
            _currentCharacterIndex++;

            yield return new WaitForSeconds(TypingSpeed);
        }
    }
}
