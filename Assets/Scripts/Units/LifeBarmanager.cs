﻿using UnityEngine;

public class LifeBarmanager : MonoBehaviour
{
    private GenericUnitController GenericUnitController;
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
        GenericUnitController = transform.parent.GetComponent<GenericUnitController>();
        ParentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        initialX = LifeBarMaskPositon.position.x;
        maxLife = transform.parent.GetComponent<GenericUnitController>().Data.MetaData.HP;
        Life = transform.parent.GetComponent<GenericUnitController>()?.UnitLifeManager ?? null;
        if (Life != null) {
            Life.damageTaken += UpdateLifeBar; 
        }
        LifeBarSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        ResizeLifeBarToUnitSpriteSize();
    }

    public void ResizeLifeBarToUnitSpriteSize() {
        if (ParentSpriteRenderer.sprite != null) {
        float ParentSpriteSize = ParentSpriteRenderer.sprite.bounds.size.x;
        //Debug.Log("Parent Bound size " + ParentSpriteRenderer.sprite.bounds.size.x);
        //Debug.Log("LIFE Bound size " + GetComponent<SpriteRenderer>().sprite.bounds.size.x);
        float ratio = ParentSpriteSize / LifeBarSize;
        Vector2 newScale = new Vector2(ratio, transform.localScale.y);
        gameObject.transform.localScale = newScale;
        LifeBarSize = LifeBarSize * transform.localScale.x;
        }
    }



    public void UpdateLifeBar(int damageTaken) {
        float hitPercentageOfLife = ((float)damageTaken / (float)maxLife);
        float amountToMove = (LifeBarSize * hitPercentageOfLife);
        LifeBarMaskPositon.Translate(Vector2.left * amountToMove);    
    }

    // Update is called once per frame
    
}
