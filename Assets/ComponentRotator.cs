using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public class ComponentRotator : MonoBehaviour
{
    public float RotationSpeed;
    float angleForOrbit;
    public float AngleForOrbit {
        get => angleForOrbit;
        set {
            angleForOrbit = value;
            if (value > 360f) {
                angleForOrbit = 0.0f;
            }
            if (value < 0) {
                angleForOrbit = 360f;
            }
        }
    }

    public float AngleForRotation {
        get => angleForOrbit;
        set {
            angleForOrbit = value;
            if (value > 360f) {
                angleForOrbit = 0.0f;
            }
            if (value < 0) {
                angleForOrbit = 360f;
            }
        }
    }

    bool AsyncRotationInProgress = false;

    [Required] public GenericWeaponController ParentWeapon;

    public Transform TargetTransform
    {
        get => ParentWeapon.Target?.transform;
    }
    public void StopAsyncRotation() {
        AsyncRotationInProgress = false;
    }
    public async void StartAsyncRotationToweradsTarget() {
        if (AsyncRotationInProgress == true) {
            AsyncRotationInProgress = false;
            await Task.Yield();
        }
        AsyncRotationInProgress = true;
        Vector2 cachedPos = new Vector2();
        while (AsyncRotationInProgress == true) {
            cachedPos = TargetTransform?.position ?? cachedPos;
            Vector2 vecToTarget = cachedPos - (Vector2)transform.position;
            float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.DeltaGameTime * RotationSpeed);
            await Task.Yield();
        }
        AsyncRotationInProgress = false;
    }


    public virtual void DefaultRotationFunction(Vector2 TargetPosition) {
        Vector2 vecToTarget = TargetPosition - (Vector2)transform.position;
        float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.DeltaGameTime * RotationSpeed);
    }
    

    public Vector2 AngleToPosition(float angle) {
        return WeaponUtils.DegreeToVector2(angle);
    }

    public virtual void StartRotatingTowardsTarget() {
        StopRotating();
        RotationCoroutine = WeaponUtils.RotateTowardsTarget(transform, TargetTransform, RotationSpeed);
        StartCoroutine(RotationCoroutine);
    }
    
    public void  RotateTowardsTargetAsync(Transform self, Transform target, float rotationSpeed) {
        Vector2 cachedPos = new Vector2();
        while (true)
        {
            
        }
    }

    public virtual void StartIdleRotation() {
        RotationCoroutine = WeaponUtils.IdleRotation();
    }

    public virtual void StopRotating() {
        if (RotationCoroutine != null) {
            StopCoroutine(RotationCoroutine);
        }
        RotationCoroutine = null;
    }
    

    IEnumerator rotationCoroutine;

    public IEnumerator RotationCoroutine {get => rotationCoroutine; set {rotationCoroutine = value;}}
 
}
