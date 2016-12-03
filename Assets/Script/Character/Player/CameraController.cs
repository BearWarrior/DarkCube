using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private GameObject player;
    private GameObject target;
    private GameObject targetStatic;

    public bool playerInMenu;

    public float vitesse;

    public GameObject cameraLookAtPlayer;
    public Vector3 cameraLookAtMenuTR;

	// Use this for initialization
	void Start () 
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (target == null)
        {
            target = GameObject.FindWithTag("CameraTarget");
            targetStatic = target;
        }
        if (cameraLookAtPlayer == null)
            cameraLookAtPlayer = GameObject.FindWithTag("CameraLookAt");

        playerInMenu = false;

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
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        //Camera sur la cible et regarde le joueur
        transform.position = target.transform.position;



        RaycastHit hit;
        Ray ray = new Ray(player.transform.position, (target.transform.position - player.transform.position));
        LayerMask layerPlayer = LayerMask.GetMask("Player");
        LayerMask layerProj = LayerMask.GetMask("Projectile");

        int layerValue = ~(layerPlayer.value | layerProj.value);

        //Si il y a un obstable entre la target et le point a regarder, on place la caméra sur le point de contact et on la décale par rapport a sa normale
        if (Physics.Raycast(ray, out hit, Vector3.Distance(player.transform.position, target.transform.position), layerValue))
            this.transform.position = hit.point + new Vector3(0.1f * hit.normal.x, 0.1f * hit.normal.y, 0.1f * hit.normal.z);




        if (!playerInMenu)
            this.transform.LookAt(cameraLookAtPlayer.transform);
        else
        {
            this.transform.LookAt(cameraLookAtMenuTR);
        }
	}

    public void changeTarget(GameObject newTarg)
    {
        target = newTarg;
    }

    public void resetTarget()
    {
        target = targetStatic;
    }
}
