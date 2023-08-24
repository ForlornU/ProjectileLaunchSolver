using UnityEngine;

public class AdvancedTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject arrow;
    [SerializeField, Range(0.1f, 20f)] float maxHeightRange = 2f;
    [SerializeField] bool random = false;
    UIVisualizer ui;

    private void Start()
    {
        ui = GetComponent<UIVisualizer>();
    }

    public LaunchData Calculate(TargetData data)
    {
        RotateArcher(data.targetPosition);
        LaunchData launchData = CalculateLaunch(data.targetPosition);
        launchData.initialPosition = startPosition.position;

        ui.UpdateLine(launchData);
        ui.WriteToUi(launchData);

        return launchData;
    }
    public void Launch(LaunchData data)
    {
        LaunchArrow(data.initialVelocity);
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target.With(y:transform.position.y));
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Vector3 vel)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = vel;
    }

    LaunchData CalculateLaunch(Vector3 targetPosition)
    {
        LaunchData data = new LaunchData();

        data.gravity = Physics.gravity.y;
        float distanceY = targetPosition.y - startPosition.position.y;
        Vector3 horizontalDirection = startPosition.position.DirectionTo(targetPosition).With(y: 0f);

        float height = RandomizeHeight(distanceY);
        float time = CalculateTimeToTarget(height, data.gravity, distanceY);
        Vector3 velocity = CalculateInitialVelocity(height, horizontalDirection, data.gravity, time);

        data.horizontalDistance = horizontalDirection.magnitude;
        data.initialVelocity = velocity;
        data.timeToTarget = time;
        data.targetPosition = targetPosition;
        return data;
    }

    float CalculateTimeToTarget(float height, float gravity, float distanceY)
    {
        return Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (distanceY - height) / gravity);
    }

    Vector3 CalculateInitialVelocity(float height, Vector3 horizontalDirection, float gravity, float time)
    {
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = horizontalDirection * (1 / time);
        Vector3 velocityFinal = velocityY + velocityXZ;
        velocityFinal *= -Mathf.Sign(gravity);
        return velocityFinal;
    }

    float RandomizeHeight(float relativeHeight)
    {
        //Larger values yield higher arc, must never be below 0
        float minValue = (relativeHeight > 0) ? relativeHeight + 0.1f : 0.1f;
        float maxValue = (relativeHeight > 0) ? relativeHeight + maxHeightRange : minValue + maxHeightRange;

        float randomizedHeight = random ? Random.Range(minValue, maxValue) : Mathf.Lerp(minValue, maxValue, 0.5f);
        return randomizedHeight;
    }
}
