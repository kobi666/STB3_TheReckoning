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

   public void SpawnEnemyToSpline(GameObject _enemyGO, GameObject _spline, Vector2 _position) {
       GameObject E = GameObject.Instantiate(_enemyGO, _position, Quaternion.identity);
       E.name = (E.name + Random.Range(10000, 99999).ToString());
       E.GetComponent<BezierSolution.UnitWalker>().spline = _spline.GetComponent<BezierSolution.BezierSpline>();
   }

    public IEnumerator SpawnEnemiesToSplineAtInterval (int numberOfEnemies, float intervalBetweenSpawns) {
        for (int i = 0 ; i < numberOfEnemies ; i++) {
            SpawnEnemyToSpline(TestEnemyPrefab, SplineGOs[0],_initialPoints[0]);
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
        Debug.Log("Finished");
        yield break;
    }

    public IEnumerator SpawnEnemiesToSplineAtInterval_RandomSpline (int numberOfEnemies, float intervalBetweenSpawns) {
        
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
           StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline(20, 2.0f));
   }

   
}
