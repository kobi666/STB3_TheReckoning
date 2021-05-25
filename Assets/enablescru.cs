using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enablescru : MonoBehaviour
{
   private bool locker = false;
   public GameObject obj;
   public float counter;
   public float max;
   private void Update()
   {
      counter += Time.deltaTime;
      if (counter >= max)
      {
         if (!locker)
         {
            obj.SetActive(true);
            locker = true;
         }
      }
   }
}
