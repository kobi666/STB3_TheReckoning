using UnityEngine;
using System.Threading.Tasks;

public class ProjectileExitPoint : MonoBehaviour
{
    public GenericWeaponController ParentWeaponController;
    public float rotationSpeed = 1;
    public GenericUnitController Target
    {
        get => ParentWeaponController?.Target;
    }


    private void Start()
    {
        ParentWeaponController = GetComponentInParent<GenericWeaponController>();
    }

    bool AsyncRotationInProgress = false;

    public void StopAsyncRotation() {
        AsyncRotationInProgress = false;
    }
    public async void StartAsyncRotation() {
        if (AsyncRotationInProgress == true) {
            AsyncRotationInProgress = false;
            await Task.Yield();
        }
        AsyncRotationInProgress = true;
        while (AsyncRotationInProgress == true) {
            DefaultRotationFunction();
            await Task.Yield();
        }
        AsyncRotationInProgress = false;
    }

    private Vector2 cachecVectorToTargetPosition;
    public void DefaultRotationFunction()
    {
        cachecVectorToTargetPosition = Target?.transform.position - transform.position ?? cachecVectorToTargetPosition;
        float angleToTarget = Mathf.Atan2(cachecVectorToTargetPosition.y, cachecVectorToTargetPosition.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.DeltaGameTime * rotationSpeed);
    }
    
}