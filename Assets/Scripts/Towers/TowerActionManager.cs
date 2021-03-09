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
        parentSlotController = tsc;
        parentTowerController = tsc.ChildTower;
        Actions.initActions(tsc);
    }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        PlayerInput.TestButtons.T.performed += ctx => Actions.North.ExecuteAction(parentSlotController);
    }

    private void Start()
    {
        
    }
}
