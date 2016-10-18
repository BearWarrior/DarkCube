using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController2 : MonoBehaviour
{
    public float vitesseMarche;
    public float vitesseSprint;
    public float vitesseRotation;
    public float impulsionSaut;

    private float HorizontalAxis;
    private float lastHorizontalAxis;
    private float lastlastHorizontalAxis;

    private float VerticalAxis;
    private float lastVerticalAxis;
    private float lastlastVerticalAxis;

    private float timerZoneConeAct;
    private float timerZoneConeMax;

    public GameObject projector;
    private GameObject player;

    private bool isGrounded;
    private bool aiming;
    private bool running;
    private bool usingZoneCone;
    private bool gabaritUsed;
    private bool controllable;
    private bool projectorAtMouse; //true : projector au niveau de la souris   false : projector devant le joueur position fixe

    private Quaternion tilt;

    // PhotonView view; //permet de jouer son joueur et pas celui des autres

    private SortDeZone sortDeZone;

    // Use this for initialization
    void Awake()
    {
        //Creating the cube full of cubes
        int nbCubeSide = 6;
        float sideLength = 0.60000f;
        float scale = 0.07f;
        float distPerCube = sideLength / nbCubeSide;
        GameObject cubes = new GameObject("cubesDuplicates");
        cubes.transform.SetParent(gameObject.transform.parent);
        GameObject posCubes = new GameObject("posCubesDuplicates");
        posCubes.transform.SetParent(gameObject.transform);
        List<GameObject> posCubesVert = new List<GameObject>();
        for(int i = 0; i < 6; i++)
        {
            GameObject faceVert = new GameObject("faceVert" + i);
            faceVert.transform.SetParent(posCubes.transform);
            posCubesVert.Add(faceVert);
        }

        List<List<GameObject>> horizFaces = new List<List<GameObject>>();
        List<List<GameObject>> vertFaces = new List<List<GameObject>>();
        for (int i = 0; i < 6; i++)
        {
            horizFaces.Add(new List<GameObject>());
            vertFaces.Add(new List<GameObject>());
        }

        for (int width = 0; width < nbCubeSide; width++)
        {
            for (int length = 0; length < nbCubeSide; length++)
            {
                for (int height = 0; height < nbCubeSide; height++)
                {
                    if ((width < nbCubeSide / 3 || width >= 2 * nbCubeSide / 3) || (height < nbCubeSide / 3 || height >= 2 * nbCubeSide / 3))
                    {
                        if ((width < nbCubeSide / 3 || width >= 2 * nbCubeSide / 3) || (length < nbCubeSide / 3 || length >= 2 * nbCubeSide / 3))
                        {
                            if ((height < nbCubeSide / 3 || height >= 2 * nbCubeSide / 3) || (length < nbCubeSide / 3 || length >= 2 * nbCubeSide / 3))
                            {
                                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                cube.transform.localScale = new Vector3(1, 1, 1) * scale;
                                cube.transform.position = new Vector3(distPerCube * width - sideLength / 2 + distPerCube/2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                cube.transform.SetParent(cubes.transform);
                                Destroy(cube.GetComponent<BoxCollider>());
                                
                                GameObject posCube = new GameObject();
                                posCube.transform.position = new Vector3(distPerCube * width - sideLength/ 2 + distPerCube / 2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                //posCube.transform.SetParent(posCubes.transform);
                                posCube.transform.SetParent(posCubesVert[width].transform);

                                horizFaces[length].Add(posCube); //face pointant en haut/bas
                                vertFaces[width].Add(posCube); //face pointant a droite/gauche
                            }
                        }
                    }
                }
            }
        }

        cubes.transform.localPosition = new Vector3(0, 0, 0);
        posCubes.transform.localPosition = new Vector3(0, 0, 0);
        posCubes.tag = "Armature";

        //Debug par décalage
        //for (int i = 0; i < sideFaces.Count; i++)d
        //{
        //    for (int j = 0; j < sideFaces[i].Count; j++)
        //    {
        //        sideFaces[i][j].transform.position += new Vector3(0, i + 1, 0);
        //    }
        //}

        SortChooser sortChooser = gameObject.AddComponent<SortChooser>();
        sortChooser.setListCubes(horizFaces, posCubesVert);

        PlayerCubeFlock flock = gameObject.AddComponent<PlayerCubeFlock>();
        flock.init(cubes, posCubes);



        HorizontalAxis = 0;
        lastHorizontalAxis = 0;
        lastlastHorizontalAxis = 0;

        VerticalAxis = 0;
        lastVerticalAxis = 0;
        lastlastVerticalAxis = 0;

        tilt = Quaternion.Euler(new Vector3(0, 0, -5));

        player = GameObject.FindWithTag("Player");

        //view = GetComponent<PhotonView>(); //permet de jouer son joueur et pas celui des autres

        isGrounded = false;
        controllable = true;
        gabaritUsed = false;
        usingZoneCone = false;

        projector = GameObject.FindWithTag("Projector");
    }

    void Update()
    {
        if (gabaritUsed) //Joueur a un sort de zone dans la main -> affichage du gabarit correspondant
        {
            if (projectorAtMouse) // Gabarit sur la souris
            {
                //GESTION DU GABARIT
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2 + 0.08f * Screen.height));
                //Recuperation du layerMask Player
                LayerMask layer = LayerMask.GetMask("Player");
                //Inversion (on avoir la detection de tout SAUF du joueur)
                int layerValue = ~layer.value;
                //La visée passe a travers le player et le projectile
                if (Physics.Raycast(ray, out hit, /*Mathf.Infinity*/sortDeZone.portee, layerValue))
                {
                    projector.transform.position = new Vector3(hit.point.x, player.transform.position.y + 2, hit.point.z);
                    if (Physics.Raycast(projector.transform.position, -Vector3.up, out hit, Mathf.Infinity, layerValue))
                    {
                        projector.transform.position = hit.point + new Vector3(0, 1, 0);
                    }

                    projector.transform.eulerAngles = new Vector3(90, player.transform.eulerAngles.y, 0);
                    projector.SetActive(true);
                }
                else // il ne touche rien, on le bloque à sort.portee
                {
                    Vector3 point = ray.origin + ray.direction * sortDeZone.portee;
                    if (Physics.Raycast(point, -Vector3.up, out hit, /*Mathf.Infinity*/sortDeZone.portee, layerValue))
                    {
                        projector.transform.position = new Vector3(hit.point.x, 20f, hit.point.z);
                        projector.transform.eulerAngles = new Vector3(90, player.transform.eulerAngles.y, 0);
                        projector.SetActive(true);
                    }
                }
            }
            else // Gabarit devant le joueur
            {
                projector.SetActive(true);
            }
        }
        else
        {
            projector.SetActive(false);
        }


        if (controllable) //Joueur controllable (pas dans un menu)
        {
            //if (view == null || view.isMine) //permet de jouer son joueur et pas celui des autres
            //{
            //SPRINT
            if (Input.GetKeyDown(KeyCode.LeftShift))
                if (running)
                    running = false;
                else
                    if (player.GetComponent<Player>().getEndurance() > 0)
                    running = true;

            if (running && player.GetComponent<Player>().getEndurance() == 0)
                running = false;

            if (HorizontalAxis == 0 && VerticalAxis == 0)
                running = false;

            if (running)
                player.GetComponent<Player>().downEndurance();
            //}
        }

        if (usingZoneCone) //Le joueur utilsie un sort de zone qui part de lui.
        {
            /*List<GameObject> listEnemy = GameObject.FindWithTag("World").GetComponent<EnemyBehaviour>().listEnemy;
            foreach(GameObject enemy in listEnemy)
            {
                Vector3 direction = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);

                Debug.Log("dist : " + Vector3.Distance(transform.position, enemy.transform.position));
                Debug.Log("angle : " + Vector3.Angle(transform.forward, direction));
                if (Vector3.Distance(transform.position, enemy.transform.position) < sortDeZone.taille) // Si il est à la bonne distance
                {
                    
                }
            }*/

            if (timerZoneConeAct > timerZoneConeMax) //fini
                usingZoneCone = false;
            else
                timerZoneConeAct += Time.deltaTime;
        }
    }


    public void useZoneCone(float time)
    {
        timerZoneConeAct = 0;
        timerZoneConeMax = time;
        usingZoneCone = true;
    }

    public void setGabarit(bool active, SortDeZone sort)
    {
        if (active)
        {
            sortDeZone = sort;
            gabaritUsed = active;
            Material texture;

            switch (sort.gabarit)
            {
                case EnumScript.GabaritSortDeZone.Cercle:
                    projectorAtMouse = true;
                    texture = Resources.Load("SortDeZone/Gabarit/LightProjectorCircle") as Material;
                    projector.GetComponent<Projector>().material = texture;
                    //CHANGER CA
                    //switch (sort.taille)
                    //{
                    //    case EnumScript.TailleSortDeZone.Petit:
                    //        projector.GetComponent<Projector>().orthographicSize = 10;
                    //        break;
                    //    case EnumScript.TailleSortDeZone.Moyen:
                    //        projector.GetComponent<Projector>().orthographicSize = 15;
                    //        break;
                    //    case EnumScript.TailleSortDeZone.Grand:
                    //        projector.GetComponent<Projector>().orthographicSize = 20;
                    //        break;
                    //}
                    projector.GetComponent<Projector>().orthographicSize = 3;


                    projector.GetComponent<Projector>().aspectRatio = 1;
                    break;
                case EnumScript.GabaritSortDeZone.Ligne:
                    texture = Resources.Load("SortDeZone/Gabarit/LightProjectorRectangle") as Material;
                    projector.GetComponent<Projector>().material = texture;
                    //CHANGER CA
                    /*switch (sort.taille)
                    {
                        case EnumScript.TailleSortDeZone.Petit:
                            projector.GetComponent<Projector>().orthographicSize = 4;
                            projector.GetComponent<Projector>().aspectRatio = 4f;
                            break;
                        case EnumScript.TailleSortDeZone.Moyen:
                            projector.GetComponent<Projector>().orthographicSize = 4;
                            projector.GetComponent<Projector>().aspectRatio = 6f;
                            break;
                        case EnumScript.TailleSortDeZone.Grand:
                            projector.GetComponent<Projector>().orthographicSize = 4;
                            projector.GetComponent<Projector>().aspectRatio = 8f;
                            break;
                    }*/
                    projector.GetComponent<Projector>().orthographicSize = 4 / 2;
                    projector.GetComponent<Projector>().aspectRatio = 6f / 2;

                    projectorAtMouse = true;
                    break;
                case EnumScript.GabaritSortDeZone.Cone:
                    projector.transform.SetParent(player.transform);
                    float taille = 0;
                    //CHANGER CA
                    /*switch (sort.taille)
                    {
                        case EnumScript.TailleSortDeZone.Petit:
                            taille = 15;
                            break;
                        case EnumScript.TailleSortDeZone.Moyen:
                            taille = 22;
                            break;
                        case EnumScript.TailleSortDeZone.Grand:
                            taille = 29;
                            break;
                    }*/
                    taille = 22 / 5;
                    projector.transform.localPosition = new Vector3(0, 0, .5f + taille);
                    projector.transform.localEulerAngles = new Vector3(90, 0, 0);

                    texture = Resources.Load("SortDeZone/Gabarit/LightProjectorCone") as Material;
                    projector.GetComponent<Projector>().material = texture;
                    projector.GetComponent<Projector>().orthographicSize = taille;
                    projectorAtMouse = false;
                    break;
            }
        }
        else
        {
            gabaritUsed = false;
        }
    }


    void FixedUpdate()
    {
        if (controllable)
        {
            //if (view == null || view.isMine) //permet de jouer son joueur et pas celui des autres
            //{
            //Moyenne entre les 3 derniers input pour eviter la saccade lors du chagement direction
            lastlastHorizontalAxis = lastHorizontalAxis;
            lastHorizontalAxis = HorizontalAxis;
            HorizontalAxis = Input.GetAxis("Horizontal");
            lastlastVerticalAxis = lastVerticalAxis;
            lastVerticalAxis = VerticalAxis;
            VerticalAxis = Input.GetAxis("Vertical");
            float mvtH = (HorizontalAxis + lastHorizontalAxis + lastlastHorizontalAxis) / 3;
            float mvtV = (VerticalAxis + lastVerticalAxis + lastlastVerticalAxis) / 3;

            //Deplacement droite gauche avant arriere
            Vector3 movement = new Vector3((Convert.ToInt32(running) * vitesseSprint + vitesseMarche) * Time.deltaTime * mvtH, GetComponent<Rigidbody>().velocity.y, (Convert.ToInt32(running) * vitesseSprint + vitesseMarche) * Time.deltaTime * mvtV);
            movement = transform.TransformDirection(movement);

            GetComponent<Rigidbody>().velocity = movement;

            //tilt = Quaternion.Euler(new Vector3(Input.GetAxis("Vertical") * (Convert.ToInt32(running) * 5 + 5), 0, Input.GetAxis("Horizontal") * -(Convert.ToInt32(running) * 5 + 5)));
            //GameObject.FindWithTag("Tiltable").transform.localRotation = Quaternion.Slerp(GameObject.FindWithTag("Tiltable").transform.localRotation, tilt, Time.deltaTime * 10);

            GetComponent<PlayerCubeFlock>().direction = new Vector3(mvtH, 0, mvtV);

            //Si clic gauche ou clic droit enfoncé -> controle caméra -> souris disparait + la caméra rotate le joueur
            if (aiming)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                transform.Rotate(0, vitesseRotation * Time.deltaTime * Input.GetAxis("Mouse X"), 0);
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().moveTarget();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            //Si clic droit down on passe en mode visée et si on l'était pas, on rejoint l'origine avec la caméra
            if (Input.GetMouseButtonDown(1) && !aiming)
            {
                aiming = true;
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().joinOrigine(true);
            }

            //On quitte le mode visée
            if (Input.GetKey(KeyCode.Escape))
                aiming = false;

            //Si clic gauche enfoncé et pas en mode visée -> Caméra rotation autour jouuer
            if (Input.GetMouseButton(0) && !isAiming())
            {
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().freeCameraMove();
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().joinOrigine(false);
            }

            //Si clic molette -> camera revient smoothly derriere le joueur
            if (Input.GetMouseButtonDown(2))
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().joinOrigine(true);

            //Zoom mouse ScrollWheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                GameObject.FindWithTag("CameraTarget").GetComponent<CameraTarget>().Zoom(Input.GetAxis("Mouse ScrollWheel"));

            //Gestion du saut
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + new Vector3(0, impulsionSaut, 0);
            //}
        }
    }




    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool isAiming()
    {
        return aiming;
    }

    public bool isRunning()
    {
        return running;
    }

    public void OnCollisionStay(Collision other)
    {
        float angleWithGround = Vector3.Angle(other.contacts[0].normal, Vector3.up);
        if (angleWithGround < 75)
            isGrounded = true;
    }

    public void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    public void setControllable(bool b)
    {
        if (!b) //incontrollable
        {
            GameObject.FindWithTag("Tiltable").transform.localRotation = new Quaternion(0, 0, 0, 0);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        controllable = b;
    }

    public bool getControllable()
    {
        return controllable;
    }
}
