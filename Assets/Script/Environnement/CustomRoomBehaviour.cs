using UnityEngine;
using System.Collections;

public class CustomRoomBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public GameObject projector;
    public GameObject spawnPlayer;

    

    // Use this for initialization
    void Start ()
    {
        Instantiate(player, spawnPlayer.transform.position, new Quaternion(0, 0, 0, 0));
        Instantiate(cam);
        GameObject proj = Instantiate(projector);

        GameObject.FindWithTag("Player").GetComponent<PlayerController>().projector = proj;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
