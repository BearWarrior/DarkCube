using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    public void Generate()
    {
        //Generate square -> centralRoom
        //Generate 6 SuiteOfRoom (One per type of suite)
        //Choose where to put them (NESW)
        //Place them with a little space between each
        //Calculate corridor
        //calculate corridorDoor for each SuiteOfRooms
        //Giant 2-dim table 
        //Instantiate

        Room centralRoom = new Room();
        centralRoom.Generate(5, 3);

        List<SuiteOfRoom> suitesOfRoom = new List<SuiteOfRoom>();
        for(int i = 0; i < 6; i++)
        {
            SuiteOfRoom suite = new SuiteOfRoom();
            suite.Generate();
            suitesOfRoom.Add(suite);
        }
    }
}
