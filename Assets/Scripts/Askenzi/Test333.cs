using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Animal
{
    public string Name;
    public bool HasTail;
}

[Serializable]
public class Zebra : Animal
{
    public int amountOfStripes;
}

[Serializable]
public class WildZebra : Zebra
{
    public bool EatsHumans;
}
