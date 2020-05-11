using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackDebug : MonoBehaviour
{
    PlayerUnitController pc;
    TextMeshPro textobj;
    Color32 color;

    IEnumerator textDebugFlickerCR() {
           textobj.faceColor = new Color32(color.r, color.g, color.b, 1);
           yield return new WaitForSeconds(1.0f);
           textobj.faceColor = new Color32(color.r, color.g, color.b, 1);
        }

    public void TextDebugFlicker(PlayerUnitController pc) {
            Debug.LogWarning("attack");
            
        }
    // Start is called before the first frame update
    void Start()
    {
        textobj = GetComponent<TextMeshPro>();
        color = textobj.color;
        pc = transform.parent.GetComponent<PlayerUnitController>() ?? null;
        pc.onAttack += TextDebugFlicker;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
