using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    private EffectableTargetBank _effectableTargetBank;

    private void Start()
    {
        _effectableTargetBank = GetComponent<EffectableTargetBank>();
        _effectableTargetBank.onTargetAdd += delegate(Effectable effectable) { Debug.LogWarning(effectable.name + "ADD"); };
        _effectableTargetBank.onTargetRemove += delegate(string s) { Debug.LogWarning(s + "REMOVE"); };
    }
}
