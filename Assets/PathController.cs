using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class PathController : MonoBehaviour
{
   public SortedList<SplineTypes,SplinePathController> ChildSplines = new SortedList<SplineTypes,SplinePathController>();

   public event Action onPathUpdate;

   public void OnPathUpdate()
   {
      onPathUpdate?.Invoke();
   }

   public event Action onPathSplinesFound;
   protected void Start()
   {
      var _ChildSplines = GetComponentsInChildren<SplinePathController>();
      foreach (var spc in _ChildSplines)
      {
         //spc.parentPath = this;
         ChildSplines.Add(spc.SplineType, spc);
      }

      if (!ChildSplines.IsNullOrEmpty())
      {
         onPathSplinesFound?.Invoke();
      }
      
   }
}
