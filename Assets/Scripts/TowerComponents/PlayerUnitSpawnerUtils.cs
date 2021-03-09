using UnityEngine;

public class PlayerUnitSpawnerUtils
{
    public static PlayerUnitController SpawnPlayerUnit(PlayerUnitController unitPrefab, Vector2 targetPosition, int unitBaseIndex) {
        PlayerUnitController puc = GameObject.Instantiate(unitPrefab, targetPosition, Quaternion.identity);
        puc.name = unitPrefab.name + "_" + UnityEngine.Random.Range(10000,50000);
        puc.UnitBaseIndex = unitBaseIndex;
        puc.gameObject.SetActive(true);
        return puc;
    }

    /*public static PlayerUnitController SpawnPlayerUnitFromSpawner(GenericUnitSpawner spawner, int unitBaseIndex)
    {
        PlayerUnitController puc = spawner.UnitPools[spawner.Units[0].UnitPrefabBase.name].Get();
        puc.UnitBaseIndex = unitBaseIndex;
        puc.transform.position = spawner.SpawningPointPosition;
        //PlayerUnitController puc = SpawnPlayerUnit(spawner.Data.SpawnerData.playerUnitPrefab,spawner.SpawningPointPosition, unitBaseIndex);
        puc.dataLegacy.SetPosition = spawner.GetRallyPoint(unitBaseIndex);
        return puc;
    }*/

    
    public static Vector2 RadianToVector2(float radian)
        {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    public static Vector2 DegreeToVector2(float degree)
        {
        return RadianToVector2(degree * Mathf.Deg2Rad);
        }
    
    public static float[] UnitPositionDegrees2 = {0f,180f};
    public static float[] UnitPositionDegrees3 = {0f,120f,240f};
    public static float[] UnitPositionDegrees4 = {0f,180f,270f,90f};
    public static float[] UnitPositionDegrees5 = {0f, 144f, 288f,72f,216f};
    public static float[] UnitPositionDegrees6 = {0.0f,240f,120f,300f,180f,60f};

    public static float[] GetUnitPositionDegrees(int maxUnits) {
        switch (maxUnits) {
            case 2:
                return UnitPositionDegrees2;
            case 3: 
                return UnitPositionDegrees3;
            case 4:
                return UnitPositionDegrees4;
            case 5: 
                return UnitPositionDegrees5;
            case 6:
                return UnitPositionDegrees6;
            default:
                Debug.LogWarning("undefined max number of orbs " + maxUnits +  " , Defaulting to calculated degrees");
                return UnitPositionDegrees(maxUnits);
        }
    }

    public static float[] UnitPositionDegrees(int maxOrbs) {
        float[] fa = new float[maxOrbs -1];
        float degreeDelta = 360 / maxOrbs;
        for (int i=1; i <= fa.Length+1 ; i++) {
            fa[i] = i * degreeDelta;
        }
        return fa;
    }
    
}
