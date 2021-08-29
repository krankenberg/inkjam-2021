using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InkFiles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class TextControl : MonoBehaviour
    {
        public GameObject Text;
        public GameObject NextPageIndicator;
        public GameObject Portrait;
        public float TimeUntilNextCharacter = 0.01F;

        public Color DefaultColor;
        public List<SpeakerColor> SpeakerColors;

        public float AnchorWithoutPortrait = 0;
        public float AnchorWithPortrait = 0.25F;

        private TextMeshProUGUI _textMeshPro;
        private Animator _indicatorAnimatorController;
        private Text _indicatorText;
        private Image _portraitImage;
        private InkStory _inkStory;
        private RectTransform _textRectTransform;

        private float _passedTime;
        private bool _reachedEndOfText;

        private bool _textWasJustSet;

        private static readonly int Visible = Animator.StringToHash("Visible");

        private void Start()
        {
            _textMeshPro = Text.GetComponent<TextMeshProUGUI>();
            _indicatorAnimatorController = NextPageIndicator.GetComponent<Animator>();
            _indicatorText = NextPageIndicator.GetComponent<Text>();
            _portraitImage = Portrait.GetComponent<Image>();
            _inkStory = GameObject.FindWithTag("StoryManager").GetComponent<InkStory>();
            _textRectTransform = Text.GetComponent<RectTransform>();

            GlobalGameState.OnHideUiChange += OnHideUiChange;

            transform.gameObject.SetActive(false);
        }

        private void OnHideUiChange(bool hideUi)
        {
            transform.gameObject.SetActive(!hideUi);
        }

        private void Update()
        {
            if (_textWasJustSet)
            {
                _textWasJustSet = false;
                return;
            }

            var currentPageInfo = GetCurrentPageInfo();

            FadeInLetters(currentPageInfo);

            if (!_reachedEndOfText && !HasMorePages() && IsAtEndOfPage(currentPageInfo))
            {
                _reachedEndOfText = true;
                _inkStory.ShowChoices();
            }

            _indicatorAnimatorController.SetBool(Visible, (HasMorePages() || _inkStory.CanContinue()) && IsAtEndOfPage(currentPageInfo));

            if (Input.GetButtonUp("Fire1"))
            {
                var pointerEventData = new PointerEventData(EventSystem.current)
                {
                    button = PointerEventData.InputButton.Left
                };
                NextPage(pointerEventData);
            }
        }

        private void FadeInLetters(TMP_PageInfo currentPageInfo)
        {
            if (!IsAtEndOfPage(currentPageInfo))
            {
                _passedTime += Time.unscaledDeltaTime;
                while (_passedTime > TimeUntilNextCharacter)
                {
                    _textMeshPro.maxVisibleCharacters++;
                    _passedTime -= TimeUntilNextCharacter;
                }
            }
        }

        public void SetText(string text)
        {
            _textMeshPro.pageToDisplay = 1;
            _textMeshPro.maxVisibleCharacters = 0;
            _reachedEndOfText = false;
            _textMeshPro.SetText(ParseInkText(text));
            _textWasJustSet = true;
        }

        private string ParseInkText(string text)
        {
            var speakerColor = SpeakerColorOfText(ref text);

            _textMeshPro.color = DefaultColor;
            _indicatorText.color = DefaultColor;
            ShowNoPortrait();

            if (speakerColor != null)
            {
                _textMeshPro.color = speakerColor.Color;
                _indicatorText.color = speakerColor.Color;
                if (speakerColor.Portrait != null)
                {
                    _portraitImage.sprite = speakerColor.Portrait;
                    Portrait.SetActive(true);
                    _textRectTransform.anchorMin = new Vector2(AnchorWithPortrait, 0);
                }
            }

            return text;
        }

        private void ShowNoPortrait()
        {
            Portrait.SetActive(false);
            _textRectTransform.anchorMin = new Vector2(AnchorWithoutPortrait, 0);
        }

        public SpeakerColor SpeakerColorOfText(ref string text)
        {
            var textParts = text.Split(':');

            foreach (var speakerColor in SpeakerColors)
            {
                if (textParts[0].Equals(speakerColor.Identifier, StringComparison.OrdinalIgnoreCase))
                {
                    text = text.Replace(textParts[0] + ": ", "")
                        .Replace("\\n", "\n");        
                    text = Regex.Replace(text, "\\*{2}(.+)\\*{2}", ReplaceBold);
                    text = Regex.Replace(text, "\\*(.+)\\* ?", ReplaceEmotes);
                    return speakerColor;
                }
            }

            return null;
        }
        
        private string ReplaceEmotes(Match match)
        {
            var withoutStars = match.Groups[1].Value;
            return $"{OpenColor(DefaultColor)}({withoutStars}){CloseColor()}\n";
        }
        
        private string ReplaceBold(Match match)
        {
            var withoutStars = match.Groups[1].Value;
            return $"<b>{withoutStars}</b>";
        }

        private string OpenColor(Color color)
        {
            return $"<#{ ColorUtility.ToHtmlStringRGBA(color)}>";
        }

        private string CloseColor()
        {
            return "</color>";
        }

        public void NextPage(BaseEventData baseEventData)
        {
            var currentPageInfo = GetCurrentPageInfo();
            if (IsAtEndOfPage(currentPageInfo))
            {
                var pointerEventData = baseEventData as PointerEventData;
                var leftMouseButton = pointerEventData.button == PointerEventData.InputButton.Left;
                var rightMouseButton = pointerEventData.button == PointerEventData.InputButton.Right;
                var newPage = _textMeshPro.pageToDisplay + (leftMouseButton ? 1 : rightMouseButton ? 1 : 0);

                _textMeshPro.pageToDisplay = ClampPage(newPage);

                if (newPage > _textMeshPro.textInfo.pageCount)
                {
                    _inkStory.Continue();
                }
            }
            else
            {
                _textMeshPro.maxVisibleCharacters = !HasMorePages() ? _textMeshPro.textInfo.characterCount : currentPageInfo.lastCharacterIndex;
            }
        }

        private bool IsAtEndOfPage(TMP_PageInfo currentPageInfo)
        {
            var lastPage = !HasMorePages();
            return lastPage && _textMeshPro.maxVisibleCharacters >= _textMeshPro.textInfo.characterCount ||
                   !lastPage && _textMeshPro.maxVisibleCharacters >= currentPageInfo.lastCharacterIndex;
        }

        private int ClampPage(int newPage)
        {
            return Mathf.Clamp(newPage, 1, _textMeshPro.textInfo.pageCount);
        }

        private bool HasMorePages()
        {
            var pageCount = _textMeshPro.textInfo.pageCount;
            var currentPage = _textMeshPro.pageToDisplay;
            return pageCount > 1 && currentPage < pageCount;
        }

        private TMP_PageInfo GetCurrentPageInfo()
        {
            return _textMeshPro.textInfo.pageInfo[_textMeshPro.pageToDisplay - 1];
        }
    }
}
