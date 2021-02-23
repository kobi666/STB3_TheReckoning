using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ofirtest2 : MonoBehaviour
{

    public Animal Animal222;
    public Zebra Zebra333;
    public WildZebra WildZebra444;

    public void printAnimalStats(Animal Animalparameter)
    {
        WildZebra stamzebra = Animalparameter as WildZebra;
        Debug.LogWarning(Animalparameter.Name + " " + Animalparameter.HasTail);
        
    }

    public void printZebraStats(Zebra zebraParameter)
    {
        
    }
    
    private void Start()
    {
        //printAnimalStats(WildZebra444);
    }
}
