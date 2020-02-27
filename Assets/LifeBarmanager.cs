using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeBarmanager : MonoBehaviour
{
    SpriteRenderer ParentSpriteRenderer;
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
        ParentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        initialX = LifeBarMaskPositon.position.x;
        maxLife = gameObject.GetComponentInParent<PlayerUnitController2>().Data.HP;
        Life = gameObject.GetComponentInParent<PlayerUnitController2>().LifeManager;
        Life.damageTaken += UpdateLifeBar;
        LifeBarSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
        ResizeLifeBarToUnitSpriteSize();
    }

    public void ResizeLifeBarToUnitSpriteSize() {
        float ParentSpriteSize = ParentSpriteRenderer.sprite.bounds.size.x * transform.parent.transform.localScale.x;
        //Debug.Log("Parent Bound size " + ParentSpriteRenderer.sprite.bounds.size.x);
        //Debug.Log("LIFE Bound size " + GetComponent<SpriteRenderer>().sprite.bounds.size.x);
        float ratio =   ParentSpriteSize / LifeBarSize;
        Vector2 newScale = new Vector2(ratio, transform.localScale.y);
        gameObject.transform.localScale = newScale;
    }



    public void UpdateLifeBar(int damageTaken) {
        float hitPercentageOfLife = ((float)damageTaken / (float)maxLife);
        float amountToMove = (LifeBarSize * hitPercentageOfLife);
        LifeBarMaskPositon.Translate(Vector2.left * amountToMove);    
    }

    private void Update() {
       
    }

    // Update is called once per frame
    
}
