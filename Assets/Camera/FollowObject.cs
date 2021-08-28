using UnityEngine;

namespace Camera
{
    public class FollowObject : MonoBehaviour
    {
        public Vector3 Offset = new Vector3(2.5F, 1.8F, -5F);
        public Transform FollowedTransform;
        private Transform _transform;

        private void Start()
        { 
            _transform = transform;
        }

        private void LateUpdate()
        {
            _transform.position = FollowedTransform.position + Offset;
        }
    }
}
