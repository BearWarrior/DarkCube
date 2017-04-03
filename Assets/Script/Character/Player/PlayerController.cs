using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour, IInputsObservable
{
    public float vitesseMarche;
    public float vitesseSprint;
    public float vitesseRotation;
    public float impulsionSaut;
    [Space(15)]
    public GameObject canvasInteractionHelper;
    public GameObject canvasCubeFaces;
    public GameObject canvasLifeBar;
    public GameObject canvasDash;
    [Space(15)]

    //Used for the deplacmennt
    private int cptZ = 0;
    private int cptQ = 0;
    private int cptS = 0;
    private int cptD = 0;

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

    private float acceleration = 0.075f;
    private int accelerationInvers;

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public GameObject menuPause { get; set; }

    // Use this for initialization
    void Awake()
    {
        keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().lookAtInputs(this.gameObject);

        accelerationInvers = (int)(1 / acceleration);
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
        Material matHexaCube = Resources.Load("Player/Materials/ChangeSkin") as Material;
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
                                cube.GetComponent<Renderer>().material =  matHexaCube;
                                cube.name = "Cube" + cpt;
                                cube.transform.localScale = new Vector3(1, 1, 1) * scale;
                                cube.transform.position = new Vector3(distPerCube * width - sideLength / 2 + distPerCube / 2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                cube.transform.SetParent(cubes.transform);
                                Destroy(cube.GetComponent<BoxCollider>());

                                GameObject posCube = new GameObject("posCube" + cpt);
                                posCube.transform.position = new Vector3(distPerCube * width - sideLength / 2 + distPerCube / 2, distPerCube * length - sideLength / 2 + distPerCube / 2, distPerCube * height - sideLength / 2 + distPerCube / 2);
                                posCube.transform.SetParent(posCubesVert[width].transform);

                                horizFaces[length].Add(posCube); //face pointant en haut/bas
                                cpt++;
                            }
                        }
                    }
                }
            }
        }

        GameObject core = (GameObject)Instantiate(Resources.Load("Particle/prefabs/others/core"), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        core.name = "Core";
        core.transform.localScale = new Vector3(1, 1, 1) * scale * 2;
        core.transform.position = new Vector3(0, 0, 0);
        core.transform.SetParent(cubes.transform);
        Destroy(core.GetComponent<SphereCollider>());

        canvasInteractionHelper.transform.SetParent(core.transform);
        canvasInteractionHelper.transform.SetAsFirstSibling();
        canvasInteractionHelper.transform.localPosition = new Vector3(4.8f, 1.5f, 0);

        canvasCubeFaces.transform.SetParent(core.transform);
        canvasCubeFaces.transform.SetAsFirstSibling();
        canvasCubeFaces.transform.localPosition = new Vector3(-4.75f, .5f, 0);

        canvasDash.transform.SetParent(core.transform);
        canvasDash.transform.SetAsFirstSibling();
        canvasDash.transform.localPosition = new Vector3(-4.75f, -0.4f, -0.6f);

        canvasLifeBar.transform.SetParent(core.transform);
        canvasLifeBar.transform.SetAsFirstSibling();
        canvasLifeBar.transform.localPosition = new Vector3(0f, .5f, -3.1f);

        GameObject posCore = new GameObject("posCore");
        posCore.transform.position = new Vector3(0, 0, 0);
        posCore.transform.SetParent(posCubes.transform);

        cubes.transform.localPosition = new Vector3(0, 0, 0);
        posCubes.transform.localPosition = new Vector3(0, 0, 0);

        this.GetComponent<Player>().sphere = core;
        this.GetComponent<Player>().armature = cubes;

        SortChooser sortChooser = gameObject.AddComponent<SortChooser>();
        sortChooser.setListCubes(horizFaces, posCubes);
        sortChooser.setCanvas(canvasCubeFaces);


        PlayerCubeFlock flock = gameObject.AddComponent<PlayerCubeFlock>();
        flock.init(cubes, posCubes);
       
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
        if (controllable) //Joueur controllable (pas dans un menu)
        {
            if(Input.GetKey(KeyCode.F10))
            {
                Time.timeScale = 0;
                controllable = false;
                menuPause.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

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
            //____________________________________REFACTOR DASH_______________________________//
            //SPRINT
            /*if (Input.GetKeyDown(KeyCode.LeftShift) && (HorizontalAxis != 0 || VerticalAxis != 0))
            {
                if (running)
                    running = false;
                else
                    if (getEndurance() > 0)
                    running = true;
            }*/

            if (running && getEndurance() == 0)
                running = false;

            //Using Axes (can't change input from axes THANKS UNITY)
            /*if (HorizontalAxis == 0 && VerticalAxis == 0)
                running = false;*/

            if (running)
                downEndurance();
            //____________________________________REFACTOR DASH_______________________________//
            //Instatiation du particle Dash
            //Using Axes (can't change input from axes THANKS UNITY)
            //Vector3 dire = new Vector3(HorizontalAxis, 0, VerticalAxis);
            //float angle = Vector3.Angle(dire, Vector3.forward);
            //Using Axes (can't change input from axes THANKS UNITY)
            /*if (HorizontalAxis < 0)
                angle *= -1;*/
            /*if (running && !dashUsed)
            {
                GameObject dash = (GameObject) Instantiate(Resources.Load("Particle/Prefabs/Others/BlueDash"), this.transform.position, Quaternion.Euler(new Vector3(0, angle, 0) + this.transform.localEulerAngles));
                Destroy(dash, 3);
                dashUsed = true;
            }*/
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
            //Using KeyCode
            float mvtH = 0;
            float mvtV = 0;
            if (Input.GetKey(keys["Left"]))
            {
                cptQ++;
                cptQ = (cptQ > accelerationInvers) ? accelerationInvers : cptQ;
            }
            else
            {
                cptQ--;
                cptQ = (cptQ < 0) ? 0 : cptQ--;
            }

            if (Input.GetKey(keys["Right"]))
            {
                cptD++;
                cptD = (cptD > accelerationInvers) ? accelerationInvers : cptD;
            }
            else
            {
                cptD--;
                cptD = (cptD < 0) ? 0 : cptD--;
            }

            if (Input.GetKey(keys["Forward"]))
            {
                cptZ++;
                cptZ = (cptZ > accelerationInvers) ? accelerationInvers : cptZ;
            }
            else
            {
                cptZ--;
                cptZ = (cptZ < 0) ? 0 : cptZ--;
            }

            if (Input.GetKey(keys["Backward"]))
            {
                cptS++;
                cptS = (cptS > accelerationInvers) ? accelerationInvers : cptS;
            }
            else
            {
                cptS--;
                cptS = (cptS < 0) ? 0 : cptS--;
            }

            mvtH -= cptQ * acceleration;
            mvtH += cptD * acceleration;
            mvtV += cptZ * acceleration;
            mvtV -= cptS * acceleration;

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
            if (Input.GetKey(keys["FreeCam"]))
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

            //Jump management
            if (Input.GetKeyDown(keys["Jump"]) && IsGrounded())
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + new Vector3(0, impulsionSaut, 0);
        }
    }

    public void displayInteractionHelper(bool display)
    {
        canvasInteractionHelper.SetActive(display);
    }

    public void exitMenuPause()
    {
        controllable = true;
        menuPause.SetActive(false);
        keys = GameObject.FindWithTag("InputsLoader").GetComponent<InputsLoader>().getInputs();
    }

    public void keysChanged(Dictionary<string, KeyCode> keys)
    {
        this.keys = keys;
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
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        controllable = b;
    }

    public bool getControllable()
    {
        return controllable;
    }

    public void showCanvas(bool show)
    {
        canvasInteractionHelper.SetActive(show);
        canvasCubeFaces.SetActive(show);
        canvasLifeBar.SetActive(show);
        canvasDash.SetActive(show);
    }
}
