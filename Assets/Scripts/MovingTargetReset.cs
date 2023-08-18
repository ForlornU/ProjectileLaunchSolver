using UnityEngine;

public class MovingTargetReset : MonoBehaviour
{
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {

        if (transform.position.y < -5f)
        {
            Reset();
        }
    }

    private void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.position = startPosition;
    }
}
