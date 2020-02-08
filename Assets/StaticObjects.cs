using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public static Transform PPH;
    public StaticObjects instance;
    void Awake()
    {
        instance = this;
    }

    private void Start() {
        PPH = GameObject.FindGameObjectWithTag("PPH").transform;
    }

    // Update is called once per frame
    
}
