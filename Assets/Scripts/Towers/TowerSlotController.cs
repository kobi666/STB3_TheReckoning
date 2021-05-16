using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class TowerSlotController : MonoBehaviour
{
    public LevelManager MyLevelManager;
    [Required]
    public TowerController childTower;
    
    public TowerActions TowerActions
    {
        get => childTower?.TowerActionManager.Actions ?? null;
    }

    SpriteRenderer SR;
    [ShowInInspector]
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    // Start is called before the first frame update

    private TowerController oldTower;
    

    public TowerController ChildTower
    {
        get => childTower;
        set => childTower = value;
    }

    public void PlaceNewTower(TowerController newTowerPrefab) {
        if (childTower != null) {
            oldTower = childTower;
            childTower = null;
        }

        TowerController newTower =
            Instantiate(newTowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        newTower.ParentSlotController = this;
        childTower = newTower;
        childTower.name = (newTowerPrefab.name + UnityEngine.Random.Range(10000, 99999).ToString());
        SelectorTest2.instance.SelectedTowerSlot = this;
        if (oldTower != null) {
            Destroy(oldTower.gameObject);
        }
        SR.sprite = null;
    }

    public void CalculateAdjecentTowers()
    {
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, MyLevelManager.LevelTowerSlots.Values.ToArray(), TowerUtils.Cardinal8,20);
    }

    
    public event Action onTowerPositionCalculation;

    public void OnTowerPositionCalculation()
    {
        onTowerPositionCalculation?.Invoke();
    }

    protected void Awake()
    {
        onTowerPositionCalculation += CalculateAdjecentTowers;
    }
    
    


    protected void Start()
    {
        MyLevelManager = GameManager.Instance.CurrentLevelManager;
        MyLevelManager.OnTowerSlotUpdate(this,true);
        SR = GetComponent<SpriteRenderer>();
        ChildTower = GetComponentInChildren<TowerController>();
        TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, MyLevelManager.LevelTowerSlots.Values.ToArray(), TowerUtils.Cardinal8,6);
        OnTowerPositionCalculation();
        //ChildTower.OnInit(this);
        TowerActions?.initActions(this);
    }

}

