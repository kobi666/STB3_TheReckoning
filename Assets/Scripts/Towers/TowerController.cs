using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
[RequireComponent(typeof(TowerActionManager))]
public class TowerController : MyGameObject
{
    [InfoBox("Can't be 0...")] public int DefaultTowerCost = 10;
    
    public TowerActionManager TowerActionManager;
    [HideInInspector] public GenericWeaponController WeaponController;
    [HideInInspector]
    public TowerSlotController ParentSlotController;
    public List<TowerComponent> TowerComponents = new List<TowerComponent>();
    
    [ValidateInput("biggerThanOne")]
    public int TowerRank = 0;

    bool biggerThanOne(int value)
    {
        return (value > 0);
    }

    [Required, PreviewField] public Sprite TowerSprite;
    
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
        if (TowerActionManager == null) {
        TowerActionManager = GetComponent<TowerActionManager>();
        }
        onInit += delegate(TowerSlotController towerSlotController) { ParentSlotController = towerSlotController;};
        onInit += delegate(TowerSlotController controller) { WeaponController = GetComponentInChildren<GenericWeaponController>(); };
        onInit += TowerActionManager.initActionManager;
    }

    protected void Start()
    {
         GameManager.Instance.PlayerTowersManager.ActiveTowers.Add(MyGameObjectID,this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerTowersManager.ActiveTowers.Remove(MyGameObjectID);
    }
}
