using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoroutineTest : MonoBehaviour
{
    float counter;

    IEnumerator PHcoroutine;

    IEnumerator TestCoroutine(float f) {
        while (true) {
            Debug.LogWarning("Test coroutine is occuring " + f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoroutineToInitlizeInnerCoroutine() {
        try {
            StopCoroutine(PHcoroutine);
        }
        catch (Exception e) {
            Debug.LogWarning(e.Message);
        }
        PHcoroutine = TestCoroutine(counter);
        StartCoroutine(PHcoroutine);
        while(counter < 10) {
            Debug.LogWarning("Initilizer is occuring");
            yield return new WaitForFixedUpdate();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineToInitlizeInnerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
    }
}
