using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DegreeUtils
{
    public static class Extnesions
    {
        public static Vector2?[] GetSpreadPositions(this Object _object, int numOfObjects, Vector2? basePosition, float distanceFromBasePosition)
        {
            Vector2?[] v2Array = new Vector2?[numOfObjects];
            float DegreeIncrement = 360f / (float) numOfObjects;
            
            if (numOfObjects <= 1)
            {
                v2Array[0] = basePosition;
            }
            else {
                if (basePosition != null) {
                    for (int i = 0; i < v2Array.Length; i++)
                    {
                        float d = (float)i * DegreeIncrement;
                        v2Array[i] = basePosition + (DegreeToVector2(d) * distanceFromBasePosition );
                    }
                }
            }
            return v2Array;
        }
        
        public static Vector2 RadianToVector2(float radian)
        {
            Vector2 v2 = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            return v2;
        }
        public static Vector2 DegreeToVector2(float degree)
        {
            Vector2 v2 = RadianToVector2(degree * Mathf.Deg2Rad);
            return v2;
        }
        
        
    }
}

