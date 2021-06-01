using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;


[System.Serializable]
public class TowerSlotController : MyGameObject
{
    public LevelManager MyLevelManager;
    [Required]
    public TowerController childTower;
    
    [Required]
    public DirectionalDiscovery DirectionalDiscoveryController;
    
    public TowerActions TowerActions
    {
        get => childTower?.TowerActionManager.Actions ?? null;
    }

    SpriteRenderer SR;
    [ShowInInspector]
    public Dictionary<Vector2, TowerPositionData> TowerSlotsByDirections8 = new Dictionary<Vector2, TowerPositionData>();
    
    public Dictionary<Vector2,TowerSlotController> FoundTowerSlots = new Dictionary<Vector2, TowerSlotController>(); 
    // Start is called before the first frame update


    public Dictionary<Vector2, TowerPositionData> FindTowerSlotsByDirections8()
    {
        Dictionary<Vector2, TowerPositionData> dict = new Dictionary<Vector2, TowerPositionData>();
        DirectionalDiscoveryController.GetDirectionalDiscoveriesOnObject();
        var VectorDirectionsClockwise = TowerUtils.DirectionsClockwise4;
        for (int i = 0; i < DirectionalDiscoveryController.DirectionalDiscoveriesFound.Length; i++)
        {
            int foundTowerSlotID = DirectionalDiscoveryController.DirectionalDiscoveriesFound[i].Item2.MyGameObjectID;
            
        }

        return dict;
    }
    
    
    private TowerController oldTower;
    

    public TowerController ChildTower
    {
        get => childTower;
        set => childTower = value;
    }

    public /*async*/ void PlaceNewTower(TowerController newTowerPrefab) {
        if (childTower != null) {
            oldTower = childTower;
            childTower = null;
        }

        TowerController newTower =
            Instantiate(newTowerPrefab, transform.position, Quaternion.identity, gameObject.transform);
        
            newTower.gameObject.SetActive(true);
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
        DirectionalDiscoveryController.GetDirectionalDiscoveriesOnObject();
        var DirectionalVectors4 = TowerUtils.DirectionsClockwise4;
        for (int i = 0; i < DirectionalDiscoveryController.DirectionalDiscoveriesFound.Length; i++)
        {
            var foundDirectionalDiscovery = DirectionalDiscoveryController.DirectionalDiscoveriesFound[i].Item2;
            TowerSlotController foundTowerSlot = foundDirectionalDiscovery != null ? MyLevelManager.LevelTowerSlots[foundDirectionalDiscovery._MyGameObjectID].Item2 : null;
            FoundTowerSlots.Add(DirectionalVectors4[i],foundTowerSlot);
        }
        //TowerSlotsByDirections8 = TowerUtils.CardinalTowersNoAnglesLoopOver(gameObject, MyLevelManager.LevelTowerSlots.Values.ToArray(), TowerUtils.Cardinal8,20);
    }

    
    public event Action onTowerPositionCalculation;

    public void OnTowerPositionCalculation()
    {
        onTowerPositionCalculation?.Invoke();
    }

    protected void Awake()
    {
        MyLevelManager.LevelTowerSlots.Add(MyGameObjectID,(transform.position,this));
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

