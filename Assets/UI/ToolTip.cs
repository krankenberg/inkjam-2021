using TMPro;
using UnityEngine;

namespace UI
{
    public class ToolTip : MonoBehaviour
    {
        public string Text;

        private TextMeshProUGUI _text;
        private RectTransform _textRectTransform;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _textRectTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var mousePosition = Input.mousePosition;
                _textRectTransform.anchoredPosition = new Vector2(mousePosition.x / Screen.width * 256, mousePosition.y / Screen.height * 256);
                _text.text = Text;
            }
            else
            {
                _textRectTransform.anchoredPosition = new Vector2(1000, 0);
                _text.text = "";
            }
        }
    }
}
