using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CameraBoundObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.ObserveEveryValueChanged(x => x).Subscribe(
            x => transform.position = x.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
