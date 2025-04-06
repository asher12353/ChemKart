using UnityEngine;

public class PositionSynchronizer : MonoBehaviour
{
    public Transform position;
    public SphereCollider sphere;

    void Update()
    {
        if(position != null)
        {
            transform.position = new Vector3(position.position.x, position.position.y - sphere.radius, position.position.z);
        }
    }
}
