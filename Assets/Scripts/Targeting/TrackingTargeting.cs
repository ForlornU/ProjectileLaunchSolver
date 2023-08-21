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
        LaunchData newData = CalculateFlight(data);
        ui.WriteToUi(newData);
        ui.StraightLine(startPosition.position, newData.initialVelocity);
        LaunchArrow(newData);
        return newData;
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

    LaunchData CalculateFlight(TargetData target)
    {
        LaunchData arrowData = new LaunchData();
        Rigidbody targetRigidbody = target.targetObject.GetComponent<Rigidbody>();

        Vector3 velocity = GetArrowVelocity(targetRigidbody, target);
        if (targetRigidbody != null)
            arrowData.timeToTarget = TimeToImpact(targetRigidbody);

        return FillArrowData(arrowData, velocity, target);
    }

    Vector3 GetArrowVelocity(Rigidbody targetRigidbody, TargetData target)
    {
        Vector3 v = targetRigidbody!= null ? AnticipateVelocity(targetRigidbody).normalized : startPosition.position.DirectionTo(target.targetPosition).normalized;
        return v;
    }

    LaunchData FillArrowData(LaunchData arrowData, Vector3 velocity, TargetData target)
    {
        arrowData.initialVelocity = velocity * power;
        arrowData.initialPosition = startPosition.position;
        arrowData.targetPosition = target.targetPosition;
        arrowData.horizontalDistance = startPosition.position.DirectionTo(target.targetObject.position).With(y: 0f).magnitude;
        return arrowData;
    }

    void LaunchArrow(LaunchData lData)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = lData.initialVelocity;
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
        return dir.normalized;
    }

}
