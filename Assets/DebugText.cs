using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    // Start is called before the first frame update
    public static DebugText D;
    public Text text;

    public void SetText(string _text) {        
        text.text = _text;
    }

    private void Start() {
        D = this;
        text = GetComponent<Text>();
    }
}
