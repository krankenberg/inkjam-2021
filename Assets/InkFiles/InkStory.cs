using System;
using System.Linq;
using Ink.Runtime;
using UI;
using UnityEngine;

namespace InkFiles
{
    public class InkStory : MonoBehaviour
    {
        public TextControl TextControl;
        public ChoiceControl ChoiceControl;
        public SceneSetup SceneSetup;

        public TextAsset InkFile;

        private Story _inkStory;
        private bool _storyStarted;

        private void Start()
        {
            _inkStory = new Story(InkFile.text);
            
            _inkStory.BindExternalFunction ("setupScene", (string sceneName) =>
            {
                SceneSetup.Setup(sceneName);
            });  
            
            _inkStory.BindExternalFunction ("endDialog", () =>
            {
                GlobalGameState.FreePlay = true;
            });  
        }

        private void Update()
        {
            if (!_storyStarted)
            {
                _storyStarted = true;
                Continue();
            }
        }

        public void ChooseChoice(int index)
        {
            ChoiceControl.ClearChoices();
            _inkStory.ChooseChoiceIndex(index);
            
            Continue();
        }

        public bool CanContinue()
        {
            return _inkStory.canContinue;
        }

        public void Continue(bool changeFreePlay = true)
        {
            if (CanContinue())
            {
                if (changeFreePlay)
                {
                    GlobalGameState.FreePlay = false;
                }

                var nextLine = _inkStory.Continue();
                if (_inkStory.currentTags.Contains("JUST_INKY"))
                {
                    Debug.Log("JUST_INKY: " + nextLine);
                    Continue(false);
                    return;
                }
                TextControl.SetText(nextLine);
            }
        }

        public void ShowChoices()
        {
            if (_storyStarted)
            {
                var choiceCount = _inkStory.currentChoices.Count;
                if (choiceCount > 0)
                {
                    GlobalGameState.FreePlay = false;
                    ChoiceControl.SetChoices(_inkStory.currentChoices.Select(choice => choice.text).ToList());
                }
            }
        }

        public void StartStitch(string stitchName)
        {
            var choice = _inkStory.currentChoices[0];
            var currentPath = choice.sourcePath;
            var pathParts = currentPath.Split('.');
            var knotName = pathParts[0];
            var newPath = knotName + "." + stitchName;
            Debug.Log("Start stitch: " + newPath);

            _inkStory.ChoosePathString(newPath);
            Continue();
        }
        
    }
}
