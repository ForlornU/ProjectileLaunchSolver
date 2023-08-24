using UnityEngine;

public class SimpleTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    float power = 15f;

    public LaunchData Calculate(TargetData targetData)
    {
        RotateArcher(targetData.targetPosition);
        LaunchArrow(targetData.targetPosition);

        //This data is not used, but required. This class is too simple to have any use for it
        LaunchData fauxData = new LaunchData();
        return fauxData;
    }

    public void Launch(LaunchData data)
    {
        Debug.Log("Left click to fire a simple arrow");
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target.With(y: transform.position.y));
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Vector3 target)
    {
        Vector3 dir = startPosition.position.DirectionTo(target);

        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);
        newArrow.GetComponent<Rigidbody>().velocity = dir.normalized * power;
    }
}

