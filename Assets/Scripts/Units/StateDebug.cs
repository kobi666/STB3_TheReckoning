using UnityEngine;
using TMPro;

public class StateDebug : MonoBehaviour
{
    UnitController uc;
    private GenericUnitController gnc;
    TextMeshPro text;
    
    void ChangeStateTextAndColor()
    {
        text.text = gnc.StateMachine.CurrentState.StateName;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        gnc = transform.parent.GetComponent<GenericUnitController>() ?? null;
        // ChangeStateTextAndColor();
        //uc.SM.onStateChange += ChangeStateTextAndColor;
    }

    private void Update() {
        ChangeStateTextAndColor();
        if (gnc.UnitBattleManager.targetExists) {
        Debug.DrawLine(gnc.transform.position, gnc.UnitBattleManager.TargetUnit.transform.position);
        }
    }
}
