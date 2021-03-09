using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class testSplineCon : MonoBehaviour
{
    // Start is called before the first frame update
    private BGCurve BgCurve;
    private BGCcMath math;

    private void Awake()
    {
        BgCurve = GetComponent<BGCurve>();
    }
}
