using UnityEngine;

public struct TargetData
{
    public Transform targetObject;
    public Vector3 targetPosition;

    public TargetData(Vector3 mouseClickPosition, Transform target)
    {
        targetObject = target;
        targetPosition = mouseClickPosition;
    }
}
