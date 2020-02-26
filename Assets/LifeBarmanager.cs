using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeBarmanager : MonoBehaviour
{
    int maxLife;
    float initialX;
    UnitLifeManager Life;
    
    public SpriteMask LifeBarMask {
        get => GetComponentInChildren<SpriteMask>();
    }
    public Transform LifeBarMaskPositon {
        get => LifeBarMask.gameObject.transform;
    }
    float LifeBarSize;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Child Started");
        initialX = LifeBarMaskPositon.position.x;
        maxLife = gameObject.GetComponentInParent<PlayerUnitController2>().Data.HP;
        Life = gameObject.GetComponentInParent<PlayerUnitController2>().LifeManager;
        Life.damageTaken += UpdateLifeBar;
        LifeBarSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
        Debug.Log(gameObject.GetComponentInParent<PlayerUnitController2>().Data.HP);
    }

    public void UpdateLifeBar(int damageTaken) {
        float hitPercentageOfLife = ((float)damageTaken / (float)maxLife);
        float amountToMove = (LifeBarSize * hitPercentageOfLife);
        Debug.Log(LifeBarMaskPositon.position.x - amountToMove);
        LifeBarMaskPositon.Translate(Vector2.left * amountToMove);    
    }

    // Update is called once per frame
    
}
