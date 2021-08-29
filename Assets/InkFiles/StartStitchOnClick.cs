using System.Linq;
using UnityEngine;

namespace InkFiles
{
    public class StartStitchOnClick : MonoBehaviour
    {
        public string ToolTip;
        public string Stitch;
        public Transform[] InteractionPoints;

        private InkStory _inkStory;
        private Move _playerMove;
        private Transform _playerTransform;

        private void Start()
        {
            _inkStory = GameObject.FindWithTag("StoryManager").GetComponent<InkStory>();
            _playerMove = GameObject.FindWithTag("Player").GetComponent<Move>();
            _playerTransform = _playerMove.transform;
        }

        public void Click()
        {
            _playerMove.OnReachedTarget = () =>
            {
                _playerMove.LookAt = transform;
                _inkStory.StartStitch(Stitch);
            };
            _playerMove.WalkToPosition(GetClosestInteractionPoint());
        }

        private Vector3 GetClosestInteractionPoint()
        {
            return InteractionPoints == null || InteractionPoints.Length == 0
                ? transform.position
                : InteractionPoints
                    .Select(interactionPoint => interactionPoint.position)
                    .OrderBy(interactionPoint => Vector3.Distance(interactionPoint, _playerTransform.position))
                    .First();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (InteractionPoints != null)
            {
                var position = transform.position;
                foreach (var interactionPoint in InteractionPoints)
                {
                    if (interactionPoint != null)
                    {
                        Gizmos.DrawLine(position, interactionPoint.position);
                    }
                }
            }
        }
    }
}
