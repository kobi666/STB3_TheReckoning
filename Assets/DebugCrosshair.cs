using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrosshair : MonoBehaviour
{
    
    Vector2 movement;
    PlayerInput PlayerControl;
    public GameObject originPos;
    Vector2 currentPos;
    
    Vector2 pos;
    // Start is called before the first frame update
    public float speed;
    void MoveCursor(Vector2 Movement) {
        Vector2 newPos = new Vector2(transform.position.x  + movement.x , transform.position.y + movement.y);
        newPos.x = Mathf.Clamp(newPos.x, currentPos.x - 1.5f, currentPos.x + 1.5f);
        newPos.y = Mathf.Clamp(newPos.y, currentPos.y - 1.5f, currentPos.y + 1.5f);
        transform.position = Vector2.MoveTowards(transform.position, newPos, 0.2f);
    }

    void returnToOrigin() {
        Debug.Log("Cancelled");
        transform.position = Vector2.MoveTowards(transform.position, currentPos, 0.2f);
       
    }

    private void Awake() {
        currentPos = transform.position;
        movement = Vector2.zero;
        PlayerControl = new PlayerInput();
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => movement = ctx.ReadValue<Vector2>();
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => movement = Vector2.zero;
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
        if(movement != Vector2.zero) {
            MoveCursor(movement);
        }
        else {
            returnToOrigin();
        }
        Debug.DrawRay(transform.position, currentPos - (Vector2)transform.position, Color.yellow);
    }
}
