using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrosshair : MonoBehaviour
{
    
    Vector2 movement = Vector2.zero;
    PlayerInput PlayerControl;
    public GameObject originPos;
    Vector2 currentPos {
        get => originPos.transform.position;
    }
    // Start is called before the first frame update
    public float speed;
    void MoveInRange(Vector2 Movement) {
        transform.Translate(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime, 0 );
    }

    void returnToOrigin() {
        Debug.Log("Cancelled");
        movement = currentPos;
        //transform.Translate(currentPos.x * speed * Time.deltaTime, currentPos.y * speed * Time.deltaTime,0);
    }

    private void Awake() {
        PlayerControl = new PlayerInput();
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => movement = ctx.ReadValue<Vector2>();
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => returnToOrigin();
    }
    private void OnEnable() {
        PlayerControl.GamePlay.Enable();
    }

    private void OnDisable() {
        PlayerControl.GamePlay.Disable();
    }
    void Start()
    {
        //Vector2 currentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveInRange(movement);
        transform.position = new Vector2(Mathf.Clamp(movement.x, currentPos.x - 1.5f, currentPos.x + 1.5f), Mathf.Clamp(movement.y, currentPos.y - 1.5f, currentPos.y + 1.5f));
    }
}
