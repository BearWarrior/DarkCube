using UnityEngine;
using System.Collections;
using System;

public class PlayerControllerOLD : MonoBehaviour
{
    public float vitesseMarche;
    public float vitesseSprint;
    public float vitesseRotation;

    private float gravtity;
    private float actualGravity;

    private float HorizontalAxis;
    private float lastHorizontalAxis;
    private float lastlastHorizontalAxis;

    private float VerticalAxis;
    private float lastVerticalAxis;
    private float lastlastVerticalAxis;

    public GameObject projector;

    private bool isGrounded;

    private bool aiming;
    private bool running;


    private Quaternion leftTilt;
    private Quaternion tilt;
    private Quaternion noTilt;

    private GameObject player;


    // Use this for initialization
    void Start()
    {
        HorizontalAxis = 0;
        lastHorizontalAxis = 0;
        lastlastHorizontalAxis = 0;

        VerticalAxis = 0;
        lastVerticalAxis = 0;
        lastlastVerticalAxis = 0;

        isGrounded = false;

        gravtity = 10;

        leftTilt = Quaternion.Euler(new Vector3(0, 0, 5));
        tilt = Quaternion.Euler(new Vector3(0, 0, -5));
        noTilt = Quaternion.Euler(new Vector3(0, 0, 0));

        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        /*
        //GESTION DU GABARIT
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Recuperation du layerMask Player
        LayerMask layer = LayerMask.GetMask("Player");
        //Inversion (on avoir la detection de tout SAUF du joueur
        int layerValue = ~layer.value;
        //La visée passe a travers le player et le projectile
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerValue))
        {
            projector.transform.position = new Vector3(hit.point.x, 20f, hit.point.z);
            projector.SetActive(true);
        }
        else
        {
            projector.SetActive(false);
        }*/

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
    }




    void FixedUpdate()
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

        tilt = Quaternion.Euler(new Vector3(Input.GetAxis("Vertical") * (Convert.ToInt32(running) * 5 + 5), 0, Input.GetAxis("Horizontal") * -(Convert.ToInt32(running) * 5 + 5)));
        GameObject.FindWithTag("Tiltable").transform.localRotation = Quaternion.Slerp(GameObject.FindWithTag("Tiltable").transform.localRotation, tilt, Time.deltaTime * 10);
   

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
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + new Vector3(0, 12, 0);
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
        isGrounded = true;
    }

    public void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}
