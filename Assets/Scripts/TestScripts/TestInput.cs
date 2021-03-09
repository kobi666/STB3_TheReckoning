using System.Collections;
using UnityEngine;
using System;

public class TestInput : MonoBehaviour
{

    
    public float Distance = 1;
    public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

    public void PrintPositions(float distance) {
        float[] degrees = {0,60,120,180,240,300,360};
        foreach (var item in degrees)
        {
            //GameObject n = Instantiate(new GameObject, DegreeToVector2)
        }
    }

    float ff;
    public float FF {
        get=>ff;
        set {
            ff = value;
            if (value > 3) {
                ff = 0;
            }
        }
    }

    public float getFF() {
        return FF;
    }

    public IEnumerator testCoroutine(float fff) {
        //WeaponUtils.AngleFloat af = new WeaponUtils.AngleFloat(initialF);
        while (true) {
            fff += Time.deltaTime;
//            Debug.LogWarning(fff);
            yield return new WaitForFixedUpdate();
        }
    }
    public static TestInput instance;
    PlayerInput playerInput;
    // Start is called before the first frame update
    public event Action onW;
    public void OnW() {
        onW?.Invoke();
    }

    public event Action onD;
    public void OnD(){
        onD?.Invoke();
    }

    
    
    void Start()
    {
        

    }

    private void Awake() {
        playerInput = new PlayerInput();
        instance = this;
        PrintPositions(Distance);
        instance = this;
        FF += Time.deltaTime;
        playerInput.TestButtons.T.performed += ctx => OnW();
        playerInput.TestButtons.Y.performed += ctx => OnD();
        StartCoroutine(testCoroutine(FF));
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
