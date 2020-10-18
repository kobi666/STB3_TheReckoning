using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class parent1 : testInterface
{
    [ShowInInspector]
    public int myInt { get; set; }
    
    [ShowInInspector]
    public float myFloat { get; set; }
}

[System.Serializable]
public class parent2 : testInterface
{
    [ShowInInspector]
    [LabelText(" label 11")]
    public int myInt { get; set; }
    
    [ShowInInspector]
    [LabelText(" label 22")]
    public float myFloat { get; set; }
}

[System.Serializable]
public abstract class parent11
{
    public abstract float myFloat { get; set; }
    public abstract int myInt { get; set; }
}


[System.Serializable]
public class child1 : parent11
{
    [ShowInInspector]
    public override float myFloat { get; set; }
    [ShowInInspector]
    public override int myInt { get; set; }
}

[System.Serializable]
public class child2 : parent11
{
    [ShowInInspector]
    [LabelText("new label 1")]
    public override float myFloat { get; set; }
    [ShowInInspector]
    [LabelText("new label 2")]
    public override int myInt { get; set; }
}
