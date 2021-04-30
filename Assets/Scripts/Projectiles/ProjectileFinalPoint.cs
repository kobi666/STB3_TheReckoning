using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

public class ProjectileFinalPoint : MonoBehaviour
{
    private GenericWeaponController ParentWeaponController;
    public float rotationSpeed = 1;
    public Transform PositionTransform;
    [HideInInspector] public float InitialPositionX;
    [HideInInspector] public Vector2 InitialPosition;
    

    
    public TargetUnit Target
    {
        get => ParentWeaponController?.Target;
    }
    
    [Required]
    public CollisionDetector RangeDetector;
    public EffectableTargetBank EffectableTargetBank;
    protected void Awake()
    {
        InitialPositionX = transform.position.x;
        InitialPosition = transform.position;
        EffectableTargetBank = GetComponent<EffectableTargetBank>() ?? null;
    }

    public void ReturnToInitialPosition()
    {
        transform.position = InitialPosition;
    }

    public void UpdatePositionAccordingToRadius(float radius)
    {
        transform.position = new Vector2(transform.position.x + radius, transform.position.y);
    }
    
    protected void Start() {
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
    
    public void DefaultRotationFunction() {
        Vector2 vecToTarget = Target.transform.position - transform.position;
        float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.DeltaGameTime * rotationSpeed);
    }

}
