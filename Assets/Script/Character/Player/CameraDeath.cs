using UnityEngine;
using System.Collections;

public class CameraDeath : MonoBehaviour
{
    private GameObject canvasDeath;

    private Transform placeToGo;
    private GameObject player;
    private Transform playerCameraTarget;

    private float distance;
    private float startTime = 0;
    private float speed = 4;

    private bool canvasDeathAnimationLaunched = false;

	// Use this for initialization
	void Start ()
    {
        Camera.main.GetComponent<Crosshair>().display = false;
        player = GameObject.FindWithTag("Player");
        playerCameraTarget = GameObject.FindWithTag("CameraTarget").transform;
        GameObject[] locations = GameObject.FindGameObjectsWithTag("SurveillanceCam");
        distance = 10000;
        for(int i = 0; i < locations.Length; i++)
        {
            float dist = Vector3.Distance(locations[i].transform.position, player.transform.position);
            if (dist < distance)
            {
                distance = dist;
                placeToGo = locations[i].transform;
            }
        }

        canvasDeath = GameObject.Find("MenuDeath");
        canvasDeath.transform.SetParent(placeToGo); //Temporary, used to move
        canvasDeath.transform.position = placeToGo.position;
        canvasDeath.transform.localPosition += new Vector3(0, -.6f, .75f);
        canvasDeath.transform.eulerAngles = new Vector3(45, placeToGo.transform.eulerAngles.y, 0);
        canvasDeath.transform.SetParent(null);

        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(Camera.main.transform.position, placeToGo.position) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / distance;
            transform.position = Vector3.Lerp(playerCameraTarget.position, placeToGo.position, fracJourney);
            transform.eulerAngles = new Vector3(45, placeToGo.eulerAngles.y, 0);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (!canvasDeathAnimationLaunched)
            {
                canvasDeath.GetComponent<MenuDeath>().animateMenu();
                canvasDeathAnimationLaunched = true;
            }
        }
    }
}
