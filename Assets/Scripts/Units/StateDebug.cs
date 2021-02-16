using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateDebug : MonoBehaviour
{
    UnitController uc;
    TextMeshPro text;
    
    void ChangeStateTextAndColor() {
        text.text = uc.CurrentStateLegacy.stateName.ToUpper();
        text.color = uc.CurrentStateLegacy.textColor;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        uc = transform.parent.GetComponent<UnitController>() ?? null;
        // ChangeStateTextAndColor();
        //uc.SM.onStateChange += ChangeStateTextAndColor;
    }

    private void Update() {
        ChangeStateTextAndColor();
        if (uc.dataLegacy.EnemyTarget != null) {
        Debug.DrawLine(uc.transform.position, uc.dataLegacy.EnemyTarget.transform.position);
        }
    }
}
