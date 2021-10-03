using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;


[Serializable]
public class NewLootSlot : StaticDirectionalSlot<NewLootSlot>
{
    [Required]
    public LootItemManager ParentLootItemManager;
    
    public ReactiveProperty<NewLootObject> LootObject;

    public override bool IsSlotAvailable()
    {
        return (LootObject != null);
    }

    public SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LootObject.Subscribe(_ => SpriteRenderer.sprite = _.ObjectSprite);
    }
}



