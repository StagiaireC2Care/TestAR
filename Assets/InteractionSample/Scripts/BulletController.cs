using UnityEngine;

/// <summary>
/// Control the life cycle of the bullet
/// 1. The bullet will be destroyed within a certain time
/// 2. The bullet is destroyed on impact
/// </summary>
public class BulletController : MonoBehaviour
{
    public float deadTime = 5f;
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        deadTime -= Time.deltaTime;
        if(deadTime < 0) Destroy(gameObject);
    }

    //Provide the function to add force to fire bullet
    public void AddForce(Vector3 force)
    {
        rigidbody.AddForce(force , ForceMode.Impulse);
    }
    
    //The first frame callback for the collision
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

