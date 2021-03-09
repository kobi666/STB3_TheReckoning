using UnityEngine;

public class RoomBorder : MonoBehaviour
{

    public EdgeCollider2D Edge_Collider;
    public Rigidbody2D Rigid_body2D;
    private void Awake() {
        Edge_Collider = GetComponent<EdgeCollider2D>() ?? null;
        Rigid_body2D = GetComponent<Rigidbody2D>() ?? null;
    }
    
}
