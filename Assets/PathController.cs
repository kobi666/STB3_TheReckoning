using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class PathController : MonoBehaviour
{
   public List<SplinePathController> ChildSplines = new List<SplinePathController>();

   public event Action onPathUpdate;

   public void OnPathUpdate()
   {
      onPathUpdate?.Invoke();
   }

   public event Action onPathSplinesFound;
   protected void Start()
   {
      ChildSplines = GetComponentsInChildren<SplinePathController>().ToList();
      foreach (var spc in ChildSplines)
      {
         spc.parentPath = this;
      }

      if (!ChildSplines.IsNullOrEmpty())
      {
         onPathSplinesFound?.Invoke();
      }
      
   }
}
