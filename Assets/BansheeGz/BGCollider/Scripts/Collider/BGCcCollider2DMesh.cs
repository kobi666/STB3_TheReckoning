/* 
    <copyright file="BGCcCollider2DMesh" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	/// <summary>Fill in Polygon Collider 2D inside 2D spline</summary>
	[HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCollider2DMesh")]
	[
		BGCc.CcDescriptor(
			Description = "Create a mesh collider inside 2D spline.",
			Name = "Collider 2D Mesh",
			Icon = "BGCcCollider2DMesh123")
	]
	[RequireComponent(typeof(PolygonCollider2D))]
	[AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCollider2DMesh")]
	public class BGCcCollider2DMesh : BGCcColliderMeshAbstract<PolygonCollider2D>
	{
		private BGTriangulator2D triangulator;

		protected override void Build(PolygonCollider2D collider, List<Vector3> positions)
		{
			collider.points = to2DArray(positions);

			if (IsMeshGenerationOn)
			{
				var renderer = Get<MeshRenderer>();
				var filter = Get<MeshFilter>();
				var filterMesh = filter.sharedMesh;
				if (filterMesh == null) filterMesh = new Mesh();
				if (triangulator == null) triangulator = new BGTriangulator2D();
				triangulator.Bind(filterMesh, positions, new BGTriangulator2D.Config
				{
					Closed = Curve.Closed,
					Mode2D = Curve.Mode2D,
					Flip = false,
					ScaleUV = Vector2.one,
					OffsetUV = Vector2.zero,
					DoubleSided = false,
					ScaleBackUV = Vector2.one,
					OffsetBackUV = Vector2.zero,
				});
                
				filter.sharedMesh = filterMesh;
			}
		}

		private Vector2[] to2DArray(List<Vector3> positions)
		{
			var result = new Vector2[positions.Count];
			switch (Curve.Mode2D)
			{
				case BGCurve.Mode2DEnum.Off:
					throw new ArgumentOutOfRangeException("2D Mode for a curve should be on");
				case BGCurve.Mode2DEnum.XY:
					for (var i = 0; i < positions.Count; i++)
					{
						var position = positions[i];
						result[i]=new Vector2(position.x, position.y);
					}

					break;
				case BGCurve.Mode2DEnum.XZ:
					for (var i = 0; i < positions.Count; i++)
					{
						var position = positions[i];
						result[i]=new Vector2(position.x, position.z);
					}
					break;
				case BGCurve.Mode2DEnum.YZ:
					for (var i = 0; i < positions.Count; i++)
					{
						var position = positions[i];
						result[i]=new Vector2(position.y, position.z);
					}

					break;
				default:
					throw new ArgumentOutOfRangeException("Curve.Mode2D");
			}

			return result;
		}
	}
}