using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    float minVelocity = 0.1f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude <= minVelocity)
            return;
        
        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent = collision.transform;
        Destroy(rb);
        Destroy(this);
    }
}
