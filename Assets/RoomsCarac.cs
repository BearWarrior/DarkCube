using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomCaracEnum
{
    public string BlocCorner;
    public string BlocGround;
    public string BlocStraight;
    public string BlocDoor;
    public float BlocSize;
    public float Scale;
}

public class RoomsCarac
{
    Dictionary<string, RoomCaracEnum> Caracs = new Dictionary<string, RoomCaracEnum> 
    {
        {"Hangar",
            new RoomCaracEnum()
            {
                BlocCorner = "Rooms/HangarProc/Prefab/Corner",
                BlocGround = "Rooms/HangarProc/Prefab/Ground",
                BlocStraight = "Rooms/HangarProc/Prefab/WallStraight",
                BlocDoor = "Rooms/HangarProc/Prefab/Door",
                BlocSize = 64.516f,
            }
        }
    };
}
