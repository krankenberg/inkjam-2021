using UnityEngine;

public class Marker : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 0.1F);
    }
}
