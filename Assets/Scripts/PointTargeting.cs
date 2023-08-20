using UnityEngine;

public class PointTargeting : MonoBehaviour
{
    ArcherInterface Archer;
    LaunchData storedLaunchData;

    [SerializeField] Transform target;
    float targetingDelay = 0.15f;
    [SerializeField, Range(0.06f, 2f)] float maxDelay = 0.15f;

    private void Start()
    {
        Archer = GetComponent<ArcherInterface>();
        targetingDelay = maxDelay;
    }

    void Update()
    {
        if(Input.GetMouseButton(0) && targetingDelay == 0)
        {
            Predict();
        }
        else if(Input.GetMouseButtonDown(1))
            ShootArrow();

        Cooldown();
    }

    void Cooldown()
    {
        targetingDelay = Mathf.Clamp(targetingDelay -= Time.deltaTime, 0f, maxDelay);
    }

    void Predict()
    {
        targetingDelay = maxDelay;

        LaunchData newData = Archer.Calculate(UpdateTarget());
        if (newData.horizontalDistance < 5f)
        {
            Debug.Log("Too close!");
            return;
        }
        storedLaunchData = newData;
    }

    void ShootArrow()
    {
        if (storedLaunchData.timeToTarget < 0.1f) //Trying to check if null
            return;
        Archer.Launch(storedLaunchData);
    }

    TargetData UpdateTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            target.transform.position = hit.point;

        return new TargetData(hit.point, hit.transform, 0f);
    }
}
