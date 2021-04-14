using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public static class ScriptExtensions 
{
   public static BGCurvePoint Clone(this BGCurvePointI pointToClone, BGCurve targetCurve, Vector3 positionDelta)
   {
      BGCurvePoint newPoint = new BGCurvePoint(targetCurve, pointToClone.PositionLocal + positionDelta);
      newPoint.ControlType = pointToClone.ControlType;
      if (newPoint.ControlType != BGCurvePoint.ControlTypeEnum.Absent)
      {
         newPoint.ControlFirstLocal = pointToClone.ControlFirstLocal;
         newPoint.ControlSecondLocal = pointToClone.ControlSecondLocal;
      }
      return newPoint;
   }
}
