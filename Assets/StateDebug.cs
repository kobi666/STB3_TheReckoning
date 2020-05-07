using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateDebug : MonoBehaviour
{
    UnitController uc;
    TextMeshPro text;
    
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        uc = transform.parent.GetComponent<UnitController>() ?? null;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = uc.CurrentState.stateName.ToUpper();
    }
}
