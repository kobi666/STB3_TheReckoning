using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorUtils 
{
    public static IEnumerator SmoothMove(Transform selector, Vector2 targetPosition, float movementDuration) {
        float startTime = Time.time;
        float t;
        while((Vector2)selector.position != targetPosition) {
            t = (Time.time - startTime) / movementDuration;
            selector.position = new Vector2(Mathf.SmoothStep(selector.position.x, targetPosition.x, t),Mathf.SmoothStep(selector.position.y, targetPosition.y, t));
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }


}
