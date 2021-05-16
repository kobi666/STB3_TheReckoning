using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CursorSelectablesManager : MonoBehaviour
{
    public Dictionary<Vector2,ICursorSelctable> Selectables = new Dictionary<Vector2, ICursorSelctable>();
    
    public static Dictionary<Vector2, TowerPositionData> CardinalTowersNoAnglesLoopOver(GameObject self,  Dictionary<Vector2, TowerSlotController> allTowers, CardinalSet cardinalSet, int rangeCheckCyclesAmount) {
    Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
    Vector2 selfPosition = self.transform.position;
    float towerDiscoveryRange = StaticObjects.Instance.TowerSize;

    for (int i =0 ; i < cardinalSet.length ; i++ ) {
        dict.Add(cardinalSet.directionsClockwise[i], new TowerPositionData(null, 999f, 99));
        foreach (var tower in allTowers)
        {
            if (tower.Value.name == self.name || tower.Value == null) {
                continue;
            }
            for (int ii = 1; ii < rangeCheckCyclesAmount; ii++)
            {
                float newDiscoveryRange = (towerDiscoveryRange * (float)ii);
                TowerPositionQuery tq = new TowerPositionQuery(selfPosition, tower.Key, towerDiscoveryRange, newDiscoveryRange);
                if (tower.Value.name == dict[cardinalSet.directionsClockwise[i]].TowerSlotGo?.name)
                {
                    continue;
                }
                if (cardinalSet.discoveryConditionsClockwise[i](tq))
                {
                    float d = Vector2.Distance(selfPosition, tower.Key);
                    TowerPositionData tempTowerPos = new TowerPositionData(tower.Value.gameObject, tower.Value, d, ii);
                    float currentDistanceScore =
                        GetDistanceScore(towerDiscoveryRange, dict[cardinalSet.directionsClockwise[i]]);
                    float candidateDistanceScore = GetDistanceScore(towerDiscoveryRange, tempTowerPos);
                    
                    if (candidateDistanceScore < currentDistanceScore)
                    {
                        dict[cardinalSet.directionsClockwise[i]] = tempTowerPos;
                    }
                }
            }
        }
        
        
    }

    return dict;
    }
    
    public static float GetDistanceScore( float baseDiscoveryRange, TowerPositionData towerPos)
    {
        float distanceInDistanceUnits = (towerPos.Distance / baseDiscoveryRange);
        float DistanceScore = distanceInDistanceUnits + ((towerPos.DiscoveryRangeCycleNumber * baseDiscoveryRange) * StaticObjects.Instance.DistanceScoreMultiplier);
        return DistanceScore;
    }
    
    
}

public class CardinalSet {
    public Vector2[] directionsClockwise;
    public float[] anglesClockwise;
    public string[] directionNamesClockwise;
    public Predicate<TowerPositionQuery>[] discoveryConditionsClockwise;
    public int length {
        get => directionsClockwise.Length;
    }
    public CardinalSet (Vector2[] dirs, float[] angles, string[] dirNames, Predicate<TowerPositionQuery>[] discoveryConditions) {
        directionsClockwise = dirs;
        anglesClockwise = angles;
        directionNamesClockwise = dirNames;
        discoveryConditionsClockwise = discoveryConditions;

    }
    
    public static Vector2[] DirectionsClockwise8 = { new Vector2(0,1), new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(0,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static Vector2[] DirectionsClockwise4 = { new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)};
    public static Vector2[] DirectionsClockwise6 = {new Vector2(1,1),new Vector2(1,0),new Vector2(1,-1),new Vector2(-1,-1),new Vector2(-1,0),new Vector2(-1,1)};
    public static float[] AnglesClockwise8 = {90,45,360,315,270,225,180,135};
    public static float[] AnglesClockwise4 = {90,360,270,180};
    public static string[] DirectionNamesClockWise8 = {"UP","UP_RIGHT","RIGHT","DOWN_RIGHT","DOWN","DOWN_LEFT","LEFT","UP_LEFT"};
    public static string[] DirectionNamesClockWise4 = {"UP","RIGHT","DOWN","LEFT"};
    public static Predicate<TowerPositionQuery>[] DiscoveryConditions8 = {GetUpTower, GetUpRightTower,GetRightTower,GetDownRightTower,GetDownTower,GetDownLeftTower,GetLeftTower,GetUpLeftTower};

    public static bool GetUpTower(TowerPositionQuery tq) {
        
        if (tq.TargetTower.y > tq.ThisTower.y + tq.BaseDiscoveryRange) {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.DiscoveryRange );
                
        }
        return false;
    }
    public static bool GetUpRightTower(TowerPositionQuery tq) {
        Vector2 pos = new Vector2(tq.ThisTower.x + tq.DiscoveryRange , tq.ThisTower.y + tq.DiscoveryRange );
            if (tq.TargetTower.y > pos.y)
            {
                if (tq.TargetTower.x > pos.x)
                {
                    return true;
                }
            }

            return false;
    }

    public static bool GetRightTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.x > tq.ThisTower.x + tq.BaseDiscoveryRange )
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.DiscoveryRange);
            }
        

        return false;
    }

    public static bool GetDownRightTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x + tq.BaseDiscoveryRange, tq.ThisTower.y - tq.BaseDiscoveryRange );
            if (tq.TargetTower.y < pos.y)
            {
                if (tq.TargetTower.x > pos.x)
                {
                    return true;
                }
            }
        

        return false;
    }

    public static bool GetDownTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.y < tq.ThisTower.y - tq.BaseDiscoveryRange)
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.x, tq.TargetTower.x, tq.DiscoveryRange );
                
            }
        
        return false;
    }

    public static bool GetDownLeftTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x - tq.BaseDiscoveryRange , tq.ThisTower.y - tq.BaseDiscoveryRange );
            if (tq.TargetTower.y < pos.y)
            {
                if (tq.TargetTower.x < pos.x)
                {
                    return true;
                }
            }
        

        return false;
    }

    public static bool GetLeftTower(TowerPositionQuery tq) {
        
            if (tq.TargetTower.x < tq.ThisTower.x - tq.BaseDiscoveryRange)
            {
                return FindIfTowerInStraightPositionRangeXorY(tq.ThisTower.y, tq.TargetTower.y, tq.DiscoveryRange );
                
            }
        

        return false;
    }

    public static bool GetUpLeftTower(TowerPositionQuery tq) {
        
            Vector2 pos = new Vector2(tq.ThisTower.x - tq.BaseDiscoveryRange , tq.ThisTower.y + tq.BaseDiscoveryRange );
            if (tq.TargetTower.y > pos.y)
            {
                if (tq.TargetTower.x < pos.x)
                {
                    return true;
                }
            }
        

        return false;
    }
    public static CardinalSet Cardinal4 = new CardinalSet(DirectionsClockwise4, AnglesClockwise4, DirectionNamesClockWise4, DiscoveryConditions8);
    public static CardinalSet Cardinal8 = new CardinalSet(DirectionsClockwise8, AnglesClockwise8, DirectionNamesClockWise8, DiscoveryConditions8);
    
    public static bool FindIfTowerInStraightPositionRangeXorY(float myPosXorY, float targetPosXorY, float posRange) {
        float max = myPosXorY + posRange;
        float min = myPosXorY - posRange;
        float halfTowerSize = posRange / 2;
        if (targetPosXorY - halfTowerSize >= min) {
            if(targetPosXorY - halfTowerSize <= max) {
                return true;
            }
        }
        if (targetPosXorY + halfTowerSize >= min) {
            if(targetPosXorY + halfTowerSize <= max) {
                return true;
            }
        }
        return false;
    }
}


