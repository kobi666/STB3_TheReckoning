using UnityEngine;

[System.Serializable]
public class TowerActionManager : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private TowerSlotController parentSlotController;
    private TowerController parentTowerController;
    public TowerActions Actions = new TowerActions();

    public void initActionManager(TowerSlotController tsc)
    {
        if (tsc == null)
        {
            Debug.LogError("TSC is NULL");
        }
        parentSlotController = tsc;
        parentTowerController = tsc.ChildTower;
        Actions.initActions(tsc);
    }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        
    }

    private void Start()
    {
        
    }
}
