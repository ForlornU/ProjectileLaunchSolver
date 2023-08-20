using UnityEngine;

public class TrackingTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform startPosition;
    [SerializeField] float power = 30f;

    UIVisualizer ui;

    private void Start()
    {
        ui = GetComponent<UIVisualizer>();
    }

    public LaunchData Calculate(TargetData data)
    {
        RotateArcher(data.targetPosition);
        LaunchArrow(data);

        //This data is not used, but required. This class is too simple to have any use for it
        LaunchData fauxData = new LaunchData();
        return fauxData;
    }
    public void Launch(LaunchData data)
    {
        Debug.Log("Left click to fire a predicting arrow at a moving target");
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target.With(y: transform.position.y));
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(TargetData target)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        Rigidbody targetRigidbody = target.targetObject.GetComponent<Rigidbody>();
        Vector3 velocity;

        if (targetRigidbody != null)
            velocity = AnticipateVelocity(targetRigidbody);
        else
            velocity = startPosition.position.DirectionTo(target.targetPosition).normalized;

        ui.StraightLine(startPosition.position, velocity, power);

        newArrow.GetComponent<Rigidbody>().velocity = velocity * power;
    }

    float TimeToImpact(Rigidbody target)
    {
        Vector3 estimatedRigidbodyVelocity = (target.position - startPosition.position).normalized * power;

        float distance = Vector3.Distance(startPosition.position, target.position);

        Vector3 relativeVelocity = estimatedRigidbodyVelocity - target.velocity;

        return distance / relativeVelocity.magnitude;
    }

    Vector3 AnticipateVelocity(Rigidbody target)
    {
        float timeToHit = TimeToImpact(target);
        Vector3 expectedPosition = target.position + target.velocity * timeToHit;
        Vector3 dir = expectedPosition - startPosition.position;

        Debug.Log("Time until hitting target: " + timeToHit);
        return dir.normalized;
    }

}
