using UnityEngine;
using System.Collections;

public class CustomRoomBehaviour : MonoBehaviour
{
    public GameObject player;
    private GameObject playerInst;
    public GameObject cam;
    public GameObject projector;
    public GameObject spawnPlayer;

    public GameObject menuPause;
    

    // Use this for initialization
    void Start ()
    {
        playerInst = Instantiate(player, spawnPlayer.transform.position, new Quaternion(0, 0, 0, 0)).transform.GetChild(0).gameObject;
        Instantiate(cam);
        GameObject proj = Instantiate(projector);

        playerInst.GetComponent<PlayerController>().projector = proj;
        playerInst.GetComponent<PlayerController>().menuPause = menuPause;
        menuPause.GetComponent<MenuMain>().player = playerInst;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
