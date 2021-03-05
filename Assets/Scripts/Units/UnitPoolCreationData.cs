using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable]
public class UnitPoolCreationData
{
    public GameObject ParentGameObject;
    [SerializeField] public GenericUnitController GenericUnitController;
#if UNITY_EDITOR
    [TagSelector]
# endif
    public string GroupTag;
    
    
#if UNITY_EDITOR
    [TagSelector]
# endif
    public string TargetGroupTag;
    
    [TypeFilter("getEffects")]
    public List<Effect> MeleeEffects;

    private IEnumerable<Type> getEffects()
    {
        return Effect.GetEffects();
    }


    public UnitMetaData UnitMetaData;
    public PoolObjectQueue<GenericUnitController> PoolObjectQueue;

    public PoolObjectQueue<GenericUnitController> CreateUnitPool()
    {
        GameObject parent = GameObject.Instantiate(new GameObject(), GameObjectPool.Instance.transform);
        GenericUnitController guc =
            GameObject.Instantiate(GenericUnitController, parent.transform);
        guc.gameObject.SetActive(false);
        guc.Data.MetaData = UnitMetaData;
        if (!MeleeEffects.IsNullOrEmpty()) {
        guc.UnitBattleManager.MeleeWeapon.WeaponAttack.SetEffectList(MeleeEffects);
        }
        guc.GroupTag = GroupTag;
        guc.EffectableTargetBank.DiscoverableTags.Clear();
        guc.EffectableTargetBank.DiscoverableTags.Add(TargetGroupTag);
        
        parent.name = "placeholder_" + ParentGameObject.name + "_" + GenericUnitController.name;
        PoolObjectQueue<GenericUnitController> pool = 
            new PoolObjectQueue<GenericUnitController>(GenericUnitController, 5, parent);
        return pool;
    }
}


