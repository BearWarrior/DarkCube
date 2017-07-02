using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarGenerator : MonoBehaviour
{
    enum TileType {GROUND = 9, WALL = 1, DOOR = 2};

    void Start()
    {
        Hangar hangar = new Hangar();
        hangar.Generate();
        InstantiateHangar(hangar, 0.6f);
    }

    private GameObject InstantiateHangar(Hangar hangar, float scale)
    {
        GameObject hangarGO = new GameObject("HangarTest2");

        for (int i = 0; i < hangar.rooms.Count; i++)
        {
            GameObject roomGO = InstantiateRoom(hangar.rooms[i]);
            roomGO.transform.SetParent(hangarGO.transform);
        }

        hangarGO.transform.localScale = new Vector3(scale, scale, scale);
        return hangarGO;
    }

    public GameObject InstantiateRoom(Room room)
    {
        const string BLOC_CORNER = "Rooms/HangarProc/Prefab/Corner";
        const string BLOC_GROUND = "Rooms/HangarProc/Prefab/Ground";
        const string BLOC_STRAIGHT = "Rooms/HangarProc/Prefab/WallStraight";
        const string BLOC_DOOR = "Rooms/HangarProc/Prefab/Door";
        const float BLOC_SIZE = 64.516f;

        GameObject roomGO = new GameObject("Room");
        GameObject tuile = null;

        for (int l = 0; l < room.Table.Count; l++)
        {
            for (int w = 0; w < room.Table[l].Count; w++)
            {
                if (ThisOneIs(room.Table, w, l, TileType.WALL))
                {
                    if(w == 0 && l == 0)
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_CORNER), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 180, 0));
                    }
                    else if (w == room.Table[0].Count -1  && l == 0)
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_CORNER), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 270, 0));
                    }
                    else if (w == room.Table[0].Count - 1 && l == room.Table.Count - 1)
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_CORNER), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 0, 0));
                    }
                    else if (w == 0 && l == room.Table.Count - 1)
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_CORNER), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 90, 0));
                    }
                    else
                    {
                        if (BottomIs(room.Table, w, l, TileType.GROUND))
                        {
                            tuile = (GameObject)Instantiate(Resources.Load(BLOC_STRAIGHT), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 270, 0));
                        }
                        else if (TopIs(room.Table, w, l, TileType.GROUND))
                        {
                            tuile = (GameObject)Instantiate(Resources.Load(BLOC_STRAIGHT), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 90, 0));
                        }
                        else if (RightIs(room.Table, w, l, TileType.GROUND))
                        {
                            tuile = (GameObject)Instantiate(Resources.Load(BLOC_STRAIGHT), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 180, 0));
                        }
                        else if (LeftIs(room.Table, w, l, TileType.GROUND))
                        {
                            tuile = (GameObject)Instantiate(Resources.Load(BLOC_STRAIGHT), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 0, 0));
                        }
                    }
                }
                else if (ThisOneIs(room.Table, w, l, TileType.DOOR))
                {
                    if (TopIs(room.Table, w, l, TileType.WALL))
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_DOOR), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 90, 0));
                    }
                    else
                    {
                        tuile = (GameObject)Instantiate(Resources.Load(BLOC_DOOR), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 0, 0));
                    }
                }
                else if (ThisOneIs(room.Table, w, l, TileType.GROUND))
                {
                    tuile = (GameObject)Instantiate(Resources.Load(BLOC_GROUND), new Vector3(BLOC_SIZE * w, 0, -BLOC_SIZE * l), Quaternion.Euler(0, 0, 0));
                }

                if(tuile!=null)
                    tuile.transform.SetParent(roomGO.transform);
            }
        }
        roomGO.transform.position = new Vector3(BLOC_SIZE * room.posInHangar.x, 0, -BLOC_SIZE * room.posInHangar.y);

        return roomGO;
    }

    private static bool ThisOneIs(List<List<int>> tab, int actualWidth, int actualLength, TileType type)
    {
        return (tab[actualLength][actualWidth] == (int)type);
    }
    private static bool LeftIs(List<List<int>> tab, int actualWidth, int actualLength, TileType type)
    {
        return ((actualWidth - 1 >= 0 && tab[actualLength][actualWidth - 1] == (int)type));
    }
    private static bool RightIs(List<List<int>> tab, int actualWidth, int actualLength, TileType type)
    {
        return ((actualWidth + 1 <= tab[actualLength].Count - 1 && tab[actualLength][actualWidth + 1] == (int)type));
    }
    private static bool TopIs(List<List<int>> tab, int actualWidth, int actualLength, TileType type)
    {
        return ((actualLength - 1 >= 0 && tab[actualLength - 1][actualWidth] == (int)type));
    }
    private static bool BottomIs(List<List<int>> tab, int actualWidth, int actualLength, TileType type)
    {
        return ((actualLength + 1 <= tab.Count - 1 && tab[actualLength + 1][actualWidth] == (int)type));
    }
}