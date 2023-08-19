using UnityEngine;

public class PointTargeting : MonoBehaviour
{
    ArcherInterface Archer;

    [SerializeField]
    Transform target;

    LaunchData storedLaunchData;

    private void Start()
    {
        Archer = GetComponent<ArcherInterface>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Predict();
        if(Input.GetMouseButtonDown(1))
            ShootArrow();
    }

    void Predict()
    {
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
