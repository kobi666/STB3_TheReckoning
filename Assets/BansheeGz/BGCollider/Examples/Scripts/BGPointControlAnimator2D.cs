using UnityEngine;
using BansheeGz.BGSpline.Curve;

namespace BansheeGz.BGSpline.Example
{
    //Animate Spline's point control (for demo scenes) 
    [RequireComponent(typeof(BGCurve))]
    public class BGPointControlAnimator2D : MonoBehaviour
    {
        //serializable
        public Vector2 From = new Vector2(8, 4);
        public Vector2 To = new Vector2(12, 2);
        public float CyclePeriod = 1;
        public int PointIndex;
        public bool AnimateSecondControl;

        //non serializable
        private BGCurve curve;
        private float cycleStarted = -1;
        private float halfCyclePeriod;

        void Start()
        {
            curve = GetComponent<BGCurve>();
            curve[PointIndex].ControlSecondLocal = From;
            halfCyclePeriod = CyclePeriod * .5f;
        }

        void FixedUpdate()
        {
            var currentTime = Time.time;
            if (cycleStarted < 0 || cycleStarted + CyclePeriod < currentTime) cycleStarted = currentTime;

            //calculate new pos
            var passedTime = currentTime - cycleStarted;
            var position = Vector2.Lerp(From, To, passedTime < halfCyclePeriod ? passedTime / halfCyclePeriod : (halfCyclePeriod * 2 - passedTime) / halfCyclePeriod);

            //assign new pos
            var point = curve[PointIndex];
            if (AnimateSecondControl) point.ControlSecondLocal = position;
            else point.ControlFirstLocal = position;
        }
    }
}