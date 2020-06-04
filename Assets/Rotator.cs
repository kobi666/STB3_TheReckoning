using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rotator<T> : MonoBehaviour where T : IOrbital<T>
{
    public bool OrbitalsOrbitBound = true;
    public abstract int MaxNumOfOrbitals {get;} 
    public Vector2[] OrbitalDirections(int numOfOrbitals) {
        return TowerUtils.GetDirections(numOfOrbitals);
    }
    SortedList<string, (T, bool)> orbitalList = new SortedList<string, (T, bool)>();

    public void EnableRotationForOrbital(string orbName) {
        if (OrbitalList.ContainsKey(orbName)) {
            (T, bool) t = (OrbitalList[orbName].Item1, true);
            OrbitalList[orbName] = t;
        }
        if (OrbitalsOrbitBound) {
            bool b = true;
            foreach (var item in OrbitalList)
            {
                if (item.Value.Item2 == false) {
                    b = false;
                }
            }
            if (b) {
                StartRotationAllOrbitals();
            }
        }
        if (!OrbitalsOrbitBound) {
            OrbitalList[orbName].Item1.ReStartOrbiting();
        }
    }

    public void DisableRotationForOrbital(string orbName) {
        if (OrbitalList.ContainsKey(orbName)) {
            (T, bool) t = (OrbitalList[orbName].Item1, false);
            OrbitalList[orbName] = t;
            if (OrbitalsOrbitBound) {
                StopRotationAllOrbitals();
            }
            if (!OrbitalsOrbitBound) {
                OrbitalList[orbName].Item1.StopOrbiting();
            }
        }
    }

    public SortedList<string, (T, bool)> OrbitalList {
        get => orbitalList;
    }

    

    

    public void AddOrbital(T t) {
        if (OrbitalList.Count <= MaxNumOfOrbitals) {
            if (OrbitalList.ContainsKey(t.OrbitalName)) {
                return;
            }
            else {
                OrbitalList.Add(t.OrbitalName, (t, true));
            }
            //var degreesbetween = 360 / MaxNumOfOrbitals;
            for (int i = 0 ; i < OrbitalList.Count ; i++) {
                T orb = OrbitalList.Values[i].Item1;
                orb.AngleForOrbit = WeaponUtils.GetOrbRotationDegrees(MaxNumOfOrbitals)[i];
                //Vector2 v = OrbitalDirections(MaxNumOfOrbitals)[i] * orb.DistanceFromOrbitalBase;
                orb.OrbitalTransform.position = WeaponUtils.DegreeToVector2(orb.AngleForOrbit) * orb.DistanceFromOrbitalBase;
                EnableRotationForOrbital(orb.OrbitalName);
            }
        }
        else {
            Debug.LogWarning("Max num of Orbitals Reached");
        }
    }

    public void RemoveOrbital(T t) {
        if (OrbitalList.Count >= 0) {
            if (OrbitalList.ContainsKey(t.OrbitalName)) {
                OrbitalList.Remove(t.OrbitalName);
                for (int i = 0 ; i <= OrbitalList.Count ; i++) {
                OrbitalList.Values[i].Item1.OrbitalTransform.position = Utils.V2toV3(OrbitalDirections(MaxNumOfOrbitals)[i]);
                    }
            }
        }
        else {
            Debug.LogWarning("Min number of Orbitals Reached");
        }
    }

    public void StartRotationAllOrbitals() {
        foreach (var item in OrbitalList)
        {
                item.Value.Item1.ReStartOrbiting();
        }
    }

    public void StopRotationAllOrbitals() {
        foreach (var item in OrbitalList)
        {
                item.Value.Item1.StopOrbiting();
        }
    }

    


    


    
}
