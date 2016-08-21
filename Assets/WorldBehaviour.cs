using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class WorldBehaviour : MonoBehaviour
{
    struct Coord
    {
        public int x;
        public int y;
    }
    public GameObject player;
    public GameObject cam;
    public GameObject projector;

    List<List<int>> room = new List<List<int>>();
    Coord posDepart;
    Vector3 begining;


    // Use this for initialization
    void Start()
    {
        GenerateRoom(10, 15, 11);

        Instantiate(player, begining, new Quaternion(0, 0, 0, 0));
        Instantiate(cam);

        GenerateRoom(10, 15, 11);
    }

    public void GenerateRoom(int width, int length, int nbSol)
    {
        CreateRoom(width, length, nbSol);
        InstantiateRoom(width + 1, length + 2);
    }

    void CreateRoom(int width, int length, int nbSolWanted)
    {
        int nbSol = 0;
        room.Clear();
        for (int wid = 0; wid < width; wid++)
        {
            List<int> temp = new List<int>();
            for (int leng = 0; leng < length; leng++)
                temp.Add(0);
            room.Add(temp);
        }

        List<Coord> path = new List<Coord>();

        //On place le début sur 3 tuiles en bas ainsi qu'un sol en face de la tuile début milieu
        int rand = Random.Range(2, length - 3);

        room[width - 1][rand] = 1; room[width - 1][rand - 1] = 1; room[width - 1][rand + 1] = 1;
        room[width - 2][rand] = 3; room[width - 2][rand - 1] = 1; room[width - 2][rand + 1] = 1;
        room[width - 3][rand] = 4; //sol

        nbSol = 2;

        begining = new Vector3(rand * 36 + 36, 5, -(width - 1) * 36 + 36 / 2);

        posDepart.x = rand;
        posDepart.y = width - 2;

        Coord posActuel; posActuel.x = rand ; posActuel.y = width - 3;

        List<Coord> possibleTiles = new List<Coord>();
        List<Coord> lastTilesAdded = new List<Coord>();
        List<Coord> futurTilesToCheck = new List<Coord>();
        lastTilesAdded.Add(posActuel);


        //Place the ground
        while (nbSolWanted >= nbSol)
        {
            //For each tiles we we have just placed
            for (int tileAdded = 0; tileAdded < lastTilesAdded.Count; tileAdded++)
            {
                Coord posLeft; posLeft.x = lastTilesAdded[tileAdded].x - 1; posLeft.y = lastTilesAdded[tileAdded].y;
                Coord posRight; posRight.x = lastTilesAdded[tileAdded].x + 1; posRight.y = lastTilesAdded[tileAdded].y;
                Coord posUp; posUp.x = lastTilesAdded[tileAdded].x; posUp.y = lastTilesAdded[tileAdded].y - 1;
                Coord posDown; posDown.x = lastTilesAdded[tileAdded].x; posDown.y = lastTilesAdded[tileAdded].y + 1;

                //gauche    
                if (lastTilesAdded[tileAdded].x - 1 >= 0 && room[lastTilesAdded[tileAdded].y][lastTilesAdded[tileAdded].x - 1] == 0)
                {
                    if (lastTilesAdded[tileAdded].x - 2 >= 0)
                        if (lastTilesAdded[tileAdded].y - 1 >= 0)
                            if (lastTilesAdded[tileAdded].y + 1 <= width - 1)
                                possibleTiles.Add(posLeft);
                }
                //droite    
                if (lastTilesAdded[tileAdded].x + 1 <= length - 1 && room[lastTilesAdded[tileAdded].y][lastTilesAdded[tileAdded].x + 1] == 0)
                {
                    if (lastTilesAdded[tileAdded].x + 2 <= length - 1)
                        if (lastTilesAdded[tileAdded].y - 1 >= 0)
                            if (lastTilesAdded[tileAdded].y + 1 <= width - 1)
                                possibleTiles.Add(posRight);
                }
                //haut     
                if (lastTilesAdded[tileAdded].y - 1 >= 0 && room[lastTilesAdded[tileAdded].y - 1][lastTilesAdded[tileAdded].x] == 0)
                {
                    if (lastTilesAdded[tileAdded].y - 2 >= 0)
                        if (lastTilesAdded[tileAdded].x + 1 <= length - 1)
                            if (lastTilesAdded[tileAdded].x - 1 >= 0)
                                possibleTiles.Add(posUp);
                }
                //bas       
                if (lastTilesAdded[tileAdded].y + 1 <= width - 1 && room[lastTilesAdded[tileAdded].y + 1][lastTilesAdded[tileAdded].x] == 0)
                {
                    if (lastTilesAdded[tileAdded].y + 2 <= width - 1)
                        if (lastTilesAdded[tileAdded].x + 1 <= length - 1)
                            if (lastTilesAdded[tileAdded].x - 1 >= 0)
                                possibleTiles.Add(posDown);
                }

                int posTileCount = possibleTiles.Count - 1;
                posTileCount = (posTileCount == 0) ? 1 : posTileCount;
                for (int i = 0; i < posTileCount; i++)
                {
                    int tile = Random.Range(0, possibleTiles.Count);
                    room[possibleTiles[tile].y][possibleTiles[tile].x] = 4;
                    nbSol++;
                    futurTilesToCheck.Add(possibleTiles[tile]);
                    possibleTiles.RemoveAt(tile);
                }
                possibleTiles.Clear();
            }

            lastTilesAdded.Clear();
            for(int i = 0; i < futurTilesToCheck.Count; i++)
                lastTilesAdded.Add(futurTilesToCheck[i]);
            futurTilesToCheck.Clear();    
        }


   
        //Fill the blanks
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                //Si c'est un 0, on regarde dans les 4 directions, si on trouve un autre sol dans les 4, on remplace par un 8
                if (room[wid][leng] == 0)
                {
                    int nbMurAutour = 0;
                    //droite
                    for (int i = leng; i < length - 1; i++)
                    {
                        if (room[wid][i] == 4 || room[wid][i] == 1)
                        {
                            nbMurAutour++;
                            break;
                        }
                    } 
                    //gauche
                    for (int i = leng; i >= 0; i--)
                    {
                        if (room[wid][i] == 4 || room[wid][i] == 1)
                        {
                            nbMurAutour++;
                            break;
                        }
                    }
                    //haut
                    for (int i = wid; i >= 0; i--)
                    {
                        if (room[i][leng] == 4 || room[i][leng] == 1)
                        {
                            nbMurAutour++;
                            break;
                        }
                    }
                    //bas
                    for (int i = wid; i <= width - 1; i++)
                    {
                        if (room[i][leng] == 4 || room[i][leng] == 1)
                        {
                            nbMurAutour++;
                            break;
                        }
                    }

                    if(nbMurAutour == 4)
                        room[wid][leng] = 4;
                }
            }
        }


        //Add a margin (bottom left and right
        for (int i = 0; i < room.Count; i++)
        {
            room[i].Insert(0, 0);
            room[i].Insert(room[i].Count, 0);
        }
        room.Insert(room.Count, new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });


        //Circle with walls
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                //Pour chaque sol, on met tous les cases autours en murs si c'est un zéro
                if (room[wid][leng] == 4)
                {
                    room[wid + 1][leng] = (room[wid + 1][leng] == 0) ? 1 : room[wid + 1][leng];
                    room[wid + 1][leng + 1] = (room[wid + 1][leng + 1] == 0) ? 1 : room[wid + 1][leng + 1];
                    room[wid][leng + 1] = (room[wid][leng + 1] == 0) ? 1 : room[wid][leng + 1];
                    room[wid - 1][leng + 1] = (room[wid - 1][leng + 1] == 0) ? 1 : room[wid - 1][leng + 1];
                    room[wid - 1][leng] = (room[wid - 1][leng] == 0) ? 1 : room[wid - 1][leng];
                    room[wid - 1][leng - 1] = (room[wid - 1][leng - 1] == 0) ? 1 : room[wid - 1][leng - 1];
                    room[wid][leng - 1] = (room[wid][leng - 1] == 0) ? 1 : room[wid][leng - 1];
                    room[wid + 1][leng - 1] = (room[wid + 1][leng - 1] == 0) ? 1 : room[wid + 1][leng - 1];
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateRoom(int width, int length)
    {
        GameObject roomGO = new GameObject("salle");
        GameObject tuile = new GameObject();

        //Placer la tuile de début

        //Puis la noter comme sol
        room[posDepart.y][posDepart.x+1] = 4; //x+1 car on a rajouté une colonne

        //On place les tuiles correctement (Straight, Corner ...) : 
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                if (room[wid][leng] == 1) //Mur
                {
                    //STRAIGHT
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] !=1 ) && (leng - 1 >= 0 && room[wid][leng - 1] !=1 )) //haut bas ET pas gauche pas droite
                    {
                        if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 4)) // Si le sol est  à droite
                            if ((leng - 1 >= 0 && room[wid][leng - 1] == 4)) //Et a droite
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Corridor"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                            else
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                        else //SI le sol est à gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, -90, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1)  &&  (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) //droite gauche ET pas haut pas bas
                    {
                        if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 4)) // Si le sol est  en bas
                            if ((wid - 1>= 0 && room[wid - 1][leng] == 4))
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Corridor"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                            else
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                        else // SI le sol est en haut
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }

                    //CORNER
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1)  &&  (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))) //droite et bas ET pas haut pas gauche
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est à gauche -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                        else //le sol est en haut a gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1))) //gauche et haut ET pas droite et pas bas
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est a droite -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                        else //le sol est en haut a gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1))) //droite et haut ET pas auche et pas bas
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est da gauche -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
                        else //le sol est en haut a droite
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))) //gauhe et bas ET pas haut et pas droite
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est da droite -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                        else //le sol est en haut a droite
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
                    }

                    //Corridor
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) 
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) 
                    {
                        /*ok*/tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1))
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
                    }

                    //TWall
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1) && (leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1)) //haut bas droite pas gauche
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                    }
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1) && (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1)) //haut bas pas droite gauche
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) //droite gauche pas haut bas
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)) //droite gauche haut pas bas
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }
                }
                else if (room[wid][leng] == 4) //Ground
                {
                    tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Ground"), new Vector3(36 * leng, 0, -36 * wid), new Quaternion(0, 0, 0, 0));
                }

                tuile.transform.SetParent(roomGO.transform);
            }
        }
    }


    void printTable(List<List<int>> table)
    {
        string affichage = "";
        for (int wid = 0; wid < table.Count; wid++)
        {
            for (int leng = 0; leng < table[wid].Count; leng++)
                affichage += table[wid][leng];
            affichage += '\n';
        }
        print(affichage);
    }
}




//room = new List<List<int>> {
//    new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
//    new List<int> { 0, 1, 1, 1, 1, 1, 1, 1, 0 },
//    new List<int> { 0, 1, 0, 0, 0, 0, 0, 1, 0 },
//    new List<int> { 0, 1, 0, 1, 1, 1, 0, 1, 0 },
//    new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0 },
//    new List<int> { 0, 1, 1, 1, 0, 1, 0, 1, 0 },
//    new List<int> { 0, 0, 0, 0, 0, 1, 0, 1, 0 },
//    new List<int> { 0, 1, 1, 1, 1, 1, 0, 1, 0 },
//    new List<int> { 0, 1, 0, 0, 0, 0, 0, 1, 0 },
//    new List<int> { 0, 1, 1, 1, 1, 4, 1, 1, 0 },
//    new List<int> { 0, 0, 0, 0, 1, 1, 1, 0, 0 }
//};