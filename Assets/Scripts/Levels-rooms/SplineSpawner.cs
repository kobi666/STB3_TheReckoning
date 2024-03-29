﻿using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class SplineSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    WaveManager _waveManager = WaveManager.Instance;
    public GameObject TestEnemyPrefab;
   public pathGeneratorTest PathsParentObject;

   public IEnumerator StartWave(Wave _wave) {
       foreach (Subwave s in _wave.Subwaves) {
           yield return StartCoroutine(SpawnSubWave(s.Package));
           yield return new WaitForSeconds(s.StartupPauseSeconds);
       }
       Debug.Log("Wave Finished!");
       yield break;
   }

   [SerializeField]
   List<BezierSpline> _splineGOs;
   List<BezierSolution.BezierSpline> Splines = new List<BezierSolution.BezierSpline>();
   public List<Vector2> _initialPoints;
   List<Vector2> InitilizeInitPoints(List<BezierSpline> SplineGoList) {
       List<Vector2> list = new List<Vector2>();
       foreach (var SplineGO in SplineGoList)
       {
           list.Add(SplineGO._EndPoints[0].transform.position);
       }
       return list;
       }
   
   public List<BezierSpline> SplineGOs {
       
       get => _splineGOs;
       set {
           _splineGOs = value;
       }
       
   }

   public IEnumerator SpawnSubWave(SubWavePackage _subwave) {
       switch(_subwave._splineOrder) {
        case "random":
            yield return StartCoroutine(SpawnEnemiesToSplineFromQueueAtInterval_RandomSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab));
            break;
        case "single":
            //yield return StartCoroutine(SpawnEnemiesToSplineAtInterval_SingleSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab, _subwave._splinePosition));
            break;
        case null:
            Debug.Log("No Spline order set! Defaulting to Random!");
            //yield return StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline( _subwave._amountOfUnits, _subwave._intervalBetweenSpawns, _subwave._unitPrefab ));
            break;
        }
        //yield return new WaitForSeconds(_subwave._timeToSpawnEntireSubwave());
   }

   public void SpawnEnemyToSpline(GameObject _enemyGO, BezierSpline _spline, Vector2 _position) {
       GameObject E = GameObject.Instantiate(_enemyGO, _position, Quaternion.identity);
       E.name = (E.name + Random.Range(10000, 99999).ToString());
       E.GetComponent<BezierSolution.UnitWalker>().spline = _spline.GetComponent<BezierSolution.BezierSpline>();
   }

   public void SpawnEnemyToSplineFromQueue(PoolObjectQueue<EnemyUnitController> queue, BezierSolution.BezierSpline spline) {
       UnitController unit = queue.Get();
       unit.gameObject.SetActive(true);
       unit.Walker.spline = spline;
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
            int r = Random.Range(0,SplineGOs.Count -1);
            SpawnEnemyToSpline(TestEnemyPrefab, SplineGOs[r],_initialPoints[r]);
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
        //Debug.Log("Finished");
        yield break;
    }

    public IEnumerator SpawnEnemiesToSplineFromQueueAtInterval_RandomSpline (int numberOfEnemies, float intervalBetweenSpawns, EnemyUnitController enemyPrefab) {
        PoolObjectQueue<EnemyUnitController> queue = GameObjectPool.Instance.GetUnitQueue(enemyPrefab);
        for (int i = 0 ; i < numberOfEnemies ; i++) {
            int r = Random.Range(0,Splines.Count);
            SpawnEnemyToSplineFromQueue(queue, Splines[r]);
            yield return new WaitForSeconds(intervalBetweenSpawns);
        }
        //Debug.Log("Finished");
        yield break;
    }




   private void Awake() {
       
   }

   private void Start()
   {
       PathsParentObject = PathsParentObject ?? GetComponentInParent<pathGeneratorTest>();
       SplineGOs = PathsParentObject.Splines;
       _initialPoints = InitilizeInitPoints(SplineGOs);
       foreach (var item in SplineGOs)
       {
           Splines.Add(item.GetComponent<BezierSolution.BezierSpline>());
       }
       //SpawnEnemyToSpline(TestEnemyPrefab,SplineGOs[0],_initialPoints[0]);
       //StartCoroutine(SpawnEnemiesToSplineAtInterval(6, 2.0f)); 
       //StartCoroutine(SpawnEnemiesToSplineAtInterval_RandomSpline(20, 2.0f));
   }

   
}
