using UnityEngine;

public class StaticObjects : MonoBehaviour
{
    public RangeDetector RangeDetector;
    public static float GameTimeMultiplier = 1.0000f;
    public float GameTime = 0.0000f;
    // Start is called before the first frame update
    public static Transform PPH;
    public static float DeltaGameTime {
        get {
            return Time.deltaTime * GameTimeMultiplier;
        }
    }

    private static StaticObjects instance;
    static bool instantiated = false;

    public static StaticObjects Instance
    {
        get
        {
            if (instantiated == false)
            {
                GameObject g = new GameObject();
                g.name = "StaticObjects";
                g.AddComponent<StaticObjects>();
                Instance = g.GetComponent<StaticObjects>();
            }

            return instance;
        }
        set
        {
            instance = value;
            instantiated = true;
        }
    }
        
    
    
    
    
    
    
    public float TowerSize;
    public Sprite TowerSprite;
    public float DistanceScoreMultiplier;
    void Awake()
    {
        Instance = this;
        GameTime = Time.time;
        TowerSize = TowerSprite?.bounds.size.x / 2 ?? 0.8f;
    }

    private void Start() {
        PPH = GameObject.FindGameObjectWithTag("PPH")?.transform ?? null;
    }

    private void update() {
        GameTime = (Time.time * GameTimeMultiplier);
        
    }

    // Update is called once per frame
    
}
