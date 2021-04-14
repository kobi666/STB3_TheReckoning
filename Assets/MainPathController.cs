using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using Sirenix.OdinInspector;
using UnityEngine;

public class MainPathController : SplinePathController
{
    [Required]
    public CustomBGCBoxCollider CustomBgcBoxCollider;

    [Required]
    public SeconderyPathController SeconderyPathControllerPrefab;

    public NumberOfPaths NumberOfPaths;
    public float singlePathWidth = 0.5f;
    public List<SplinePathController> SplinePaths = new List<SplinePathController>();
    
    [ShowInInspector]
    private float TotalPathWidth
    {
        get => (float)NumberOfPaths * singlePathWidth;
    }

    protected void Awake()
    {
        base.Awake();
        SplinePaths.Add(this);
        CustomBgcBoxCollider = GetComponent<CustomBGCBoxCollider>();
        InitPath();
    }
    
    
    public void InitPath()
    {
        if (NumberOfPaths != NumberOfPaths.One) {
            foreach (var pathPositionDelta in GetPathPositionDelta())
            {
                SplinePaths.Add(CreateSeconderyPath(pathPositionDelta));
            }
        }
        
    }

    public float[] GetPathPositionDelta()
    {
        float[] floatArray = null;

        if (NumberOfPaths == NumberOfPaths.Three)
        {
            floatArray = new float[2]
            {
                singlePathWidth, -singlePathWidth
            };
        }

        if (NumberOfPaths == NumberOfPaths.Five)
        {
            floatArray = new float[4]
            {
                singlePathWidth, -singlePathWidth, singlePathWidth * 2f, -singlePathWidth * 2f
            };
        }
        return floatArray;
    }

    SeconderyPathController CreateSeconderyPath(float pathPositionDelta)
    {
        SeconderyPathController seconderyPathController = Instantiate(SeconderyPathControllerPrefab, transform);
        seconderyPathController.transform.position = transform.position + new Vector3(0, pathPositionDelta, 0);
        for (int i = 0; i < BgCurve.Points.Length; i++)
        {
            seconderyPathController.BgCurve.AddPoint(BgCurve.Points[i]
                .Clone(seconderyPathController.BgCurve, new Vector3(0, pathPositionDelta, 0)));
        }

        return seconderyPathController;
    }

}


public enum NumberOfPaths
{
    One = 1,
    Three = 3,
    Five = 5
}
