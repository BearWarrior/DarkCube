using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    internal enum TileType { GROUND = 9, WALL = 1, DOOR = 2 };

    public List<List<int>> Table { get; set; }
    public EnumScript.Cardinal EntryDoorCardinal { get; set; }
    public EnumScript.Cardinal ExitDoorCardinal { get; set; }
    public Vector2 EntryDoorPos { get; set; }
    public Vector2 ExitDoorPos { get; set; }
    public Vector2 posInSuite { get; set; }

    public Room()
    {
        Table = new List<List<int>>();
        EntryDoorCardinal = EnumScript.Cardinal.NONE;
        ExitDoorCardinal = EnumScript.Cardinal.NONE;
    }

    public void Generate(int width, int length)
    {
        Table = GenerateSquare(width, length);
    }

    private static List<List<int>> GenerateSquare(int width, int length)
    {
        List<List<int>> room = new List<List<int>>();
        for (int l = 0; l < length; l++)
        {
            List<int> line = new List<int>();
            for (int w = 0; w < width; w++)
            {
                if (w == 0 || l == 0 || w == width - 1 || l == length - 1)
                    line.Add((int)TileType.WALL);
                else
                    line.Add((int)TileType.GROUND);
            }
            room.Add(line);
        }
        return room;
    }

    public void setEntryDoor(EnumScript.Cardinal wantedCardinality)
    {
        if (wantedCardinality == EnumScript.Cardinal.NORTH)
            EntryDoorPos = new Vector2(Random.Range(1, Table[0].Count - 1 - 1), 0);
        else if (wantedCardinality == EnumScript.Cardinal.EAST)
            EntryDoorPos = new Vector2(Table[0].Count - 1, Random.Range(1, Table.Count - 1 - 1));
        else if (wantedCardinality == EnumScript.Cardinal.SOUTH)
            EntryDoorPos = new Vector2(Random.Range(1, Table[0].Count - 1 - 1), Table.Count - 1);
        else if (wantedCardinality == EnumScript.Cardinal.WEST)
            EntryDoorPos = new Vector2(0, Random.Range(1, Table.Count - 1 - 1));
        EntryDoorCardinal = wantedCardinality;
        Table[(int)EntryDoorPos.y][(int)EntryDoorPos.x] = (int)TileType.DOOR;
    }

    public void setExitDoor()
    {
        var possibleSides = new List<EnumScript.Cardinal>();
        EnumScript.Cardinal forbidenCard = EntryDoorCardinal;
        foreach (EnumScript.Cardinal c in EnumScript.Cardinal.GetValues(typeof(EnumScript.Cardinal)))
            if (c != forbidenCard && c != EnumScript.Cardinal.NONE)
                possibleSides.Add(c);
        EnumScript.Cardinal chosenSide = possibleSides[Random.Range(0, possibleSides.Count)];
        if (chosenSide == EnumScript.Cardinal.NORTH)
            ExitDoorPos = new Vector2(Random.Range(1, Table[0].Count - 1 - 1), 0);
        else if (chosenSide == EnumScript.Cardinal.EAST)
            ExitDoorPos = new Vector2(Table[0].Count - 1, Random.Range(1, Table.Count - 1 - 1));
        else if (chosenSide == EnumScript.Cardinal.SOUTH)
            ExitDoorPos = new Vector2(Random.Range(1, Table[0].Count - 1 - 1), Table.Count - 1);
        else /*if (chosenSide == Card.WEST)*/
            ExitDoorPos = new Vector2(0, Random.Range(1, Table.Count - 1 - 1));
        Table[(int)ExitDoorPos.y][(int)ExitDoorPos.x] = (int)TileType.DOOR;
        ExitDoorCardinal = chosenSide;
    }

    public void Display()
    {
        string affichage = "";
        for (int wid = 0; wid < Table.Count; wid++)
        {
            for (int leng = 0; leng < Table[wid].Count; leng++)
                affichage += Table[wid][leng];
            affichage += '\n';
        }
        Debug.Log(affichage);
    }

    public void UpdatePosInSuite(Vector2 offset)
    {
        posInSuite -= offset;
    }
}
