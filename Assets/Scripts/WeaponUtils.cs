using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponUtils 
{

    public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

    float[] orbRotationDegrees3 = {0f,120f,240f};
    float[] orbRotationDegrees4 = {0f,180f,270f,90f};
    float[] orbRotationDegrees5 = {0f, 144f, 288f,72f,216f};
    float[] orbRotationDegrees6 = {0.0f,240f,120f,300f,180f,60f};

    public static float AngleFloat (float f)
        float
        
        

    public static IEnumerator OrbitGunAround(OrbitalWeapon self, Transform orbitBase, float angle, float distanceFromBase) {
        while (true) {
            //self.transform.position = 
            //yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    
    
    
    public static float[] OrbRotationDegrees(int maxOrbs) {
        float[] fa = new float[maxOrbs -1];
        float degreeDelta = 360 / maxOrbs;
        for (int i=1; i <= fa.Length+1 ; i++) {
            fa[i] = i * degreeDelta;
        }
        return fa;
    }
    
    
    public static void StartOrbitalsRotation(WeaponController[] orbitalGuns, Transform rootTower) {
        
        
    }

    public static IEnumerator RotateOrbital(Transform orbitalGun) {
        
        yield break;
    }
    
}
