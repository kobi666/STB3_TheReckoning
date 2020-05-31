using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sometest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject root;
    public Quaternion ZeroAngle = new Quaternion(0,0,0,0);
    public Quaternion angle;

    [SerializeField]
    Dictionary<string, int> testdict = new Dictionary<string, int>();

    public Dictionary<string, int> TestDict {
        get => testdict;
    }

    

    bool G;

    [SerializeField]
    public bool Gtest {
        get => G;
        set {
            G = value;
            if (value == true || value == false) {
                TestDict.Add(name + UnityEngine.Random.Range(15,65800000).ToString(), 5);
            }
        }
    }
    
    public bool b;
    public bool btest() {
        return b;
    }

    Vector3 postest() {
        return root.transform.position;
    }

    IEnumerator testCoroutine(Vector3 pos) {
        while (btest()) {
            Debug.LogWarning(pos);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    


    private void Start() {
        angle = ZeroAngle;
        StartCoroutine(testCoroutine(postest()));
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(root.transform.position, Vector3.back, 75 * Time.deltaTime);
        transform.rotation = angle;
        Gtest = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.LogWarning("asdasdasd");
        if (other.gameObject.CompareTag("Enemy")) {
            //angle = Quaternion.LookRotation(other.transform.position - transform.position);
        }
    }
}
