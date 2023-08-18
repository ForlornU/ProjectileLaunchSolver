using UnityEngine;

public struct TargetData
{
    public Transform targetObject;
    public Vector3 targetPosition;
    public float dragCoefficient;

    public TargetData(Vector3 mouseClickPosition, Transform target, float drag)
    {
        targetObject = target;
        targetPosition = mouseClickPosition;
        dragCoefficient = drag;
    }
}
