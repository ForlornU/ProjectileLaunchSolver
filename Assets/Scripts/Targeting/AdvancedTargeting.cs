using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AdvancedTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject arrow;
    [SerializeField] int lineResolution = 30;
    [SerializeField, Range(0.1f, 20f)] float maxHeightRange = 2f;
    [SerializeField] TMPro.TMP_Text dataToText;

    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public LaunchData Calculate(TargetData data)
    {
        RotateArcher(data.targetPosition);
        LaunchData launchData = CalculateLaunch(data.targetPosition);
        DrawPath(launchData);
        PrintData(launchData);
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
        //Randomize Height, larger values yield higher arc, must never be below 0
        float minValue = (relativeHeight > 0) ? relativeHeight + 0.1f : 0.1f;
        float maxValue = (relativeHeight > 0) ? relativeHeight + maxHeightRange : minValue + maxHeightRange;
        float randomizedHeight = Random.Range(minValue, maxValue);
        Debug.Log(randomizedHeight);
        return randomizedHeight;
    }

    void DrawPath(LaunchData launchData)
    {
        lineRenderer.positionCount = lineResolution;
        Vector3 drawPoint = startPosition.position;
        float debugHighestPoint = 0f;

        for (int i = 1; i <= lineResolution; i++)
        {
            float simulationTime = i / (float)lineResolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * launchData.gravity * simulationTime * simulationTime / 2f;
            drawPoint = startPosition.position + displacement;

            lineRenderer.SetPosition(i-1, drawPoint);

            if(drawPoint.y > debugHighestPoint)
                debugHighestPoint = drawPoint.y;
        }
    }

    void PrintData(LaunchData data)
    {
        dataToText.text = $"{data.initialVelocity.magnitude.ToString("F2")}" +
            $"\n{(data.targetPosition.y - startPosition.position.y).ToString("F2")}" +
            $"\n{data.horizontalDistance.ToString("F2")}" +
            $"\n{data.timeToTarget.ToString("F2")}";
    }

}
