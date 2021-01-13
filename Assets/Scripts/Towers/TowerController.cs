using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerActionManager TowerActionManager;
    [HideInInspector] public GenericWeaponController WeaponController;
    private TowerSlotController parentSlotController;
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
        onInit += delegate(TowerSlotController towerSlotController) { parentSlotController = towerSlotController;};
        onInit += TowerActionManager.initActionManager;
    }

    protected void Start()
    {
        WeaponController = GetComponentInChildren<GenericWeaponController>();
    }
}
