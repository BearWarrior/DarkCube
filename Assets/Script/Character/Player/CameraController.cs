using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private GameObject player;
    private GameObject target;

    public bool playerInMenu;
    public bool lookAtPlayer;

    public float vitesse;

    public GameObject cameraLookAtPlayer; //Will be set by the accessMenu script
    public Vector3 cameraLookAtMenuTR; //Will be set by the accessMenu script
    public GameObject menuAssociatedWith; //Will be set by the accessMenu script

    //Place camera
    public GameObject posCameraMenu { get; set; }
    public float cameraToMenuSpeed { get; set; }
    private float cameraStartTime;
    private float cameraJourneyLength;
    private Vector3 cameraFrom;
    private Vector3 cameraTo;

    public bool cameraPlayerToMenu;
    public bool cameraMenuToPlayer;

    void Start () 
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (target == null)
            target = GameObject.FindWithTag("CameraTarget");
        if (cameraLookAtPlayer == null)
            cameraLookAtPlayer = GameObject.FindWithTag("CameraLookAt");

        playerInMenu = false;
        lookAtPlayer = true;
    }

	void FixedUpdate () 
    {

        //Camera sur la cible et regarde le joueur si il n'est pas en menu
        if (!playerInMenu)
        {
            transform.position = target.transform.position;
            RaycastHit hit;
            Ray ray = new Ray(player.transform.position, (target.transform.position - player.transform.position));
            LayerMask layerPlayer = LayerMask.GetMask("Player");
            LayerMask layerProj = LayerMask.GetMask("Projectile");

            int layerValue = ~(layerPlayer.value | layerProj.value);

            //Si il y a un obstable entre la target et le point a regarder, on place la caméra sur le point de contact et on la décale par rapport a sa normale
            if (Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, target.transform.position), layerValue))
            {
                if (!hit.collider.isTrigger)
                    this.transform.position = hit.point + new Vector3(0.1f * hit.normal.x, 0.1f * hit.normal.y, 0.1f * hit.normal.z);
            }

            this.transform.LookAt(cameraLookAtPlayer.transform);
        }
        else
        {
            this.transform.LookAt(cameraLookAtMenuTR);
        }

        //Camera to and from menu management
        if(cameraPlayerToMenu) //Camera from player to menu
        {
            float distCovered = (Time.time - cameraStartTime) * cameraToMenuSpeed;
            float fracJourney = distCovered / cameraJourneyLength;
            Camera.main.transform.position = Vector3.Lerp(cameraFrom, cameraTo, fracJourney);
            
            if(fracJourney > 1)
            {
                cameraPlayerToMenu = false;
                menuAssociatedWith.SendMessage("show");
            }
        }
        else if(cameraMenuToPlayer) //Camera from menu to player
        {
            float distCovered = (Time.time - cameraStartTime) * cameraToMenuSpeed;
            float fracJourney = distCovered / cameraJourneyLength;
            Camera.main.transform.position = Vector3.Lerp(cameraFrom, cameraTo, fracJourney);

            menuAssociatedWith.SendMessage("hide");

            if (fracJourney > 1)
            {
                cameraMenuToPlayer = false;
                playerInMenu = false;
                //give control to player
                player.GetComponent<PlayerController>().setControllable(true);
                //cursor invisible and locked + crosshair
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Camera.main.GetComponent<Crosshair>().display = true;    
            }
        }        
    }

    public void launchMenu()
    {
        //Place camera
        cameraStartTime = Time.time;
        cameraFrom = Camera.main.transform.position;
        cameraTo = posCameraMenu.transform.position;
        cameraJourneyLength = Vector3.Distance(cameraFrom, cameraTo);
        cameraPlayerToMenu = true;
        playerInMenu = true;
        lookAtPlayer = false;
        Camera.main.GetComponent<Crosshair>().display = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerController>().setControllable(false);
    }

    public void exitMenu()
    {
        //Reset Camera
        cameraLookAtMenuTR = cameraLookAtPlayer.transform.position;
        cameraStartTime = Time.time;
        cameraFrom = Camera.main.transform.position;
        cameraTo = target.transform.position;
        cameraJourneyLength = Vector3.Distance(cameraFrom, cameraTo);
        cameraMenuToPlayer = true;
        lookAtPlayer = true;
    }
}

/*void Update()
{
    RaycastHit hit;
    Ray ray = new Ray(player.transform.position, (target.transform.position - player.transform.position));
    LayerMask layerPlayer = LayerMask.GetMask("Player");
    LayerMask layerProj = LayerMask.GetMask("Projectile");

    int layerValue = ~ (layerPlayer.value | layerProj.value);

    //Si il y a un obstable entre la target et le point a regarder, on place la caméra sur le point de contact et on la décale par rapport a sa normale
    if (Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, target.transform.position), layerValue))
        this.transform.position = hit.point + new Vector3(0.1f * hit.normal.x, 0.1f * hit.normal.y, 0.1f * hit.normal.z);
}*/
