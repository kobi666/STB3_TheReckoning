using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackDebug : MonoBehaviour
{
    TextMeshPro textobj;
    Color32 color;

    IEnumerator textDebugFlickerCR() {
           textobj.faceColor = new Color32(color.r, color.g, color.b, 1);
           yield return new WaitForSeconds(1.0f);
           textobj.faceColor = new Color32(color.r, color.g, color.b, 1);
        }

    public  void TextDebugFlicker() {
            Debug.LogWarning("attack");
            StartCoroutine(textDebugFlickerCR());
        }
    // Start is called before the first frame update
    void Start()
    {
        textobj = GetComponent<TextMeshPro>();
        color = textobj.color;
        transform.parent.GetComponent<UnitController>().onAttack += TextDebugFlicker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
