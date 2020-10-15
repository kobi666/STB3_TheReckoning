using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SpecificReturnTypes
{
   public static List<Type> SupportedReturnTypes = new List<Type>()
   {
      typeof(int),
      typeof(string),
      typeof(IEnumerator),
      typeof(LineRenderer),
      typeof(int[])
   };
   
   
}
