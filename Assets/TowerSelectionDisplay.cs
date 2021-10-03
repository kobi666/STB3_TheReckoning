using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerSelectionDisplay : StaticDirectionalSlot<TowerSelectionDisplay>
{
    private SpriteRenderer SpriteRenderer;

    [FormerlySerializedAs("MyDirection")] public ButtonDirectionsNames MyDirectionalButtonName;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        /*SpriteRenderer.sprite = GameManager.Instance.PlayerTowersManager.GetTowerByButtonName(MyDirectionalButtonName)
            ?.TowerSprite;*/
        GameManager.Instance.PlayerTowersManager.GetTowerReactivePropertyByButtonName(MyDirectionalButtonName)?
            .Subscribe(x  => SpriteRenderer.sprite = x.TowerSprite);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsSlotAvailable()
    {
        return true;
    }
}
