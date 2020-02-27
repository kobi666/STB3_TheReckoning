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
        
        initialX = LifeBarMaskPositon.position.x;
        maxLife = gameObject.GetComponentInParent<PlayerUnitController2>().Data.HP;
        Life = gameObject.GetComponentInParent<PlayerUnitController2>().LifeManager;
        Life.damageTaken += UpdateLifeBar;
        LifeBarSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
        //ResizeLifeBarToUnitSpriteSize();
    }

    public void ResizeLifeBarToUnitSpriteSize() {
        float ParentSpriteSize = GetComponentInParent<SpriteRenderer>().sprite.bounds.size.x * GetComponentInParent<Transform>().localScale.x;
        float ratio = LifeBarSize / ParentSpriteSize;
        Vector2 newScale = new Vector2(ratio, transform.localScale.y);
        gameObject.transform.localScale = newScale;
    }



    public void UpdateLifeBar(int damageTaken) {
        float hitPercentageOfLife = ((float)damageTaken / (float)maxLife);
        float amountToMove = (LifeBarSize * hitPercentageOfLife);
        LifeBarMaskPositon.Translate(Vector2.left * amountToMove);    
    }

    // Update is called once per frame
    
}
