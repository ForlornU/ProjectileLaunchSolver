using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UIVisualizer : MonoBehaviour
{
    [SerializeField] int lineResolution = 30;
    [SerializeField] TMPro.TMP_Text dataToText;
    LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void WriteToUi(LaunchData data)
    {
        dataToText.text = $"{data.initialVelocity.magnitude.ToString("F2")}" +
$"\n{(data.targetPosition.y - data.initialPosition.y).ToString("F2")}" +
$"\n{data.horizontalDistance.ToString("F2")}" +
$"\n{data.timeToTarget.ToString("F2")}";
    }

    public void UpdateLine(LaunchData launchData)
    {
        lineRenderer.positionCount = lineResolution;
        Vector3 drawPoint = launchData.initialPosition;
        float debugHighestPoint = 0f;

        for (int i = 1; i <= lineResolution; i++)
        {
            float simulationTime = i / (float)lineResolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * launchData.gravity * simulationTime * simulationTime / 2f;
            drawPoint = launchData.initialPosition + displacement;

            lineRenderer.SetPosition(i - 1, drawPoint);

            if (drawPoint.y > debugHighestPoint)
                debugHighestPoint = drawPoint.y;
        }
    }

    public void StraightLine(Vector3 point1, Vector3 point2)
    {
        lineRenderer.SetPosition(0, point1);
        lineRenderer.SetPosition(1, point2);
    }
    public void StraightLine(Vector3 start, Vector3 direction, float magnitude)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + direction * magnitude);
    }
}
