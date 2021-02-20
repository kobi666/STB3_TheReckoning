using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testwalker : MonoBehaviour
{
   private PathWalker pw;

   private void Start()
   {
      pw = GetComponent<PathWalker>();
      pw.CurrentDistanceOnSpline = 0;
   }

   private void Update()
   {
      pw.OnPathMovement(Time.deltaTime / 3);
   }
}
