using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewLootObject : MyGameObject
{
    
    public abstract Sprite ObjectSprite {get;}
    public abstract void OnLootObjectSelect(LootSelector lootSelector);

    

}
