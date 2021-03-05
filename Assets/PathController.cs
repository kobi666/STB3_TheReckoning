using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathController : MonoBehaviour
{
   public List<SplinePathController> ChildSplines = new List<SplinePathController>();


   protected void Start()
   {
      ChildSplines = GetComponentsInChildren<SplinePathController>().ToList();
      foreach (var spc in ChildSplines)
      {
         spc.parentPath = this;
      }
   }
}
