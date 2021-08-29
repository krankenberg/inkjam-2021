using UnityEngine;

public class Marker : MonoBehaviour
{
    public Transform[] InteractionPoints;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 0.1F);
        
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
