using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rotationTarget;
    public float rspeed;

    public void rotateToTarget(GameObject rt) {
        Vector2 vtt = rt.transform.position - transform.position;
        float ang = Mathf.Atan2(vtt.y, vtt.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(ang, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.instance.DeltaGameTime * rspeed);
    }

    private void Start() {
        StartCoroutine(WeaponUtils.RotateTowardsTargetGO(transform, rotationTarget.transform, rspeed));
    }
    // Update is called once per frame
    void Update()
    {
        //rotateToTarget(rotationTarget);

    }
}
