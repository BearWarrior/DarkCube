﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour
{
    public List<List<GameObject>> listEnemy; //List d'enemy par salle

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	       
	}

    public void PlaceEnemys(GameObject roomGO, List<List<GameObject>> tiles, List<List<int>> room, List<List<int>> decors)
    {
        int ratio = 3;
        int valActuel = 1;

        for (int wid = 0; wid < room.Count; wid++)
        {
            for (int leng = 0; leng < room[wid].Count; leng++)
            {
                if ((room[wid][leng] == 1 || room[wid][leng] == 4) && (decors[wid][leng] == 1 || decors[wid][leng] == 0)) //Si room = straight or ground AND decors = empty or pillier
                {
                    for (int enf = 0; enf < tiles[wid][leng].transform.childCount; enf++)
                    {
                        if(tiles[wid][leng].transform.GetChild(enf).tag == "SpawnPoint")
                        {
                            for (int i = 0; i < tiles[wid][leng].transform.GetChild(enf).childCount; i++)
                            {
                                if (valActuel == ratio)
                                {
                                    GameObject enemy = (GameObject)Instantiate(Resources.Load("Enemy/Agent"), tiles[wid][leng].transform.GetChild(enf).GetChild(i).transform.position, Quaternion.Euler(0, 0, 0));
                                    enemy.transform.SetParent(roomGO.transform);
                                    valActuel = 1;
                                }
                                else
                                {
                                    valActuel++;
                                }
                            }
                        }
                    }
                }
            }
        }


        //return roomGO;
    }
}
