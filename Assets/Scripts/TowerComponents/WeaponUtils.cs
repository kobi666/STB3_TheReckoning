using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponUtils 
{
    

    public static void StandardOnTargetDeathCheck(WeaponController self) {
        EnemyUnitController ec = self.EnemyTargetBank.FindSingleTargetNearestToEndOfSpline();
        if (ec != null) {
            self.Data.EnemyTarget = ec;
            self.OnAttackInitiate();
        }
    }

    

    public static IEnumerator TestAttack(WeaponController self, EnemyUnitController ec) {
        while (self.Target != null )  {
            while (ec?.IsTargetable() ?? false) {
                Debug.DrawLine(self.ProjectileExitPoint, ec.transform.position);   
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    

    

    public static IEnumerator RotateTowardsEnemyTargetUnit(Transform self, EnemyUnitController target, float rotationSpeed) {
        Transform targetTransform = target.transform;
        while (target?.IsTargetable() ?? false) {
            Vector2 vecToTarget = targetTransform.position - self.position;
            float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            self.rotation = Quaternion.Slerp(self.rotation, q, StaticObjects.instance.DeltaGameTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
    
    

    public static IEnumerator RotateTowardsTarget(Transform self, Transform target, float rotationSpeed) {
        while (true) {
            Vector2 vecToTarget = target.position - self.position;
            float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            self.rotation = Quaternion.Slerp(self.rotation, q, StaticObjects.instance.DeltaGameTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator IdleRotation() {
        yield break;
    }

    public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

    public static float[] orbRotationDegrees1 = {180f};

    public static float[] orbRotationDegrees2 = {0f,180f};
    public static float[] orbRotationDegrees3 = {0f,120f,240f};
    public static float[] orbRotationDegrees4 = {0f,180f,270f,90f};
    public static float[] orbRotationDegrees5 = {0f, 144f, 288f,72f,216f};
    public static float[] orbRotationDegrees6 = {0.0f,240f,120f,300f,180f,60f};

    public static float[] GetOrbRotationDegrees(int maxOrbs) {
        switch (maxOrbs) {
            case 1:
                return orbRotationDegrees1;
            case 2:
                return orbRotationDegrees2;
            case 3: 
                return orbRotationDegrees3;
            case 4:
                return orbRotationDegrees4;
            case 5: 
                return orbRotationDegrees5;
            case 6:
                return orbRotationDegrees6;
            default:
                Debug.LogWarning("undefined max number of orbs " + maxOrbs +  " , Defaulting to calculated degrees");
                return OrbRotationDegrees(maxOrbs);
        }
    }
    
    
    
    public static float[] OrbRotationDegrees(int maxOrbs) {
        float[] fa = new float[maxOrbs -1];
        float degreeDelta = 360 / maxOrbs;
        for (int i=1; i <= fa.Length+1 ; i++) {
            fa[i] = i * degreeDelta;
        }
        return fa;
    }
    
    
    
    
    
}
