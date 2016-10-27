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

    public Vector3 scaleRoom; //Facteur de multiplication de l'échelle de la salle

    //parametres des salles
    private int nbRoom = 0;
    private int widthRooms = 0;
    private int lengthRooms = 0;
    private int nbSolRooms = 0;

    private List<List<List<int>>> allRooms = new List<List<List<int>>>(); // Stock toutes les salles sous forme d'un tableau à deux dimensions ou les 1 sont des murs et les 4 des sols
    private List<List<List<int>>> allRoomDetails = new List<List<List<int>>>(); // Stock le détail de chacune des salles en fonction des tuiles placées (1-straight 2-Corridor ...)
    private List<List<List<int>>> allRoomsDecors = new List<List<List<int>>>(); // Stock le détail des decors chacune des salles  (1-pillier 2-Corner)
    private List<List<List<GameObject>>> allRoomsTilesGameObjects = new List<List<List<GameObject>>>(); //Stock toutes les tuiles de toutes les salles (GameObject)
    private List<GameObject> allRoomsGameObject = new List<GameObject>(); // Stockes toutes les salle (pour les désactiver par la suite)
    private List<List<Coord>> coordPortalsBeginEnd = new List<List<Coord>>(); // Stocke les coordonnée x y de tous les portals de toutes les salles (util pour la décoration)
    private List<List<GameObject>> portalBeginEndGameObject = new List<List<GameObject>>(); // stocke les position global de tous les portals afin de TP le joueur dessus (offset 0 4 0)

    private Coord posDepart; //position dans le tableau de l'emplacement de départ
    private GameObject spawnPoint; //GameObject où le joueur doit spawn

    private int playerRoom = 0;

    private const float TAILLE_TUILE = 7.2f;

    private MiniMap miniMap;

    // Use this for initialization
    void Start()
    {
        nbRoom = (nbRoom == 0) ? 3 : nbRoom;
        widthRooms = (widthRooms == 0) ? 10 : widthRooms;
        lengthRooms = (lengthRooms == 0) ? 15 : lengthRooms;
        nbSolRooms = (nbSolRooms == 0) ? 11 : nbSolRooms;

        //Génération de la premeire salle + instantiation du joueur et de la caméra
        GenerateRoom(widthRooms, lengthRooms, nbSolRooms, new Vector3(0, 0, 0), true, false);
        Instantiate(player, spawnPoint.transform.position + new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
        Instantiate(cam);

        Vector3 offset = new Vector3(0, 0, 0); //les salles sont les une au dessus des autres
        for(int i = 0; i < nbRoom-1; i++)
            GenerateRoom(widthRooms, lengthRooms, nbSolRooms, (i + 1) * offset, false, ((nbRoom - 2) == i)); //((nbRoom-2)==i) -> false sauf pour la derniere salle


        //Optimisation : on affiche que la salle dans laquelle le player se trouve
        for (int i = 1; i < nbRoom; i++)
            allRoomsGameObject[i].SetActive(false);

        //miniMap = GameObject.FindWithTag("MiniMap").GetComponent<MiniMap>();

        getCenterCurrentRoom(playerRoom);
    }


    public void GenerateRoom(int width, int length, int nbSol, Vector3 posRoom, bool firstRoom, bool lastRoom)
    {
        List<List<int>> room = CreateRoom(width, length, nbSol); 
        GameObject gameObjectRoom = InstantiateRoom(room, width + 1, length + 2, firstRoom, lastRoom);
        decorateRoom(allRoomDetails[allRoomDetails.Count - 1], new Vector3(0, 0, 0), gameObjectRoom);

        allRooms.Add(room);
        allRoomsGameObject.Add(gameObjectRoom);

        //Placement et redimensionnement
        gameObjectRoom.transform.position = posRoom;
        gameObjectRoom.transform.localScale = scaleRoom;

        GetComponent<EnemyBehaviour>().PlaceEnemys(gameObjectRoom, allRoomsTilesGameObjects[allRoomsTilesGameObjects.Count - 1], room, allRoomsDecors[allRoomsDecors.Count - 1]);
    }

    List<List<int>> CreateRoom(int width, int length, int nbSolWanted)
    {
        List<List<int>> room = new List<List<int>>();

        int nbSol = 0;
        room.Clear();
        for (int wid = 0; wid < width; wid++)
        {
            List<int> temp = new List<int>();
            for (int leng = 0; leng < length; leng++)
                temp.Add(0);
            room.Add(temp);
        }

        //On place le début sur 3 tuiles en bas ainsi qu'un sol en face de la tuile début milieu
        int rand = Random.Range(2, length - 3);

        room[width - 1][rand] = 1; room[width - 1][rand - 1] = 1; room[width - 1][rand + 1] = 1;
        room[width - 2][rand] = 4; room[width - 2][rand - 1] = 1; room[width - 2][rand + 1] = 1;
        room[width - 3][rand] = 4; //sol au dessus pour débuter l'algorithme

        nbSol = 2; //début + au dessus

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
        posDepart.x++; //le départ est donc décalé à droite


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
        return room;
    }


    /* 0 Void
     * 1 WallStraight
     * 2 Corridor
     * 3 WallAngleExt
     * 4 WallAngleInt
     * 5 CorridorCorner
     * 6 TWall
     * 7 Ground */
    GameObject InstantiateRoom(List<List<int>> room, int width, int length, bool firstRoom, bool lastRoom)
    {
        GameObject roomGO = new GameObject("room");
        roomGO.transform.position = new Vector3(0, 0, 0);
        GameObject tuile = null;

        Coord posFin;
        posFin.x = 0;
        posFin.y = 0;

        List<List<GameObject>> roomGameObject = new List<List<GameObject>>();
        List<List<int>> roomDetail = new List<List<int>>();
        List<Coord> portalBeginEnd = new List<Coord>();

        //On place les tuiles correctement (Straight, Corner ...) : 
        for (int wid = 0; wid < width; wid++)
        {
            List<int> line = new List<int>();
            List<GameObject> roomGameObjectLine = new List<GameObject>();
            for (int leng = 0; leng < length; leng++)
            {
                if (room[wid][leng] == 1) //Mur
                {
                    //STRAIGHT
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] !=1 ) && (leng - 1 >= 0 && room[wid][leng - 1] !=1 )) //haut bas ET pas gauche pas droite
                    {
                        if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 4)) // Si le sol est  à droite
                            if ((leng - 1 >= 0 && room[wid][leng - 1] == 4)) //Et a droite
                            {
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Corridor"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                                line.Add(2);
                            }
                            else
                            {
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                                line.Add(1);
                            }
                        else //SI le sol est à gauche
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, -90, 0));
                            line.Add(1);
                        }
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1)  &&  (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) //droite gauche ET pas haut pas bas
                    {
                        if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 4)) // Si le sol est  en bas
                            if ((wid - 1>= 0 && room[wid - 1][leng] == 4))
                            {
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Corridor"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                                line.Add(2);
                            }
                            else
                            {
                                tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                                line.Add(1);
                            }
                        else // SI le sol est en haut
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 0, 0));
                            line.Add(1);
                        }
                    }

                    //CORNER
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1)  &&  (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))) //droite et bas ET pas haut pas gauche
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est à gauche -> coin ext
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 0, 0));
                            line.Add(3);
                        }
                        else //le sol est en haut a gauche
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                            line.Add(4);
                        }
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1))) //gauche et haut ET pas droite et pas bas
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est a droite -> coin ext
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                            line.Add(3);
                        }
                        else //le sol est en haut a gauche
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 0, 0));
                            line.Add(4);
                        }
                    }
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)  &&  (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1))) //droite et haut ET pas auche et pas bas
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est da gauche -> coin ext
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 270, 0));
                            line.Add(3);
                        }
                        else //le sol est en haut a droite
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                            line.Add(4);
                        }
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1)  &&  (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))) //gauhe et bas ET pas haut et pas droite
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est da droite -> coin ext
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                            line.Add(3);
                        }
                        else //le sol est en haut a droite
                        {
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 270, 0));
                            line.Add(4);
                        }
                    }

                    //Corridor
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1))
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 0, 0));
                        line.Add(5);
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) 
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                        line.Add(5);
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) 
                    {
                        /*ok*/
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                        line.Add(5);
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1))
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/CorridorCorner"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 270, 0));
                        line.Add(5);
                    }

                    //TWall
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1) && (leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] != 1)) //haut bas droite pas gauche
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 90, 0));
                        line.Add(6);
                    }
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1) && (leng + 1 <= length - 1 && room[wid][leng + 1] != 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1)) //haut bas pas droite gauche
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 270, 0));
                        line.Add(6);
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] != 1)) //droite gauche pas haut bas
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 180, 0));
                        line.Add(6);
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] != 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)) //droite gauche haut pas bas
                    {
                        tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/TWall"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), Quaternion.Euler(0, 0, 0));
                        line.Add(6);
                    }
                }
                else if (room[wid][leng] == 4) //Ground
                {
                    tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Ground"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid), new Quaternion(0, 0, 0, 0));
                    line.Add(7);
                }
                else
                {
                    line.Add(0);
                    tuile = null;
                }

                roomGameObjectLine.Add(tuile);
                
                if(tuile != null)
                    tuile.transform.SetParent(roomGO.transform);
            }
            roomGameObject.Add(roomGameObjectLine);
            roomDetail.Add(line);
        }

        allRoomsTilesGameObjects.Add(roomGameObject);

        portalBeginEnd.Add(posDepart); //On ajoute la position de départ (peu importe si spawnPoint ou départ)
        List<GameObject> portalsOfheRoom = new List<GameObject>();
        //Placer l'élément de début sur la tuile de début (Spawn Point ou Portal)
        if (firstRoom)
        {
            GameObject spawnPointGO = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/SpawnPoint"), new Vector3(TAILLE_TUILE * (posDepart.x), 0, -TAILLE_TUILE * posDepart.y - TAILLE_TUILE/2), Quaternion.Euler(0, 135, 0));
            spawnPointGO.transform.SetParent(roomGO.transform);
            spawnPoint = spawnPointGO;
        }
        else
        {
            GameObject portalBeginning = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Portal"), new Vector3(TAILLE_TUILE * (posDepart.x), 0, -TAILLE_TUILE * posDepart.y - TAILLE_TUILE / 2), Quaternion.Euler(0, 0, 0));
            portalBeginning.transform.SetParent(roomGO.transform);
            portalBeginning.GetComponentInChildren<PortalBehaviour>().direction = -1;
            portalBeginning.GetComponentInChildren<PortalBehaviour>().wolrdBehaviour = this.GetComponent<WorldBehaviour>();
            portalBeginning.GetComponentInChildren<PortalBehaviour>().usable = false;
            portalsOfheRoom.Add(portalBeginning);
        }

        //Placer l'élément de fin
        //Recherche de la case la plus éloignée
        float distance = 0;
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                if (room[wid][leng] == 1 || room[wid][leng] == 4) //Mur ou sol
                {
                    float temp = Mathf.Sqrt((wid - posDepart.y) * (wid - posDepart.y) + (leng - posDepart.x)*(leng - posDepart.x));
                    if (temp > distance)
                    {
                        distance = temp;
                        posFin.y = wid;
                        posFin.x = leng;
                    }
                }
            }
        }

        portalBeginEnd.Add(posFin); //On ajoute la position de fin

        //On place le point de fin en fct de la tuile et de son orientation (sachant que l'objet de fin ne peut pas être sur tous les types de tuiles)
        int OFFSET = (int) TAILLE_TUILE / 2;
        Vector3 offset = new Vector3(0, 0, 0); ;
        //Clip l'angle sur une valeur exact
        float previousAngle = roomGameObject[posFin.y][posFin.x].transform.eulerAngles.y;
        int angle = (previousAngle > 80 && previousAngle < 100) ? 90 : (previousAngle > 170 && previousAngle < 190) ? 180 : (previousAngle > 260 && previousAngle < 280) ? 270 : 0;

        switch (roomDetail[posFin.y][posFin.x])
        {
            case 1: //wall straight
                if (angle == 0)
                    offset = new Vector3(0, 0, OFFSET);
                else if (angle == 90)
                    offset = new Vector3(OFFSET, 0, 0);
                else if (angle == 180)
                    offset = new Vector3(0, 0, -OFFSET);
                else if (angle == 270)
                    offset = new Vector3(-OFFSET, 0, 0);
                break;
            case 2://corridor
                break;
            case 3://wallAngleExt
                if (angle == 0)
                    offset = new Vector3(-OFFSET, 0, OFFSET);
                else if (angle == 90)
                    offset = new Vector3(OFFSET, 0, OFFSET);
                else if (angle == 180)
                    offset = new Vector3(OFFSET, 0, -OFFSET);
                else if (angle == 270)
                    offset = new Vector3(-OFFSET, 0, -OFFSET);
                break;
            case 4://wallAngleInt
                if (angle == 0)
                    offset = new Vector3(-OFFSET, 0, OFFSET);
                else if (angle == 90)
                    offset = new Vector3(OFFSET, 0, OFFSET);
                else if (angle == 180)
                    offset = new Vector3(OFFSET, 0, -OFFSET);
                else if (angle == 270)
                    offset = new Vector3(-OFFSET, 0, -OFFSET);
                break;
            case 5://CorridorCorner
                break;
            case 6://TWall
                break;
            case 7://Ground
                break;
        }

        GameObject portalEnd = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Portal"), new Vector3(TAILLE_TUILE * posFin.x, 0, -TAILLE_TUILE * posFin.y) + offset, Quaternion.Euler(0, 0, 0));
        portalEnd.transform.SetParent(roomGO.transform);
        portalEnd.GetComponentInChildren<PortalBehaviour>().direction = 1;
        portalEnd.GetComponentInChildren<PortalBehaviour>().wolrdBehaviour = this.GetComponent<WorldBehaviour>();
        portalEnd.GetComponentInChildren<PortalBehaviour>().usable = true;
        portalEnd.GetComponentInChildren<PortalBehaviour>().lastPortal = lastRoom;
        portalsOfheRoom.Add(portalEnd);

        portalBeginEndGameObject.Add(portalsOfheRoom);
        allRoomDetails.Add(roomDetail);
        coordPortalsBeginEnd.Add(portalBeginEnd);

        return roomGO;
    }


    /* room -> roomDetail (un numéro par type de tuile)
     * 1 -> pilliers
     * 2 - > corner
     */ 
    void decorateRoom(List<List<int>> room, Vector3 offsetRoom, GameObject gameObjectRoom)
    {
        //Recehrche de patterns :

        // sol sol -> chance de mettre un pillier au centre
        // sol sol
        int width = room.Count;
        int length = room[0].Count;

        //liste de déors pour vérifier qu'il n'y en ai pas autour (0 -> libre   1 -> occupé)
        List<List<int>> decors = new List<List<int>>();
        for (int wid = 0; wid < width; wid++)
        {
            List<int> decorsLine = new List<int>();
            for (int leng = 0; leng < length; leng++)
                decorsLine.Add(0);
            decors.Add(decorsLine);
        }
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x] = 3; //posDebut
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x + 1] = 3; 
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x - 1] = 3; 
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y + 1][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x] = 3; 
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y + 1][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x - 1] = 3; 
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y + 1][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x + 1] = 3; 
        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].y - 1][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][0].x ] = 3; 

        decors[coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][1].y][coordPortalsBeginEnd[coordPortalsBeginEnd.Count - 1][1].x] = 4; //posFin

        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                //4 Sols en carré -> pillar
                if (room[wid][leng] == 7) //case actuel = sol
                {
                    if (wid - 1 >= 0 && room[wid - 1][leng] == 7) // haut = sol
                    {
                        if (leng + 1 <= length - 1 && room[wid][leng + 1] == 7) // droite  = sol
                        {
                            if (room[wid - 1][leng + 1] == 7) // haut droite  = sol
                            {
                                //y a t'il des pilliers autour ?
                                if (wid + 1 <= width - 1 && decors[wid - 1][leng] == 0 && decors[wid + 1][leng] == 0 && decors[wid][leng - 1] == 0 && decors[wid][leng + 1] == 0)
                                {
                                    GameObject pillar = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/Pillar"), new Vector3(TAILLE_TUILE * leng + TAILLE_TUILE / 2, 0, -TAILLE_TUILE * wid + TAILLE_TUILE / 2) + offsetRoom, Quaternion.Euler(0, 0, 0));
                                    pillar.transform.SetParent(gameObjectRoom.transform);
                                    //print("wid : " + wid + "   leng : " + leng);
                                    decors[wid][leng] = 1;
                                }
                            }
                        }
                    }
                }

                // coin straight coin (vertical ou horizontal) -> DoubleCorner
                bool doubleCornerOK = false;
                if (room[wid][leng] == 1 && decors[wid][leng] == 0) //case actuel = straight 
                {
                    if (wid - 1 >= 0 && room[wid - 1][leng] == 4 && decors[wid - 1][leng] == 0) // haut = angleInt  et ni début ni fin
                    {
                        if (wid + 1 <= width - 1 && room[wid + 1][leng] == 4 && decors[wid + 1][leng] == 0)  // bas  = angleInt  et ni début ni fin
                        {
                            doubleCornerOK = true;

                            decors[wid][leng] = 2;
                            decors[wid + 1][leng] = 2;
                            decors[wid - 1][leng] = 2;
                        }
                    }
                    else if (leng - 1 >= 0 && room[wid][leng - 1] == 4 && decors[wid][leng - 1] == 0) // droite = angleInt  et ni début ni fin
                    {
                        if (leng + 1 <= length - 1 && room[wid][leng + 1] == 4 && decors[wid][leng + 1] == 0)  // gauche  = angleInt  et ni début ni fin
                        {
                            doubleCornerOK = true;

                            decors[wid][leng] = 2;
                            decors[wid][leng + 1] = 2;
                            decors[wid][leng - 1] = 2;
                        }
                    }
                }
                if (doubleCornerOK)
                {
                    GameObject doubleCorner = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/DoubleCorner"), new Vector3(TAILLE_TUILE * leng, 0, -TAILLE_TUILE * wid) + offsetRoom, allRoomsTilesGameObjects[allRoomsTilesGameObjects.Count - 1][wid][leng].transform.rotation);
                    doubleCorner.transform.SetParent(gameObjectRoom.transform);
                }
            }
        }
        allRoomsDecors.Add(decors);
    }

    /*Initialise les paramètre de la salle
     */
    public void setRoomSize(int p_nbRoom, int w, int l, int nbSol)
    {
        nbRoom = p_nbRoom;
        widthRooms = w;
        lengthRooms = l;
        nbSolRooms = nbSol;
    }

    /* direction = 1 -> salle suivante
     * direction = -1 -> salle précédente
     */ 
    public void changeRoom(int direction)
    {
        int temp = playerRoom + direction;

        if (temp != playerRoom)
        {
            playerRoom = temp;
            allRoomsGameObject[playerRoom].SetActive(true);
            GameObject.FindWithTag("Player").transform.position = portalBeginEndGameObject[playerRoom][0].transform.position + new Vector3(0, 4, 0);
            allRoomsGameObject[playerRoom - direction].SetActive(false);

            getCenterCurrentRoom(playerRoom);
        }
    }

    public void getCenterCurrentRoom(int currentRoom)
    {
        int widthMax = 0;
        int lengthMax = 0;
        int centerWidthFinal = 0;
        int centerLengthFinal = 0;

        for (int wid = 0; wid < allRooms[playerRoom].Count; wid++)
        {
            int lengthCurrent = 0;
            int centerLengthCurrent = 0;
            for (int leng = 0; leng < allRooms[playerRoom][0].Count; leng++)
            {
                if (allRooms[playerRoom][wid][leng] == 1 || allRooms[playerRoom][wid][leng] == 4) //Si il y a quelque chose
                {
                    lengthCurrent++;
                    if (allRooms[playerRoom][wid][leng - 1] == 0) //Premeir de la rangée
                    {
                        centerLengthCurrent = leng;
                    }
                }
            }
            if (lengthCurrent > lengthMax)
            {
                lengthMax = lengthCurrent;
                centerLengthFinal = centerLengthCurrent + lengthMax / 2;
            }
            lengthCurrent = 0;
        }


        for (int leng = 0; leng < allRooms[playerRoom][0].Count; leng++)
        {
            int widthCurrent = 0;
            int centerWidthCurrent = 0;
            for (int wid = 0; wid < allRooms[playerRoom].Count; wid++)
            {
                if (allRooms[playerRoom][wid][leng] == 1 || allRooms[playerRoom][wid][leng] == 4) //Si il y a quelque chose
                {
                    widthCurrent++;
                    if (allRooms[playerRoom][wid - 1][leng] == 0) //Premeir de la colonne
                    {
                        centerWidthCurrent = wid;
                    }
                }
            }
            if (widthCurrent > widthMax)
            {
                widthMax = widthCurrent;
                centerWidthFinal = centerWidthCurrent + widthMax / 2;
            }
            widthCurrent = 0;
        }

        //printTable(allRooms[playerRoom]);
        //print ("length : " + lengthMax + "  width : " + widthMax + "  centerX : " + centerLengthFinal + "  centerY : " + centerWidthFinal);


        //Vector3 centre = new Vector3(centerLengthFinal*TAILLE_TUILE,0,  -centerWidthFinal*TAILLE_TUILE);
        //float size = Mathf.Max(lengthMax, widthMax);

        //miniMap.ChangePositionAndSize(centre, size);
    }



    /* Fonction d'afficahge d'un tableau a deux dimension 
     * sous forme de List<List<int>>
     */ 
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
