using UnityEngine;

public class RoomController : MonoBehaviour
{
    RoomBorder[] bordersArray;
    [SerializeField]
    RoomBorders roomBorders = new RoomBorders();
    
    [SerializeField]
    public RoomLimits roomLimits = new RoomLimits(-9999f,-9999f,9999f,9999f);
    // Start is called before the first frame update
    void SetRoomLimits(RoomBorder[] borders) {
        foreach (var item in borders)
        {
            if (item.transform.position.x > roomLimits.MaxX) {
                roomLimits.MaxX = item.transform.position.x;
                roomBorders.Right = item;
            }
            if (item.transform.position.y > roomLimits.MaxY) {
                roomLimits.MaxY = item.transform.position.y;
                roomBorders.Top = item;
            }
            if (item.transform.position.x < roomLimits.MinX) {
                roomLimits.MinX = item.transform.position.x;
                roomBorders.Left = item;
            }
            if (item.transform.position.y < roomLimits.MinY) {
                roomLimits.MinY = item.transform.position.y;
                roomBorders.Bottom = item;
            }
        }
        setBorderColliders(roomBorders);
        
    }

    private void setBorderColliders(RoomBorders roomBorders) {

        roomBorders.Top.Edge_Collider.points = new Vector2[2] {new Vector2(roomLimits.MaxX, 0f),new Vector2(roomLimits.MinX, 0f)};
        roomBorders.Top.Edge_Collider.points.SetValue(new Vector2(roomLimits.MaxX, 0f),0);
        roomBorders.Top.Edge_Collider.points.SetValue(new Vector2(roomLimits.MinX, 0f),1);

        roomBorders.Bottom.Edge_Collider.points = new Vector2[2] {new Vector2(roomLimits.MaxX, 0f),new Vector2(roomLimits.MinX, 0f)};
        roomBorders.Bottom.Edge_Collider.points.SetValue(new Vector2(roomLimits.MaxX, 0f),0);
        roomBorders.Bottom.Edge_Collider.points.SetValue(new Vector2(roomLimits.MinX, 0f),1);

        roomBorders.Left.Edge_Collider.points = new Vector2[2] {new Vector2(0f, roomLimits.MinY),new Vector2(0f, roomLimits.MaxY)};
        roomBorders.Left.Edge_Collider.points.SetValue(new Vector2(0f, roomLimits.MinY),0);
        roomBorders.Left.Edge_Collider.points.SetValue(new Vector2(0f,roomLimits.MaxY),1);

        roomBorders.Right.Edge_Collider.points = new Vector2[2] {new Vector2(0f, roomLimits.MinY),new Vector2(0f, roomLimits.MaxY)};
        roomBorders.Right.Edge_Collider.points.SetValue(new Vector2(0f, roomLimits.MinY),0);
        roomBorders.Right.Edge_Collider.points.SetValue(new Vector2(0f,roomLimits.MaxY),1);
    }


    void Start()
    {
        bordersArray = GetComponentsInChildren<RoomBorder>() ?? null;
        SetRoomLimits(bordersArray);
        
    }

    // Update is called once per frame
    
}
