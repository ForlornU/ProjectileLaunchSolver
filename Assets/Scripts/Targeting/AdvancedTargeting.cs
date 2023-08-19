using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AdvancedTargeting : MonoBehaviour, ArcherInterface
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
        PrintData(launchData);
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

        //Randomize Height, larger values yield higher arc, must never be below 0
        float height = Mathf.Abs(distanceY + Random.Range(0.5f, maxHeightRange));

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
        data.targetPosition = targetPosition;
        return data;
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target).With(y:transform.position.y);
        transform.rotation = Quaternion.LookRotation(dir);
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

    void PrintData(LaunchData data)
    {
        dataToText.text = $"Force: {data.initialVelocity.magnitude} \n Vertical Displacement: {Mathf.Abs(startPosition.position.y - data.targetPosition.y)} \n Time To Target: {data.timeToTarget}";
    }

    struct LaunchData
    {
        public Vector3 targetPosition;
        public Vector3 initialVelocity;
        public float timeToTarget;
        public float gravity;
    }
}
