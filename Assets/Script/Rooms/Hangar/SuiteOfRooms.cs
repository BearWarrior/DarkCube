using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiteOfRoom
{
    private const int NB_ROOM = 4;
    private const int WIDTH_MIN = 3;
    private const int WIDTH_MAX = 5;
    private const int LENGTH_MIN = 3;
    private const int LENGTH_MAX = 5;

    private List<List<int>> Map = new List<List<int>>();
    public List<Room> rooms = new List<Room>();

    private Vector2 ActualGlobalExitDoorPos = new Vector2(-1, -1);
    private bool firstRoom = true;

    private bool IsMapOk = true;

    private void Initialize()
    {
        Map.Clear();
        rooms.Clear();
        for (int l = 0; l < (2 * NB_ROOM) * LENGTH_MAX; l++)
        {
            List<int> line = new List<int>();
            for (int w = 0; w < (2 * NB_ROOM) * LENGTH_MAX; w++)
                line.Add(0);
            Map.Add(line);
        }
        firstRoom = true;
        ActualGlobalExitDoorPos = new Vector2(-1, -1);
    }

    public void Generate()
    {
        do
        {
            Initialize();

            Room room = new Room();
            room.Generate(Random.Range(WIDTH_MIN, WIDTH_MAX + 1), Random.Range(LENGTH_MIN, LENGTH_MAX + 1));
            room.setExitDoor();
            rooms.Add(room);
            IsMapOk = PlaceRoomInMap(room);
            for (int cpt = 1; cpt < NB_ROOM; cpt++)
            {
                room = new Room();
                room.Generate(Random.Range(WIDTH_MIN, WIDTH_MAX + 1), Random.Range(LENGTH_MIN, LENGTH_MAX + 1));
                var previousRoomExitDoorCardinality = rooms[rooms.Count - 1].ExitDoorCardinal;
                room.setEntryDoor(EnumScript.getOppositeCard(previousRoomExitDoorCardinality));
                if (cpt != NB_ROOM - 1)
                    room.setExitDoor();
                rooms.Add(room);
                IsMapOk = PlaceRoomInMap(room);

                if (!IsMapOk)
                    break;
            }

            ReduceMapAndApplyChangeToRoom();
        }
        while (!IsMapOk);
    }

    private bool PlaceRoomInMap(Room room)
    {
        Vector2 position = new Vector2();
        if (ActualGlobalExitDoorPos.x == -1) 
            position = new Vector2((Map[0].Count / 2) - (room.Table[0].Count / 2), (Map.Count / 2) - (room.Table.Count / 2));
        else 
            position = ActualGlobalExitDoorPos - room.EntryDoorPos;

        for (int l = 0; l < room.Table.Count; l++)
        {
            for (int w = 0; w < room.Table[0].Count; w++)
            {
                if (Map[(int)position.y + l][(int)position.x + w] == 9)
                    return false; //Room are on top of each other -> let's start again Wouhou
                else
                    Map[(int)position.y + l][(int)position.x + w] = room.Table[l][w];
            }
        }

        room.posInSuite = position;
        ActualGlobalExitDoorPos = position + room.ExitDoorPos;

        return true;
    }

    public void ReduceMapAndApplyChangeToRoom()
    {
        Vector2 offset = new Vector2();
        offset.y = ReduceTopLinesExceptOne();
        ReduceBottomLinesExceptOne();
        TransposeMatrixMap();
        offset.x = ReduceTopLinesExceptOne();
        ReduceBottomLinesExceptOne();
        TransposeMatrixMap();
        
        foreach(Room room in rooms)
            room.UpdatePosInSuite(offset);
    }

    public void TransposeMatrixMap()
    {
        List<List<int>> newMatrice = new List<List<int>>();
        for (int i = 0; i < Map[0].Count; i++)
        {
            List<int> newLine = new List<int>();
            for (int j = 0; j < Map.Count; j++)
                newLine.Add(Map[j][i]);
            newMatrice.Add(newLine);
        }
        Map = newMatrice;
    }

    public int ReduceTopLinesExceptOne()
    {
        List<int> lineOfZero = Map[0];
        int nbLineRemoved = 0;
        bool keep = false;
        while (!keep)
        {
            keep = false;
            for (int w = 0; w < Map[0].Count; w++)
            {
                if (Map[0][w] != 0)
                {
                    keep = true;
                    break;
                }
            }
            if (keep)
                break;
            else
            {
                Map.RemoveAt(0);
                nbLineRemoved++;
            }
        }

        Map.Insert(0, lineOfZero);
        nbLineRemoved--;
        
        return nbLineRemoved;
    }

    public void ReduceBottomLinesExceptOne()
    {
        List<int> lineOfZero = Map[0];
        bool keep = false;
        while (!keep)
        {
            keep = false;
            for (int w = 0; w < Map[Map.Count - 1].Count; w++)
            {
                if (Map[Map.Count - 1][w] != 0)
                {
                    keep = true;
                    break;
                }
            }
            if (keep)
                break;
            else
                Map.RemoveAt(Map.Count - 1);
        }

        Map.Insert(Map.Count, lineOfZero);
    }

    public void Display()
    {
        string affichage = "";
        for (int wid = 0; wid < Map.Count; wid++)
        {
            for (int leng = 0; leng < Map[wid].Count; leng++)
                affichage += Map[wid][leng];
            affichage += '\n';
        }
        Debug.Log(affichage);
    }

    public static void printTable(List<List<int>> table)
    {
        string affichage = "";
        for (int wid = 0; wid < table.Count; wid++)
        {
            for (int leng = 0; leng < table[wid].Count; leng++)
                affichage += table[wid][leng];
            affichage += '\n';
        }
        Debug.Log(affichage);
    }
}
