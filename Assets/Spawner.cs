using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TestEnemyPrefab;
   public GameObject PathsParentObject;
   [SerializeField]

   List<GameObject> _splineGOs;
   public List<Vector2> _initialPoints;
   List<Vector2> InitilizeInitPoints(List<GameObject> SplineGoList) {
       List<Vector2> list = new List<Vector2>();
       foreach (GameObject SplineGO in SplineGoList)
       {
           list.Add(SplineGO.GetComponent<BezierSolution.BezierSpline>()._EndPoints[0].transform.position);
       }
       return list;
       }
   
   public List<GameObject> SplineGOs {
       
       get => _splineGOs;
       set {
           _splineGOs = value;
       }
       
   }

   public void SpawnSubWave(SubWave _subwave) {
       switch(_subwave._splineOrder) {
        case "random":
            StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab ));
            break;
        case "single":
            StartCoroutine(SpawnEnemiesToSplineAtInterval_SingleSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab, _subwave._splinePosition));
            break;
        case null:
            Debug.Log("No Spline order set! Defaulting to Random!");
            StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab ));
            break;
        }
   }

   public void SpawnEnemyToSpline(GameObject _enemyGO, GameObject _spline, Vector2 _position) {
       GameObject E = GameObject.Instantiate(_enemyGO, _position, Quaternion.identity);
       E.name = (E.name + Random.Range(10000, 99999).ToString());
       E.GetComponent<BezierSolution.UnitWalker>().spline = _spline.GetComponent<BezierSolution.BezierSpline>();
   }

    public IEnumerator SpawnEnemiesToSplineAtInterval_SingleSpline (int numberOfEnemies, float intervalBetweenSpawns, GameObject _EnemySpawn, int _splinePosition) {
        int pos;
        if (_splinePosition == (-1)) {
            pos = 1;
            }
        else {
            pos = _splinePosition;
            }
        
        for (int i = 0 ; i < numberOfEnemies ; i++) {
            SpawnEnemyToSpline(_EnemySpawn, SplineGOs[pos],_initialPoints[pos]);
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
        Debug.Log("Finished");
        yield break;
    }

    public IEnumerator SpawnEnemiesToSplineAtInterval_RandomSpline (int numberOfEnemies, float intervalBetweenSpawns, GameObject _enemySpawn) {
        
        for (int i = 0 ; i < numberOfEnemies ; i++) {
            int r = Random.Range(0,SplineGOs.Count);
            SpawnEnemyToSpline(TestEnemyPrefab, SplineGOs[r],_initialPoints[r]);
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
        Debug.Log("Finished");
        yield break;
    }




   private void Awake() {
       PathsParentObject = gameObject.transform.parent.gameObject;
       for (int i = 0 ; i <= PathsParentObject.transform.childCount-1 ; i++ ) {
           if (PathsParentObject.transform.GetChild(i).gameObject.tag == "Spline") {
           SplineGOs.Add(PathsParentObject.transform.GetChild(i).gameObject);
           }
       }
       
       
   }

   private void Start() {
       _initialPoints = InitilizeInitPoints(SplineGOs);
       //SpawnEnemyToSpline(TestEnemyPrefab,SplineGOs[0],_initialPoints[0]);
       //StartCoroutine(SpawnEnemiesToSplineAtInterval(6, 2.0f));
       //StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline(20, 2.0f));
   }

   
}
