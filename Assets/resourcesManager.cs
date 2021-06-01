using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourcesManager : MonoBehaviour
{
    private int money = 0;
    public int MoneyMultiplier = 1;

    public int Money
    {
        get => money;
        set => money = value * MoneyMultiplier;
    }
}
