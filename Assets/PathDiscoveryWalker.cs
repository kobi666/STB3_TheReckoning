using BezierSolution;
using UnityEngine;

public class PathDiscoveryWalker : MonoBehaviour
{
    public BezierWalkerWithSpeed walker;

    private void Awake()
    {
        walker = GetComponent<BezierWalkerWithSpeed>();
        walker.speed = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
