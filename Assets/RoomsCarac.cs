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

public static class RoomsCarac
{
    public static Dictionary<string, RoomCaracEnum> Caracs = new Dictionary<string, RoomCaracEnum> 
    {
        {"Hangar",
            new RoomCaracEnum()
            {
                BlocCorner = "Rooms/HangarProc/Blocs/Corner",
                BlocGround = "Rooms/HangarProc/Blocs/Ground",
                BlocStraight = "Rooms/HangarProc/Blocs/Straight",
                BlocDoor = "Rooms/HangarProc/Blocs/Door",
                //BlocSize = 64.516f,
                BlocSize = 50,
            }
        }
    };
}
