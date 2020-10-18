using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class test7 : SerializedMonoBehaviour
{
    [ValueDropdown("somelist")]
    public parent11 p11;
    
    
    public parent1 p1 = new parent1();
    public parent2 p2 = new parent2();
    private ValueDropdownList<parent11> somelist = new ValueDropdownList<parent11>()
    {
        {"item1", new child1()},
        {"item2", new child2()}
    };



}
