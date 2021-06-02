using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class resourcesManager : MonoBehaviour
{
    private int money = 0;
    public int MoneyMultiplier = 1;
    
    [ShowInInspector]
    public int Money
    {
        get => money;
        set => money = value * MoneyMultiplier;
    }
    
    private int playerLife = 20;
    public int PlayerLifeMultiplier = 1;
    [ShowInInspector]
    public int PlayerLife
    {
        get => playerLife;
        set => playerLife = value * MoneyMultiplier;
    }


}
