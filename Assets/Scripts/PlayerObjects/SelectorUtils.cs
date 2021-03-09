using System;
using System.Collections;
using UnityEngine;

public class SelectorUtils 
{
    public static IEnumerator SmoothMove(Transform selector, Vector2 targetPosition, float movementDuration, Action beginAction, Action endAction) {
        float startTime = Time.time;
        float t;
        beginAction?.Invoke();
        while((Vector2)selector.position != targetPosition) {
            t = (Time.time - startTime) / movementDuration;
            selector.position = new Vector2(Mathf.SmoothStep(selector.position.x, targetPosition.x, t),Mathf.SmoothStep(selector.position.y, targetPosition.y, t));
            yield return new WaitForFixedUpdate();
        }
        endAction?.Invoke();
        yield break;
    }

    public static IEnumerator Shake(Transform transform, float timeToShake, float ShakeAmount)
    {
        float counter = timeToShake;
        while (counter > 0)
        {
            Debug.LogWarning("asdasdasd");
            float x = transform.transform.position.x * Mathf.Sin(Mathf.PI * Time.deltaTime * ShakeAmount) * 0.1f;
            float y = transform.transform.position.y * Mathf.Sin(Mathf.PI * Time.deltaTime *  ShakeAmount) * 0.1f;
            transform.position = new Vector2(x, y);
            counter -= StaticObjects.DeltaGameTime;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }


}
