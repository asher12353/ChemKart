using UnityEngine;

public class SyncWithChildsPosition : MonoBehaviour
{
    public Transform child;
    public SphereCollider sphere;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(child != null)
        {
            transform.position = new Vector3(child.position.x, child.position.y - sphere.radius, child.position.z);
        }
    }
}
