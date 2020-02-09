using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public SubWave TestSubwave;
    public List<Spawner> Spawners;
    public Dictionary<string, Spawner> SpawnerDict = new Dictionary<string, Spawner>();
    //used for specific Enemy Selection
    public Dictionary<string, GameObject> EnemisDict = new Dictionary<string, GameObject>();
    //used to initilize specific enemy Dictionary and random Enemy selection
    public List<GameObject> EnemyUnitPrefabs = new List<GameObject>();

    void InitilizeEnemiesDict() {
        foreach (GameObject _EnemyUnit in EnemyUnitPrefabs) {
            if (_EnemyUnit != null) {
                EnemisDict.Add(_EnemyUnit.name, _EnemyUnit);
            }
        }
    }

    void InitilizeSpawners() {
        foreach (GameObject _spawner in GameObject.FindGameObjectsWithTag("Spawner")) {
            Spawner S = _spawner.GetComponent<Spawner>();
            if (S != null) {
                Spawners.Add(S);
                SpawnerDict.Add(S.gameObject.transform.parent.name, S);
            }
            else {
                Debug.Log("Spawner Object has no \'Spawner\' controller attached at path " + S.gameObject.transform.parent.name );
            }
        }
    }

    private void Start() {

        InitilizeEnemiesDict();
        InitilizeSpawners();
        TestSubwave = new SubWave(2.0f, 16, EnemisDict["Akuma"], "random");
        Spawners[0].SpawnSubWave(TestSubwave);
    }
}
