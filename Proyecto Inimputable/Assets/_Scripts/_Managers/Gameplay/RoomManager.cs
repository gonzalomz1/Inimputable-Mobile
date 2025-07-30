using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Door firstRoomEntrance;
    public Door firstRoomExit;
    public Door secondRoomEntrance;

    void Start()
    {
        GameplayManager.instance.FirstRoomLock += LockFirstRoomDoors;
        GameplayManager.instance.FirstRoomUnlock += OpenFirstRoomDoor;
        GameplayManager.instance.SecondRoomLock += LockSecondRoomDoor;
    }

    void LockFirstRoomDoors()
    {
        firstRoomEntrance.LockStateDoor();
        firstRoomExit.LockStateDoor();
    }
    void OpenFirstRoomDoor()
    { 
        firstRoomExit.DefaultStateDoor();
    }
    void LockSecondRoomDoor() => secondRoomEntrance.LockStateDoor();
}
