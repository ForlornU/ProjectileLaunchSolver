using UnityEngine;

public class PointTargeting : MonoBehaviour
{
    [SerializeField]
    GameObject Archer;

    [SerializeField]
    Transform target;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnClick();
    }

    void OnClick()
    {
        Archer.GetComponent<ArcherInterface>().Launch(UpdateTarget());
    }

    TargetData UpdateTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
            target.transform.position = hit.point;

        return new TargetData(hit.point, hit.transform, 0f);
    }
}
