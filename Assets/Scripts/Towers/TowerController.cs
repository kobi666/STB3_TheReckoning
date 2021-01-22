using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerActionManager TowerActionManager;
    [HideInInspector] public GenericWeaponController WeaponController;
    [HideInInspector]
    public TowerSlotController ParentSlotController;
    public List<TowerComponent> TowerComponents = new List<TowerComponent>();
    private event Action<TowerSlotController> onInit;

    public void OnInit(TowerSlotController tsc)
    {
        onInit?.Invoke(tsc);
    }
    protected void Awake()
    {
        TowerComponents = GetComponentsInChildren<TowerComponent>().ToList();
        foreach (var tc in TowerComponents)
        {
            tc.ParentTowerConroller = this;
        }
        TowerActionManager = GetComponent<TowerActionManager>();
        onInit += delegate(TowerSlotController towerSlotController) { ParentSlotController = towerSlotController;};
        onInit += delegate(TowerSlotController controller) { WeaponController = GetComponentInChildren<GenericWeaponController>(); };
        onInit += TowerActionManager.initActionManager;
    }

    protected void Start()
    {
         
    }
}
