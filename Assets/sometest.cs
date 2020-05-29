using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sometest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject root;
    public Quaternion ZeroAngle = new Quaternion(0,0,0,0);
    public Quaternion angle;
    public bool b;
    


    private void Start() {
        angle = ZeroAngle;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(root.transform.position, Vector3.back, 75 * Time.deltaTime);
        transform.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.LogWarning("asdasdasd");
        if (other.gameObject.CompareTag("Enemy")) {
            //angle = Quaternion.LookRotation(other.transform.position - transform.position);
        }
    }
}
