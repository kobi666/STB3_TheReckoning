using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;
using System.Threading.Tasks;

public class test13 : MonoBehaviour
{
    private BGCurve BgCurve;
    public float posMoveRange = 3;
    public float posControlMoveRange = 3;
    private BGCurvePointI[] points;
    private Vector2[] initPointPositions;
    private Vector2[] initPointControls;
    // Start is called before the first frame update
    void Start()
    {
        BgCurve = GetComponent<BGCurve>();
        points = BgCurve.Points;
        initPointPositions = new Vector2[points.Length];
        initPointControls = new Vector2[points.Length];
        int index = 0;
        foreach (var VARIABLE in points)
        {
            initPointPositions[index] = VARIABLE.PositionLocal;
            index++;
        }

        index = 0;
        foreach (var VARIABLE in points)
        {
            initPointControls[index] = VARIABLE.ControlFirstLocal;
            index++;
        }

        StartCoroutine(oscilatePoint());
    }


    IEnumerator oscilatePoint()
    {
        int posDirection = -1;
        int controlDirection = -1;
        while (true)
        {
            Vector3 posMove = Vector2.right * (posDirection * Time.deltaTime);
            points[0].PositionLocal += (Vector3)posMove;
            Vector3 controlMove = Vector2.up * (controlDirection * Time.deltaTime);
            points[0].ControlFirstLocal += controlMove;
            
            
            if (points[0].PositionLocal.x >= initPointPositions[0].x + posMoveRange)
            {
                posDirection = -1;
            }

            if (points[0].PositionLocal.x <= initPointPositions[0].x - posMoveRange)
            {
                posDirection = 1;
            }

            if (points[0].ControlFirstLocal.y >= initPointControls[0].y + posControlMoveRange)
            {
                controlDirection = -1;
            }

            if (points[0].ControlFirstLocal.y <= initPointControls[0].y - posControlMoveRange)
            {
                controlDirection = 1;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    
    
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
