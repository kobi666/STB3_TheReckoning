using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class pathGeneratorTest : MonoBehaviour
{
    public BezierSpline BaseSpline;
    public List<BezierSpline> Splines = new List<BezierSpline>();
    public float HeightDifferenceBetweenSplines = 0.10f;


    void CreateUpperAndLowerSplines()
    {
        Splines.Add(BaseSpline);
        Splines.Add(Instantiate(BaseSpline));
        Splines[1].transform.parent = transform;
        Splines[1].transform.position =
            BaseSpline.transform.position + new Vector3(0, HeightDifferenceBetweenSplines, 0);
        Splines.Add(Instantiate(BaseSpline));
        Splines[2].transform.parent = transform;
        Splines[2].transform.position =
            BaseSpline.transform.position - new Vector3(0, HeightDifferenceBetweenSplines, 0);
    }
    private void Awake()
    {
        CreateUpperAndLowerSplines();
    }
}
