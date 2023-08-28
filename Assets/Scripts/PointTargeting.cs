using UnityEngine;

public class PointTargeting : MonoBehaviour
{
    ArcherInterface Archer;
    LaunchData storedLaunchData;

    [SerializeField] Transform target;
    [SerializeField, Range(4f, 10f)] float minDistance = 5f;
    [SerializeField, Range(50f, 1000f)] float maxDistance = 200f;
    [SerializeField, Range(0.06f, 2f)] float maxDelay = 0.15f;

    float targetingDelay = 0.15f;

    private void Start()
    {
        Archer = GetComponent<ArcherInterface>();
        targetingDelay = maxDelay;
    }

    void Update()
    {
        if (targetingDelay != 0)
        {
            Cooldown();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Predict();
            targetingDelay = maxDelay;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Archer.Launch(storedLaunchData);
            targetingDelay = maxDelay;
        }
    }

    void Cooldown()
    {
        targetingDelay = Mathf.Clamp(targetingDelay -= Time.deltaTime, 0f, maxDelay);
    }

    void Predict()
    {
        LaunchData newData;

        if (UpdateTarget(out TargetData td))
        {
            target.position = td.targetPosition;
            newData = Archer.Calculate(td);
            storedLaunchData = newData;
        }
    }

    bool UpdateTarget(out TargetData td)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool validTarget = Physics.Raycast(ray, out RaycastHit hit, maxDistance);
        if (Vector3.Distance(hit.point, transform.position) < minDistance)
            validTarget = false;

        td = new TargetData(hit.point, hit.transform);
        return validTarget;
    }
    
}
