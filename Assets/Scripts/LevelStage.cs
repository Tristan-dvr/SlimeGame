using UnityEngine;

public class LevelStage : MonoBehaviour
{
    public Transform startPoint;
    [Space]
    public Vector3 size = Vector3.one;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var center = transform.position + new Vector3(size.x / 2, size.y / 2, 0);
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint.position, 1);
    }
}
