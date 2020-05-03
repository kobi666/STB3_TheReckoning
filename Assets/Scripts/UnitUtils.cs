using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitUtilities {
    public class UnitUtils : MonoBehaviour
    {
        // Start is called before the first frame update
        public static IEnumerator DestroyGameObject(GameObject _go) {
            Destroy(_go);
            yield break;
        }

        public static IEnumerator StopWalkingOnPath(BezierSolution.UnitWalker _walker) {
            _walker.StopWalking();
            yield break;
        }

        public static IEnumerator ReturnToWalkPath(BezierSolution.UnitWalker _walker) {
            _walker.ReturnWalking();
            yield break;
        }

        
    }
}