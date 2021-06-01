using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalDiscoveriesManager : MonoBehaviour
{
    public Dictionary<int, (Vector2, DirectionalDiscovery)> AllDirectionalDiscoveries { get => allDirectionalDiscoveries; set => allDirectionalDiscoveries = value; }
    public Dictionary<int, (Vector2, DirectionalDiscovery)> allDirectionalDiscoveries = new Dictionary<int, (Vector2, DirectionalDiscovery)>();
}
