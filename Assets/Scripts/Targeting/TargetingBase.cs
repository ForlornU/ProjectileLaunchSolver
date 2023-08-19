using UnityEngine;

public abstract class TargetingBase : MonoBehaviour
{
    protected virtual void RotateArcher(Vector3 target)
    {
        Vector3 targetPosFlat = target.With(y: transform.position.y);
        Vector3 direction = transform.position.DirectionTo(targetPosFlat);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
