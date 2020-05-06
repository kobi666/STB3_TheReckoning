using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleePlayerUnit : PlayerUnitController
{
    
    public override IEnumerator OnEnterPreBattle() {
        //Data.EnemyTarget.SM.SetState()
        yield break;
    }

    public override IEnumerator OnExitPreBattle() {
        yield break;
    }

    public override IEnumerator OnEnterDefault() {
        yield break;
    }

    public override IEnumerator OnExitDefault() {
        yield break;
    }

    public override IEnumerator OnEnterInBattle() {
        yield break;
    }

    public override IEnumerator OnExitInBattle() {
        yield break;
    }

    public override IEnumerator OnEnterPostBattle() {
        yield break;
    }

    public override IEnumerator OnExitPostBattle() {
        yield break;
    }

    
    // Start is called before the first frame update
    public override void LateStart() {

    }
}
