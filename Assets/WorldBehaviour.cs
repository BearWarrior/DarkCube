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
        int cpt = 0;
        while (cpt < 10)
        {
            if (CreateRoom(7, 12))
            {
                break;
            }
            cpt++;
        }



        if (cpt < 10) //La génération a réussi
        {
            GenerateRoom(7, 12);
        }

        Instantiate(player, begining, new Quaternion(0, 0, 0, 0));
        Instantiate(cam);
        //Instantiate(projector);
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*  1- Mur
     *  2- Fin de parcours (gauche du début)
     *  3- Debut  
     *  4- Sol
     * -1- Vide
     */
    bool CreateRoom(int width, int length)
    {
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
        room[width - 2][rand] = 4; room[width - 2][rand - 1] = 1; room[width - 2][rand + 1] = 2;

        begining = new Vector3(rand * 36, 5, -(width - 1) * 36 + 36/2);

        posDepart.x = rand;
        posDepart.y = width - 2;

        Coord posActuel; posActuel.x = rand - 1; posActuel.y = width - 2;

        int cpt = 0;

        //On part du cote gauche
        while (cpt < 100) //Break en cas de fin de parcours
        {
            //On regarde les emplacements disponibles
            Coord posLeft; posLeft.x = posActuel.x - 1; posLeft.y = posActuel.y;
            Coord posRight; posRight.x = posActuel.x + 1; posRight.y = posActuel.y;
            Coord posUp; posUp.x = posActuel.x; posUp.y = posActuel.y - 1;
            Coord posDown; posDown.x = posActuel.x; posDown.y = posActuel.y + 1;

            List<Coord> solution = new List<Coord>();

            //Fin du chemin (case 2 a proximité)
            if ((posActuel.x - 1 >= 0 && room[posActuel.y][posActuel.x - 1] == 2) || (posActuel.x + 1 <= length - 1 && room[posActuel.y][posActuel.x + 1] == 2) || (posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x] == 2) || (posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x] == 2))
            {
                break;
            }

            int nbSolution = 0;
            if (posActuel.x - 1 >= 0 && room[posActuel.y][posActuel.x - 1] == 0 || posActuel.x - 1 >= 0 && room[posActuel.y][posActuel.x - 1] == 2) //Si on peut aller plus à gauche (ni le vide ni déjà un mur)
            {
                //Si cette position n'est pas a cote d'un mur (hors la position actuelle)
                if (posActuel.x - 2 >= 0 && room[posActuel.y][posActuel.x - 2] == 0 || posActuel.x - 2 >= 0 && room[posActuel.y][posActuel.x - 2] == 2)
                    if (posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x - 1] == 0 || posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x - 1] == 2)
                        if (posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x - 1] == 0 || posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x - 1] == 2)
                        {
                            nbSolution++;
                            solution.Add(posLeft);
                        }
            }
            if (posActuel.x + 1 <= length - 1 && room[posActuel.y][posActuel.x + 1] == 0 || posActuel.x + 1 <= length - 1 && room[posActuel.y][posActuel.x + 1] == 2) //Si on peut aller plus à droite (ni le vide ni déjà un mur)
            {
                //Si cette position n'est pas a cote d'un mur (hors la position actuelle)
                if (posActuel.x + 2 <= length - 1 && room[posActuel.y][posActuel.x + 2] == 0 || posActuel.x + 2 <= length - 1 && room[posActuel.y][posActuel.x + 2] == 2)
                    if (posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x + 1] == 0 || posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x + 1] == 2)
                        if (posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x + 1] == 0 || posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x + 1] == 2)
                        {
                            nbSolution++;
                            solution.Add(posRight);
                        }
            }
            if (posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x] == 0 || posActuel.y - 1 >= 0 && room[posActuel.y - 1][posActuel.x] == 2) //Si on peut aller plus en haut (ni le vide ni déjà un mur)
            {
                //Si cette position n'est pas a cote d'un mur (hors la position actuelle)
                if (posActuel.y - 2 >= 0 && room[posActuel.y - 2][posActuel.x] == 0 || posActuel.y - 2 >= 0 && room[posActuel.y - 2][posActuel.x] == 2)
                    if (posActuel.x + 1 <= length - 1 && room[posActuel.y - 1][posActuel.x + 1] == 0 || posActuel.x + 1 <= length - 1 && room[posActuel.y - 1][posActuel.x + 1] == 2)
                        if (posActuel.x - 1 >= 0 && room[posActuel.y - 1][posActuel.x - 1] == 0 || posActuel.x - 1 >= 0 && room[posActuel.y - 1][posActuel.x - 1] == 2)
                        {
                            nbSolution++;
                            solution.Add(posUp);
                        }
            }
            if (posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x] == 0 || posActuel.y + 1 <= width - 1 && room[posActuel.y + 1][posActuel.x] == 2) //Si on peut aller plus en bas (ni le vide ni déjà un mur)
            {
                //Si cette position n'est pas a cote d'un mur (hors la position actuelle)
                if (posActuel.y + 2 <= width - 1 && room[posActuel.y + 2][posActuel.x] == 0 || posActuel.y + 2 <= width - 1 && room[posActuel.y + 2][posActuel.x] == 2)
                    if (posActuel.x + 1 <= length - 1 && room[posActuel.y + 1][posActuel.x + 1] == 0 || posActuel.x + 1 <= length - 1 && room[posActuel.y + 1][posActuel.x + 1] == 2)
                        if (posActuel.x - 1 >= 0 && room[posActuel.y + 1][posActuel.x - 1] == 0 || posActuel.x - 1 >= 0 && room[posActuel.y + 1][posActuel.x - 1] == 2)
                        {
                            nbSolution++;
                            solution.Add(posDown);
                        }
            }

            if (nbSolution > 0) //Si une solution existe
            {
                int nextWall = Random.Range(0, nbSolution);
                room[solution[nextWall].y][solution[nextWall].x] = 1;
                path.Add(solution[nextWall]);
                posActuel = solution[nextWall];
            }
            else
            {
                //On revient en arriere
                path.RemoveAt(path.Count - 1);
                room[posActuel.y][posActuel.x] = 0;
                posActuel = path[path.Count - 1];
            }
            cpt++;
        }

        if (cpt < 100)
        {
            room[width - 2][rand + 1] = 1; //On place la tuile de fin comme une tuile mur (2->1)
            room[width - 2][rand] = 4; //On change la tuile de départ en tuile sol
            int nbSol = 0;

            nbSol = fillWithGrounds(width, length);

            if (nbSol > 10 && nbSol < 20)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    int fillWithGrounds(int width, int length)
    {
        int nbSol = 0;
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                //Si c'est un 0, on regarde dans les 4 directions, si on trouve un mur dans les 4, c'est que on doit mettre un sol
                if (room[wid][leng] == 0)
                {
                    print("Un zero");
                    bool murGauche = false; ;
                    bool murDroit = false; ;
                    bool murHaut = false; ;
                    bool murBas = false; ;

                    int nbMurG = 0;
                    int nbMurH = 0;
                    int nbMurD = 0;
                    int nbMurB = 0;

                    int posMur = -1;

                    bool previousWasWall = false;

                    //droite
                    posMur = -1;
                    for (int i = leng; i < length - 1; i++)
                    {
                        if (room[wid][i] == 1)
                        {
                            print("Droite");
                            murDroit = true;
                            nbMurD++;
                            posMur = (posMur == -1) ? i : posMur;
                            //break;
                        }
                    }
                    if (nbMurD > 1)
                    {
                        nbMurD = 1;
                        previousWasWall = true;
                        for (int i = posMur; i < length - 1; i++)
                        {
                            if (previousWasWall)//Si le dernier etait un mur
                            {
                                if (room[wid][i] == 4)//et que c'est un sol
                                {
                                    previousWasWall = false;
                                    break;
                                }
                            }
                            else//Si le dernier etait un sol
                            {
                                if (room[wid][i] == 1)//Et que c'est un mur
                                {
                                    nbMurD++;
                                    previousWasWall = true;
                                    break;
                                }
                            }
                        }
                    }
                    //bas
                    posMur = -1;
                    for (int i = wid; i <= width - 1; i++)
                    {
                        if (room[i][leng] == 1)
                        {
                            print("Bas");
                            murBas = true;
                            nbMurB++;
                            posMur = (posMur == -1) ? i : posMur;
                            //break;
                        }
                    }
                    if (nbMurB > 1)
                    {
                        nbMurB = 1;
                        previousWasWall = true;
                        for (int i = posMur; i <= width - 1; i++)
                        {
                            if (previousWasWall)//Si le dernier etait un mur
                            {
                                if (room[i][leng] == 4)//et que c'est un sol
                                {
                                    previousWasWall = false;
                                    break;
                                }
                            }
                            else//Si le dernier etait un sol
                            {
                                if (room[i][leng] == 1)//Et que c'est un mur
                                {
                                    nbMurB++;
                                    previousWasWall = true;
                                    break;
                                }
                            }

                        }
                    }
                    //gauche
                    posMur = -1;
                    for (int i = leng; i >= 0; i--)
                    {
                        if (room[wid][i] == 1)
                        {
                            print("Gauche");
                            murGauche = true;
                            nbMurG++;
                            posMur = (posMur == -1) ? i : posMur;
                        }
                    }
                    if (nbMurG > 1)
                    {
                        nbMurG = 1;
                        previousWasWall = true;
                        for (int i = posMur; i >= 0; i--)
                        {
                            if(previousWasWall)//Si le dernier etait un mur
                            {
                                if (room[wid][i] == 4)//et que c'est un sol
                                {
                                    previousWasWall = false;
                                    break;
                                }
                            }
                            else//Si le dernier etait un sol
                            {
                                if (room[wid][i] == 1)//Et que c'est un mur
                                {
                                    nbMurG++;
                                    previousWasWall = true;
                                    break;
                                }
                            }
                        }
                    }
                    //haut
                    posMur = -1;
                    for (int i = wid; i >= 0; i--)
                    {
                        if (room[i][leng] == 1)
                        {
                            print("Haut");
                            nbMurH++;
                            murHaut = true;
                            posMur = (posMur == -1) ? i : posMur;
                        }
                    }
                    if (nbMurH > 1)
                    {
                        nbMurH = 1;
                        previousWasWall = true;
                        for (int i = posMur; i >= 0; i--)
                        {
                            if (previousWasWall)//Si le dernier etait un mur
                            {
                                if (room[i][leng] == 4)//et que c'est un sol
                                {
                                    previousWasWall = false;
                                    break;
                                }
                            }
                            else//Si le dernier etait un sol
                            {
                                if (room[i][leng] == 1)//Et que c'est un mur
                                {
                                    nbMurH++;
                                    previousWasWall = true;
                                    break;
                                }
                            }

                        }
                    }

                    print(wid+"."+leng + "  G:" + nbMurG + " D:" + nbMurD + " H:" + nbMurH + " B:" + nbMurB);

                    //SI il y a un mur de chaque coté et que y a au moins un sol en haut et a gauche
                    if (murHaut && murBas && murGauche && murDroit && nbMurG%2 == 1 && nbMurD % 2 == 1 && nbMurH % 2 == 1 && nbMurB % 2 == 1) //Si nombre impair de mur a gauche et a droite + des murs de chaque coté + au moins un sol sur la colonne au dessus, on peut continuer
                    {
                        print("allInOne");
                        room[wid][leng] = 4;
                        nbSol++;
                    }
                }
            }
        }
        return nbSol;
    }


    void GenerateRoom(int width, int length)
    {
        GameObject roomGO = new GameObject("salle");
        GameObject tuile = new GameObject();

        //Placer la tuile de début

        //Puis la noter comme sol
        room[posDepart.y][posDepart.x] = 4;

        //On place les tuiles correctement (Straight, Corner ...) : 
        for (int wid = 0; wid < width; wid++)
        {
            for (int leng = 0; leng < length; leng++)
            {
                if (room[wid][leng] == 1) //Mur
                {
                    //STRAIGHT
                    if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1)) //haut bas
                    {
                        if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 4)) // Si le sol est  à droite
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                        else //SI le sol est à gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, -90, 0));
                    }
                    if ((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (leng - 1 >= 0 && room[wid][leng - 1] == 1)) //droite gauche
                    {
                        if ((wid + 1 <= width - 1 && room[wid + 1][leng] == 4)) // Si le sol est  en bas
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                        else // SI le sol est en haut
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallStraight"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }

                    //CORNER
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1))) //droite et bas
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est à gauche -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                        else //le sol est en haut a gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1))) //gauche et haut
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est a droite -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 180, 0));
                        else //le sol est en haut a gauche
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 0, 0));
                    }
                    if (((leng + 1 <= length - 1 && room[wid][leng + 1] == 1) && (wid - 1 >= 0 && room[wid - 1][leng] == 1))) //droite et haut
                    {
                        if (((leng - 1 >= 0) && room[wid][leng - 1] == 4)) //Si le sol est da gauche -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
                        else //le sol est en haut a droite
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                    }
                    if (((leng - 1 >= 0 && room[wid][leng - 1] == 1) && (wid + 1 <= width - 1 && room[wid + 1][leng] == 1))) //gauhe et bas
                    {
                        if (((leng + 1 <= length - 1) && room[wid][leng + 1] == 4)) //Si le sol est da droite -> coin ext
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleExt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 90, 0));
                        else //le sol est en haut a droite
                            tuile = (GameObject)Instantiate(Resources.Load("WhiteRoom/Prefab/WallAngleInt"), new Vector3(36 * leng, 0, -36 * wid), Quaternion.Euler(0, 270, 0));
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
//printTable(room);
//fillWithZeros(11, 9);
//printTable(room);


//Algo 1 -> bug
//bool ground = false;
//for (int wid = 0; wid < width; wid++)
//{
//    ground = false;
//    for (int leng = 0; leng < length; leng++)
//    {
//        if (room[wid][leng] == 1) //Si on est sur un mur
//        {
//            if (room[wid][leng + 1] != 1)//Si la case à coté n'est pas un mur
//            {
//                if (ground)
//                {
//                    ground = false;
//                }
//                else
//                {
//                    if (leng + 1 > length - 1)//On arrive au bout donc il ne peut aps y avor de mur donc on met plus de sol
//                        ground = false;
//                    else
//                    {
//                        for (int i = leng + 1; i < length - 1; i++)// Et qu'il y a un mur plus loin
//                        {
//                            if (!ground) //Si on ne mettait pas de ground, on vérifie que il y ai bien un mur au bout pour mettre du sol entre les deux
//                            {
//                                if (room[wid][i] == 1)
//                                {
//                                    ground = true; //Si on mettait du sol, on le fait plus et vice versa
//                                    break;
//                                }
//                            }
//                            else //On mettait du sol, donc on arrete d'en mettre
//                            {
//                                ground = false;
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            if (ground)
//            {
//                room[wid][leng] = 4;
//                nbSol++;
//            }
//        }
//    }
//}