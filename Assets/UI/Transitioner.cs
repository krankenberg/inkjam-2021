using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Transitioner : MonoBehaviour
    {
        public float FadeInTime = 1F;
        public float FadeOutTime = 1F;

        private bool _fadeIn;
        private bool _fadeOut;

        private Image _image;

        private float _time;

        public bool FadeIn
        {
            get => _fadeIn;
            set
            {
                _fadeIn = value;
                transform.gameObject.SetActive(true);
                SetAlpha(0);
            }
        }

        public bool FadeOut
        {
            get => _fadeOut;
            set
            {
                _fadeOut = value;
                transform.gameObject.SetActive(true);
                SetAlpha(1);
            }
        }

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_fadeIn)
            {
                _time += Time.deltaTime;

                SetAlpha(_time / FadeInTime);

                if (_time > FadeInTime)
                {
                    _fadeIn = false;
                    _time = 0;
                }
            }
            else if (_fadeOut)
            {
                _time += Time.deltaTime;

                SetAlpha(1 - _time / FadeOutTime);

                if (_time > FadeOutTime)
                {
                    transform.gameObject.SetActive(false); 
                    _fadeOut = false;
                    _time = 0;
                }
            }
        }

        private void SetAlpha(float alpha)
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            
            var oldColor = _image.color;
            _image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        }
    }
}
