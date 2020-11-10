/* 
    <copyright file="BGCcCollider2DEdge" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    /// <summary>Fills Edge collider for 2D spline</summary>
    [HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCollider2DEdge")]
    [
        CcDescriptor(
            Description = "Create a set of Edge 2D colliders along 2D spline.",
            Name = "Collider 2D Edge",
            Icon = "BGCcCollider2DEdge123")
    ]
    [RequireComponent(typeof(EdgeCollider2D))]
    [AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCollider2DEdge")]
    public class BGCcCollider2DEdge : BGCcColliderAbstract<EdgeCollider2D>
    {
        //===============================================================================================
        //                                                    Editor stuff
        //===============================================================================================

        public override string Error
        {
            get { return ChoseMessage(base.Error, () => Curve.Mode2D != BGCurve.Mode2DEnum.XY ? "Curve should be in XY 2D mode" : null); }
        }

        //===============================================================================================
        //                                                    Fields (Persistent)
        //===============================================================================================


        //===============================================================================================
        //                                                    Fields (not persistent)
        //===============================================================================================

        public override bool RequireGameObjects
        {
            get { return false; }
        }

        protected override List<EdgeCollider2D> WorkingList
        {
            get { return null; }
        }

        protected override bool LocalSpace
        {
            get { return true; }
        }

        //===============================================================================================
        //                                                    Methods public
        //===============================================================================================
        public override void UpdateUi()
        {
            if (Curve.Mode2D != BGCurve.Mode2DEnum.XY) return;
            base.UpdateUi();
        }

        //===============================================================================================
        //                                                    Methods private
        //===============================================================================================

        protected override void FillSingleCollider(List<Vector3> positions, int count)
        {
            var collider = GetComponent<EdgeCollider2D>();
            if (collider == null) return;

            var points = Positions;
            var pointsCount = points.Count;

            var result = new Vector2[pointsCount];
            for (var i = 0; i < pointsCount; i++) result[i] = points[i];

            collider.points = result;
        }
    }
}