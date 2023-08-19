using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField]
    float sensitivity = 3f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude <= sensitivity)
            return;
        
        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        //sensitivity *= 2f;
    }
}
