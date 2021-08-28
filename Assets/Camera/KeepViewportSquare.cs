using UnityEngine;

namespace Camera
{
    [ExecuteInEditMode]
    public class KeepViewportSquare : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            var screenHeight = Screen.height;
            var screenWidth = Screen.width;
            if (screenWidth > screenHeight)
            {
                var viewportWidth = (float) screenHeight / screenWidth;
                var viewportX = (1 - viewportWidth) / 2;
                GetCamera().rect = new Rect(viewportX, 0, viewportWidth, 1);
            } else if (screenHeight > screenWidth)
            {
                var viewportHeight = (float) screenWidth / screenHeight;
                var viewportY = (1 - viewportHeight) / 2;
                GetCamera().rect = new Rect(0, viewportY, 1, viewportHeight);
            }
            else
            {
                GetCamera().rect = new Rect(0, 0, 1, 1);
            }
        }

        private UnityEngine.Camera GetCamera()
        {
            if (_camera == null)
            {
                _camera = GetComponent<UnityEngine.Camera>();
            }

            return _camera;
        }
    }
}
