using UnityEngine;

public class SimpleTargeting : MonoBehaviour, ArcherInterface
{
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    float power = 15f;

    public void Launch(TargetData NewTarget)
    {
        RotateArcher(NewTarget.targetPosition);
        LaunchArrow(NewTarget.targetPosition);
    }

    void RotateArcher(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void LaunchArrow(Vector3 target)
    {
        GameObject newArrow = Instantiate(arrow, startPosition.position, transform.rotation);

        Vector3 dir = target - startPosition.position;
        newArrow.GetComponent<Rigidbody>().velocity = dir.normalized * power;
    }
}

