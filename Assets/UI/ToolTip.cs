using TMPro;
using UnityEngine;

namespace UI
{
    public class ToolTip : MonoBehaviour
    {
        public string Text;

        private TextMeshProUGUI _text;
        private RectTransform _textRectTransform;
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _textRectTransform = GetComponent<RectTransform>();
            _camera = GameObject.FindWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
        }

        private void LateUpdate()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var mousePosition = Input.mousePosition;
                var viewportPoint = _camera.ScreenToViewportPoint(mousePosition);
                _textRectTransform.anchoredPosition = new Vector2(viewportPoint.x * 256, viewportPoint.y * 256);
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
