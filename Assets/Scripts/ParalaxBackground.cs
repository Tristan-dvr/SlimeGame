using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    public Transform follow;
    public float offset;
    public float paralaxScale = 1;

    private void LateUpdate()
    {
        SetPosition(follow.position.x * paralaxScale + offset);
    }

    private void SetPosition(float x)
    {
        var position = transform.position;
        position.x = x;
        transform.position = position;
    }
}
