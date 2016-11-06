using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour
{
    public List<List<GameObject>> listEnemy = new List<List<GameObject>>(); //List d'enemy par salle

    public void PlaceEnemys(GameObject roomGO, List<List<GameObject>> tiles, List<List<int>> room, List<List<int>> decors, int numRoom)
    {
        int ratio = 3;
        int valActuel = 1;
        List<GameObject> listEnemyInRoom = new List<GameObject>();

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
                                    enemy.transform.tag = "Enemy";
                                    valActuel = 1;
                                    enemy.GetComponent<Enemy>().numRoom = numRoom;

                                    listEnemyInRoom.Add(enemy);
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

        listEnemy.Add(listEnemyInRoom);
    }

    public void enemyDied(GameObject en)
    {
        int i = 0;
        listEnemy[en.GetComponent<Enemy>().numRoom].Remove(en);

        foreach(List<GameObject> list in listEnemy)
        {
            //print(i + "   " + list.Count);
            if (list.Count == 0) //Si une salle n'a plus d'enemy, on active le TP
            {
                GetComponent<WorldBehaviour>().activePortalEnd(i);
            }
            i++;
        }
    }


}
