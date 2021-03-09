using System.Collections.Generic;
using UnityEngine;

public abstract class TowerCrosshairMovementController : MonoBehaviour
{
    PlayerInput playerinput;
    TowerCrosshairManager towerCrosshairManager;
    public Dictionary<Vector2, GameObject> TowerSlotsWithPositions;
    public TowerCrosshairData Data {get => towerCrosshairManager.Data;}
    // Start is called before the first frame update
    public float MoveLock;

    private void Awake() {
        playerinput = new PlayerInput();
    }

    private void OnEnable() {
        playerinput.GamePlay.Enable();
    }

    private void OnDisable() {
        
    }
}
