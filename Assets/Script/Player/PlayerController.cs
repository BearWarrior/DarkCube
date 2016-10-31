using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
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
    private bool dashUsed;
    private bool usingZoneCone;
    private bool gabaritUsed;
    private bool controllable;
    private bool projectorAtMouse; //true : projector au niveau de la souris   false : projector devant le joueur position fixe
    
    private SortDeZone sortDeZone;

    protected float enduranceMax;
    protected float enduranceActuel;

    protected float timeBeforeRunningMax;
    protected float timeBeforeRunningAct;
    protected bool regenEndurance;

    // Use this for initialization
    void Awake()
    {
        //Creating the cube full of cubes
        int nbCubeSide = 6;
        float sideLength = 0.60000f;
        float scale = 0.07f;
        float distPerCube = sideLength / nbCubeSide;
        GameObject cubes = new GameObject("cubes");
        cubes.tag = "Armature";
        cubes.transform.SetParent(gameObject.transform.parent);
        GameObject posCubes = new GameObject("posCubes");
        posCubes.transform.SetParent(gameObject.transform);
        List<GameObject> posCubesVert = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            GameObject faceVert = new GameObject("faceVert" + i);
            faceVert.transform.SetParent(posCubes.transform);
            posCubesVert.Add(faceVert);
        }

        List<List<GameObject>> horizFaces = new List<List<GameObject>>();
        for (int i = 0; i < 6; i++)
        {
            horizFaces.Add(new List<GameObject>());
        }
        int cpt = 0;
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
                                //GameObject cube = (GameObject) Instantiate(Resources.Load("Particle/prefabs/others/miniCubeV2"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                                cube.name = "Cube" + cpt;
                                cube.transform.localScale = new Vector3(1, 1, 1) * scale;
                                cube.transform.position = new Vector3(distPerCube * width - sideLength / 2 + distPerCube / 2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                cube.transform.SetParent(cubes.transform);
                                Destroy(cube.GetComponent<BoxCollider>());

                                GameObject posCube = new GameObject("posCube" + cpt);
                                posCube.transform.position = new Vector3(distPerCube * width - sideLength / 2 + distPerCube / 2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                //posCube.transform.SetParent(posCubes.transform);
                                posCube.transform.SetParent(posCubesVert[width].transform);

                                horizFaces[length].Add(posCube); //face pointant en haut/bas
                                cpt++;
                            }
                        }
                    }
                }
            }
        }

        //GameObject core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject core = (GameObject)Instantiate(Resources.Load("Particle/prefabs/others/core"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        core.name = "Core";
        core.transform.localScale = new Vector3(1, 1, 1) * scale * 2;
        core.transform.position = new Vector3(0, 0, 0);
        core.transform.SetParent(cubes.transform);
        Destroy(core.GetComponent<SphereCollider>());

        GameObject posCore = new GameObject("posCore");
        posCore.transform.position = new Vector3(0, 0, 0);
        posCore.transform.SetParent(posCubes.transform);

        cubes.transform.localPosition = new Vector3(0, 0, 0);
        posCubes.transform.localPosition = new Vector3(0, 0, 0);

        this.GetComponent<Player>().sphere = core;
        this.GetComponent<Player>().armature = cubes;

        SortChooser sortChooser = gameObject.AddComponent<SortChooser>();
        sortChooser.setListCubes(horizFaces, posCubes);

        PlayerCubeFlock flock = gameObject.AddComponent<PlayerCubeFlock>();
        flock.init(cubes, posCubes);



        HorizontalAxis = 0;
        lastHorizontalAxis = 0;
        lastlastHorizontalAxis = 0;

        VerticalAxis = 0;
        lastVerticalAxis = 0;
        lastlastVerticalAxis = 0;

        player = GameObject.FindWithTag("Player");
        
        isGrounded = false;
        controllable = true;
        gabaritUsed = false;
        usingZoneCone = false;
        dashUsed = false;

        enduranceMax = 0.2f;
        enduranceActuel = enduranceMax;
        regenEndurance = false;
        timeBeforeRunningMax = 3;
        timeBeforeRunningAct = 0;

        projector = GameObject.FindWithTag("Projector");
    }

    public float getEndurance()
    {
        return enduranceActuel;
    }

    public void downEndurance()
    {
        regenEndurance = false;
        enduranceActuel -= Time.deltaTime;
        enduranceActuel = (enduranceActuel < 0) ? 0 : enduranceActuel;
    }

    void Update()
    {
        //RegenDash
        if (!isRunning())
            timeBeforeRunningAct += Time.deltaTime;
        else
            timeBeforeRunningAct = 0;
        if (timeBeforeRunningAct > timeBeforeRunningMax)
        {
            dashUsed = false;
            enduranceActuel = enduranceMax;
        }


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
            //SPRINT
            if (Input.GetKeyDown(KeyCode.LeftShift) && (HorizontalAxis != 0 || VerticalAxis != 0))
            {
                if (running)
                    running = false;
                else
                    if (getEndurance() > 0)
                    running = true;
            }

            if (running && getEndurance() == 0)
                running = false;

            if (HorizontalAxis == 0 && VerticalAxis == 0)
                running = false;

            if (running)
                downEndurance();

            //Instatiation du particle Dash
            Vector3 dire = new Vector3(HorizontalAxis, 0, VerticalAxis);
            float angle = Vector3.Angle(dire, Vector3.forward);
            if (HorizontalAxis < 0)
                angle *= -1;
            if (running && !dashUsed)
            {
                GameObject dash = (GameObject) Instantiate(Resources.Load("Particle/Prefabs/Others/BlueDash"), this.transform.position, Quaternion.Euler(new Vector3(0, angle, 0) + this.transform.localEulerAngles));
                Destroy(dash, 3);
                dashUsed = true;
            }
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

    //public void setGabarit(bool active, SortDeZone sort)
    //{
    //    if (active)
    //    {
    //        sortDeZone = sort;
    //        gabaritUsed = active;
    //        Material texture;

    //        switch (sort.gabarit)
    //        {
    //            case EnumScript.GabaritSortDeZone.Cercle:
    //                projectorAtMouse = true;
    //                texture = Resources.Load("SortDeZone/Gabarit/LightProjectorCircle") as Material;
    //                projector.GetComponent<Projector>().material = texture;
    //                //CHANGER CA
    //                //switch (sort.taille)
    //                //{
    //                //    case EnumScript.TailleSortDeZone.Petit:
    //                //        projector.GetComponent<Projector>().orthographicSize = 10;
    //                //        break;
    //                //    case EnumScript.TailleSortDeZone.Moyen:
    //                //        projector.GetComponent<Projector>().orthographicSize = 15;
    //                //        break;
    //                //    case EnumScript.TailleSortDeZone.Grand:
    //                //        projector.GetComponent<Projector>().orthographicSize = 20;
    //                //        break;
    //                //}
    //                projector.GetComponent<Projector>().orthographicSize = 3;


    //                projector.GetComponent<Projector>().aspectRatio = 1;
    //                break;
    //            case EnumScript.GabaritSortDeZone.Ligne:
    //                texture = Resources.Load("SortDeZone/Gabarit/LightProjectorRectangle") as Material;
    //                projector.GetComponent<Projector>().material = texture;
    //                //CHANGER CA
    //                /*switch (sort.taille)
    //                {
    //                    case EnumScript.TailleSortDeZone.Petit:
    //                        projector.GetComponent<Projector>().orthographicSize = 4;
    //                        projector.GetComponent<Projector>().aspectRatio = 4f;
    //                        break;
    //                    case EnumScript.TailleSortDeZone.Moyen:
    //                        projector.GetComponent<Projector>().orthographicSize = 4;
    //                        projector.GetComponent<Projector>().aspectRatio = 6f;
    //                        break;
    //                    case EnumScript.TailleSortDeZone.Grand:
    //                        projector.GetComponent<Projector>().orthographicSize = 4;
    //                        projector.GetComponent<Projector>().aspectRatio = 8f;
    //                        break;
    //                }*/
    //                projector.GetComponent<Projector>().orthographicSize = 4 / 2;
    //                projector.GetComponent<Projector>().aspectRatio = 6f / 2;

    //                projectorAtMouse = true;
    //                break;
    //            case EnumScript.GabaritSortDeZone.Cone:
    //                projector.transform.SetParent(player.transform);
    //                float taille = 0;
    //                //CHANGER CA
    //                /*switch (sort.taille)
    //                {
    //                    case EnumScript.TailleSortDeZone.Petit:
    //                        taille = 15;
    //                        break;
    //                    case EnumScript.TailleSortDeZone.Moyen:
    //                        taille = 22;
    //                        break;
    //                    case EnumScript.TailleSortDeZone.Grand:
    //                        taille = 29;
    //                        break;
    //                }*/
    //                taille = 22 / 5;
    //                projector.transform.localPosition = new Vector3(0, 0, .5f + taille);
    //                projector.transform.localEulerAngles = new Vector3(90, 0, 0);

    //                texture = Resources.Load("SortDeZone/Gabarit/LightProjectorCone") as Material;
    //                projector.GetComponent<Projector>().material = texture;
    //                projector.GetComponent<Projector>().orthographicSize = taille;
    //                projectorAtMouse = false;
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        gabaritUsed = false;
    //    }
    //}


    void FixedUpdate()
    {
        if (controllable)
        {
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
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        controllable = b;
    }

    public bool getControllable()
    {
        return controllable;
    }
}
