
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AdvancedTargeting : TargetingBase, ArcherInterface
{
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    GameObject arrow;
    LineRenderer lineRenderer;

    [Tooltip("Resolution/Detail-level of the line-renderer component")]
    public int lineResolution = 30;
    public float maxHeightRange = 5f;
    public TMPro.TMP_Text dataToText;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Launch(TargetData newTarget)
    {
        RotateArcher(newTarget.targetPosition);
        LaunchData launchData = CalculateLaunch(newTarget.targetPosition);
        DrawPath(launchData);
        LaunchArrow(launchData.initialVelocity);
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

        //Vertical distance between target and start position
        float distanceY = targetPosition.y - startPosition.position.y;

        float height = RandomizeHeight(targetPosition.y, distanceY);

        //Horizontal distance, points toward the target
        Vector3 horizontalDirection = startPosition.position.DirectionTo(targetPosition).With(y: 0f);

        //Time our path will take
        float time = Mathf.Sqrt(-2 * height / data.gravity) + Mathf.Sqrt(2 * (distanceY - height) / data.gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * data.gravity * height);
        Vector3 velocityXZ = horizontalDirection * (1 / time);
        Vector3 velocityFinal = velocityY + velocityXZ;
        velocityFinal *= -Mathf.Sign(data.gravity);

        data.initialVelocity = velocityFinal;
        data.timeToTarget = time;
        return data;
    }

    float RandomizeHeight(float targetHeight, float distance)
    {
        //Height must never be < 0
        float newHeight = Mathf.Abs(distance + Random.Range(0.25f, maxHeightRange));

        dataToText.text = $"Starting height: {startPosition.position.y} \ntarget height: {targetHeight} \nVertical Difference: {Mathf.Abs(distance)} \nNew Height: {newHeight}";

        return newHeight;
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

        Debug.Log("Highest reached point on arc : " + debugHighestPoint);
    }

    struct LaunchData
    {
        public Vector3 initialVelocity;
        public float timeToTarget;
        public float gravity;
    }
}
