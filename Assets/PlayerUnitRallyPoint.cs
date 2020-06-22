using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitRallyPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public float DistanceFromRallyPoint = 1.5f;

    Vector2[] RallyPoints(int maxUnits) {
        Vector2[] rallyPoints;
            float[] rallyPointsDegrees = PlayerUnitSpawnerUtils.GetUnitPositionDegrees(maxUnits);
            rallyPoints = new Vector2[maxUnits -1];
            for (int i = 0 ; i < maxUnits -1 ; i++) {
                rallyPoints[i] = (Vector2)transform.position + (PlayerUnitSpawnerUtils.DegreeToVector2(rallyPointsDegrees[i]) * DistanceFromRallyPoint);
            }
            return rallyPoints;
        }
        
    public Vector2 GetRallyPoint(int index, int maxUnits) {
        Vector2 pos;
        if (maxUnits == 1 || maxUnits <= 0) {
            pos = transform.position;
        }
        else {
            pos = RallyPoints(maxUnits)[index];
        }
        return pos;
    }
}
