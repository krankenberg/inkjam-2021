using System.Collections.Generic;
using InkFiles;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ChoiceControl : MonoBehaviour
    {
        public TextControl TextControl;
        public Color DefaultColor;
        
        private GameObject[] _buttonGameObjects;
        private TextMeshProUGUI[] _texts;
        private InkStory _inkStory;

        private void Start()
        {
            var transformChildCount = transform.childCount;
            _buttonGameObjects = new GameObject[transformChildCount];
            _texts = new TextMeshProUGUI[transformChildCount];
            for (int i = 0; i < transformChildCount; i++)
            {
                var buttonTransform = transform.GetChild(i);
                _buttonGameObjects[i] = buttonTransform.gameObject;
                _texts[i] = buttonTransform.GetComponentInChildren<TextMeshProUGUI>();
            }

            _inkStory = GameObject.FindWithTag("StoryManager").GetComponent<InkStory>();
            ClearChoices();
        }

        public void ClearChoices()
        {
            for (var i = 0; i < _buttonGameObjects.Length; i++)
            {
                _buttonGameObjects[i].SetActive(false);
            }
        }

        public void SetChoices(List<string> choices)
        {
            ClearChoices();
            for (var i = 0; i < _buttonGameObjects.Length; i++)
            {
                if (i < choices.Count)
                {
                    var choiceText = choices[i];
                    var speakerColor = TextControl.SpeakerColorOfText(ref choiceText);
                    _buttonGameObjects[i].gameObject.SetActive(true);
                    _texts[i].text = speakerColor == null ? choiceText : $"'{choiceText}'";
                    _texts[i].color = speakerColor?.Color ?? DefaultColor;
                }
            }
        }

        public void ClickChoice(int choiceIndex)
        {
            _inkStory.ChooseChoice(choiceIndex);
        }
        
    }
}
