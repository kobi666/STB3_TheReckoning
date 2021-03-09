using UnityEngine;

public class TestScript : MonoBehaviour
{
    
    
    
    public GameObject TestTarget;
    public Vector2 TargetPosition;
    public Vector2 InitPos;
    public float arcValue;
    public Vector2 initToMiddle;
    public Vector2 MiddleToTarget;
    public float speed;
    [SerializeField] private float _counter;
    public float Counter
    {
        get => _counter;
        set
        {
            if (value > 1.0f)
            {
                _counter = 0;
            }
            else
            {
                _counter = value;
            }
        }
        
    }

    

    private void Start()
    {
        
    }

    private void Update()
    {
        /*ProjectileUtils.MoveInArcToPosition(transform, InitPos, MiddlePos(), TargetPosition, ref initToMiddle,
            ref MiddleToTarget, speed, ref _counter );*/

        
    }
}
