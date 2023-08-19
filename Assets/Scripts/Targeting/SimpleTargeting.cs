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
        throw new System.NotImplementedException();
    }

    public void Launch(LaunchData data)
    {
        //RotateArcher(NewTarget.targetPosition);
        //LaunchArrow(NewTarget.targetPosition);
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = transform.position.DirectionTo(target).With(y: transform.position.y);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Vector3 target)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);

        Vector3 dir = target - startPosition.position;
        newArrow.GetComponent<Rigidbody>().velocity = dir.normalized * power;
    }



}

