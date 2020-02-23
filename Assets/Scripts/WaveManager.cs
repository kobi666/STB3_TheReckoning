using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    Wave TestWave = new Wave(6);
    public static WaveManager Instance;
    public List<GameObject> Paths;
    public Dictionary<string, GameObject> PathsDict = new Dictionary<string, GameObject>();
    
    public SubWavePackage TestSubwavePackage;
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

    private void Awake() {
        Instance = this;
    }

    void InitlizePaths() {
        foreach (GameObject _path in GameObject.FindGameObjectsWithTag("Path")) {
            Paths.Add(_path);
            PathsDict.Add(_path.name, _path);
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
        TestSubwavePackage = new SubWavePackage(0.2f, 1, EnemisDict["Akuma"], "random");
        // Spawners[0].SpawnSubWave(TestSubwave);
        Spawners[0].SpawnSubWave(TestSubwavePackage);
        for (int i = 0 ; i < TestWave.Subwaves.Length ; i++) {
            TestWave.Subwaves[i] = new Subwave(TestSubwavePackage, 5.0f);
        }
        StartCoroutine(Spawners[0].StartWave(TestWave));

    }
}
