using UnityEngine;

public class TrackingTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField]
    bool pauseDebug = true;
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    float power = 15f;

    public void Launch(TargetData NewTarget)
    {
        if (!NewTarget.targetObject.GetComponent<Rigidbody>())
            return;

        RotateArcher(NewTarget.targetPosition);
        LaunchArrow(NewTarget.targetObject);
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Transform target)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);

        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();

        (Vector3 expectedPosition, Vector3 dir) = AnticipateVelocity(targetRigidbody);

        if (pauseDebug)
        {
            Debug.DrawLine(startPosition.position, targetRigidbody.position, Color.green);
            Debug.DrawLine(startPosition.position, expectedPosition, Color.red);
            Debug.Break();
        }

        newArrow.GetComponent<Rigidbody>().velocity = dir.normalized * power;
    }

    float TimeToImpact(Rigidbody target)
    {
        Vector3 estimatedRigidbodyVelocity = (target.position - startPosition.position).normalized * power;

        float distance = Vector3.Distance(startPosition.position, target.position);

        Vector3 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;

        return distance / relativeVelocity.magnitude;
    }

    (Vector3 anticipatedPos, Vector3 newDirection) AnticipateVelocity(Rigidbody target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - startPosition.position;

        Debug.Log("Time until hitting target: " + timeToHit);
        return (expectedPosition, dir);
    }

}
