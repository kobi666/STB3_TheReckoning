using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    
    public float GameTimeMultiplier = 1.0000f;
    public float GameTime = 0.0000f;
    // Start is called before the first frame update
    public static Transform PPH;
    public float DeltaGameTime {
        get {
            return Time.deltaTime * GameTimeMultiplier;
        }
    }
    public static StaticObjects instance;
    public float TowerSize;
    public Sprite TowerSprite;
    void Awake()
    {
        instance = this;
        GameTime = Time.time;
        TowerSize = TowerSprite.bounds.size.x;
    }

    private void Start() {
        PPH = GameObject.FindGameObjectWithTag("PPH")?.transform ?? null;
    }

    private void update() {
        GameTime = (Time.time * GameTimeMultiplier);
        
    }

    // Update is called once per frame
    
}
