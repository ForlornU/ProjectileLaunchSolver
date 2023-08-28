using UnityEngine;

public class SimpleTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform startPosition;
    [SerializeField] float power = 15f;

    UIVisualizer ui;

    private void Start()
    {
        ui = GetComponent<UIVisualizer>();
    }

    public LaunchData Calculate(TargetData targetData)
    {
        RotateArcher(targetData.targetPosition);

        LaunchData fauxData = new LaunchData();
        fauxData.initialVelocity = startPosition.position.DirectionToNormalized(targetData.targetPosition) * power;
        fauxData.initialPosition = startPosition.position;
        ui.StraightLine(fauxData.initialPosition, fauxData.initialVelocity);
        return fauxData;
    }

    public void Launch(LaunchData data)
    {
        LaunchArrow(data.initialVelocity);
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target.With(y: transform.position.y));
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Vector3 velocity)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = velocity.normalized * power;
    }
}

