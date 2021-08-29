using System.Collections;
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
        public SpriteRenderer DocSpriteRenderer;
        public SpriteRenderer StanleySpriteRenderer;

        public TextAsset InkFile;

        private Story _inkStory;
        private bool _storyStarted;

        private void Start()
        {
            _inkStory = new Story(InkFile.text);

            _inkStory.BindExternalFunction("setupScene", (string sceneName) => { SceneSetup.Setup(sceneName); });

            _inkStory.BindExternalFunction("endDialog", () => { GlobalGameState.FreePlay = true; });

            _inkStory.BindExternalFunction("look", (string who, string where) =>
            {
                Debug.Log("look " + who + " -> " + where);
                var whoSpriteRenderer = who == "STANLEY" ? StanleySpriteRenderer : DocSpriteRenderer;
                if (who == "STANLEY" && where == "DOC")
                {
                    where = StanleySpriteRenderer.transform.position.x > DocSpriteRenderer.transform.position.x ? "LEFT" : "RIGHT";
                }

                if (who == "DOC" && where == "STANLEY")
                {
                    where = StanleySpriteRenderer.transform.position.x < DocSpriteRenderer.transform.position.x ? "LEFT" : "RIGHT";
                }

                if (where == "LEFT")
                {
                    whoSpriteRenderer.flipX = true;
                }

                if (where == "RIGHT")
                {
                    whoSpriteRenderer.flipX = false;
                }
            });
        }

        private void Sleep(float time)
        {
            Debug.Log("sleep " + time);
            
            var hideUiBefore = GlobalGameState.HideUi;
            GlobalGameState.HideUi = true;

            StartCoroutine(Sleep(time, hideUiBefore));
        }

        private IEnumerator Sleep(float time, bool hideUiBefore)
        {
            yield return new WaitForSeconds(time);
            
            Continue();

            yield return null;
        
            GlobalGameState.HideUi = hideUiBefore;
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
            Debug.Log("Continue");
            if (CanContinue())
            {
                if (changeFreePlay)
                {
                    GlobalGameState.FreePlay = false;
                }

                var nextLine = _inkStory.Continue();
                if (nextLine.StartsWith(">>> SLEEP"))
                {
                    Sleep(float.Parse(nextLine.Replace(">>> SLEEP ", "")));
                }
                Debug.Log("Continue: " + nextLine);
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
