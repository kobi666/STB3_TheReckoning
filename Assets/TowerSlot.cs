using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [field:SerializeField]
    public bool IsTargetable {get;set;}
    public Dictionary<Vector2, TowerUtils.TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerUtils.TowerPositionData>();

}
