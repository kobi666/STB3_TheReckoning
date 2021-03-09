using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    public event Action<RoomController> onRoomEnter;
    public event Action<RoomController> onRoomOpened;
    public event Action<RoomController> onRoomClosed;
    
    
}
