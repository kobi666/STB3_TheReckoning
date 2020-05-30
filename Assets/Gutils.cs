﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gutils 
{
    // Start is called before the first frame update
    public static IEnumerator OrbitAroundTransformNoRotation(Transform self, Transform basePosTransform, float rotationSpeed, Vector3 rotationDirection) {
        Quaternion zero = new Quaternion(0,0,0,0);
        while (true) {
            self.RotateAround(basePosTransform.position, rotationDirection, rotationSpeed * StaticObjects.instance.DeltaGameTime);
            self.rotation = zero;
        }
    }

    public static IEnumerator OrbitAroundTransform(Transform self, Transform basePosTransform, float rotationSpeed, Vector3 rotationDirection, Transform rotationAngleTransform) {
        while (true) {
            self.RotateAround(basePosTransform.position, rotationDirection, rotationSpeed * StaticObjects.instance.DeltaGameTime);
            self.rotation = rotationAngleTransform.rotation;
        }
    }
}
