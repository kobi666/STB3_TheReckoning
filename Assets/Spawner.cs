﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TestEnemyPrefab;
   public GameObject PathsParentObject;
   [SerializeField]

   List<GameObject> _splines;
   public List<Vector2> _initialPoints;
   List<Vector2> InitialPoints(List<GameObject> SplineGoList) {
       List<Vector2> list = new List<Vector2>();
       foreach (GameObject SplineGO in SplineGoList)
       {
           list.Add(SplineGO.GetComponent<BezierSolution.BezierSpline>()._EndPoints[0].transform.position);
       }
       return list;
       }
   
   public List<GameObject> Splines {
       
       get => _splines;
       set {
           _splines = value;
       }
       
   }

   public void SpawnEnemyToSpline(GameObject _enemyGO, GameObject _spline, Vector2 _position) {
       GameObject E = GameObject.Instantiate(_enemyGO, _position, Quaternion.identity);
       E.name = (E.name + Random.Range(10000, 99999).ToString());
       E.GetComponent<BezierSolution.UnitWalker>().spline = _spline.GetComponent<BezierSolution.BezierSpline>();
   }

   private void Awake() {
       PathsParentObject = gameObject.transform.parent.gameObject;
       for (int i = 0 ; i <= PathsParentObject.transform.childCount-1 ; i++ ) {
           if (PathsParentObject.transform.GetChild(i).gameObject.tag == "Spline") {
           Splines.Add(PathsParentObject.transform.GetChild(i).gameObject);
           }
       }
       
       
   }

   private void Start() {
       _initialPoints = InitialPoints(Splines);
       SpawnEnemyToSpline(TestEnemyPrefab,Splines[0],_initialPoints[0]);
   }

   
}