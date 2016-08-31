using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour
{
    private GameObject player;
    private GameObject origine;
    public GameObject cameraLookAt;

    public float zoomMax;
    public float zoomMin;

    public bool inMenu;

    public float vitesseZoom;

    public float vitesseHautBas;

    private bool joinSmooth;

    // Use this for initialization
    void Start()
    {
        inMenu = false;
        player = GameObject.FindWithTag("Player");
        origine = GameObject.FindWithTag("CameraOrigine");
    }

    void FixedUpdate()
    {
        //if(!inMenu)
        //    transform.LookAt(cameraLookAt.transform.position);

        if (joinSmooth)
            joinOrigine(true);
    }

    public void Zoom(float sens)
    {
        //Zoom arriere
        if (Vector3.Distance(this.transform.position, player.transform.position) < zoomMin && sens < 0)
            transform.position = Vector3.LerpUnclamped(this.transform.position, player.transform.position, Time.deltaTime * vitesseZoom * sens);
        //Zoom avant
        if (Vector3.Distance(this.transform.position, player.transform.position) > zoomMax && sens > 0)
            transform.position = Vector3.LerpUnclamped(this.transform.position, player.transform.position, Time.deltaTime * vitesseZoom * sens);

        if (Vector3.Distance(origine.transform.position, player.transform.position) < zoomMin && sens < 0)
            origine.transform.position = Vector3.LerpUnclamped(origine.transform.position, player.transform.position, Time.deltaTime * vitesseZoom * sens);
        if (Vector3.Distance(origine.transform.position, player.transform.position) > zoomMax && sens > 0)
            origine.transform.position = Vector3.LerpUnclamped(origine.transform.position, player.transform.position, Time.deltaTime * vitesseZoom * sens);
    }


    public void moveTarget()
    {
        transform.LookAt(player.transform.position);

        transform.RotateAround(player.transform.position, this.transform.right, -1 * vitesseHautBas * Time.deltaTime * Input.GetAxis("Mouse Y"));

        //Si l'angle dépasse l'angle max/min, on compense
        float angle = transform.eulerAngles.x + 50;
        float angleMod = (angle > 360) ? angle - 360 : angle;
        if(angleMod > 110)
            transform.RotateAround(player.transform.position, this.transform.right, -1* (angleMod-110) );
        if (angleMod < 30)
            transform.RotateAround(player.transform.position, this.transform.right, -1 * (angleMod - 30));
    }

    //Camera libre -> on déplace la cible en rotation autour du joueur
    public void freeCameraMove()
    {
        transform.LookAt(player.transform.position);

        transform.RotateAround(player.transform.position, this.transform.right, -1 * vitesseHautBas * Time.deltaTime * Input.GetAxis("Mouse Y"));

        transform.RotateAround(player.transform.position, this.transform.up, vitesseHautBas * Time.deltaTime * Input.GetAxis("Mouse X"));

        //Si l'angle dépasse l'angle max/min, on compense
        float angle = transform.eulerAngles.x + 50;
        float angleMod = (angle > 360) ? angle - 360 : angle;

        if (angleMod > 130)
            transform.RotateAround(player.transform.position, this.transform.right, -1 * (angleMod - 130));
        if (angleMod < 30)
            transform.RotateAround(player.transform.position, this.transform.right, -1 * (angleMod - 30));
    }

    public void joinOrigine(bool setActive)
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            //setActive = false;
            joinSmooth = false;
        }
        if (setActive)
        {
            Vector3 arrive = new Vector3(origine.transform.localPosition.x, this.transform.localPosition.y, origine.transform.localPosition.z);
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, arrive, 0.1f);
            if (Vector3.Distance(this.transform.localPosition, arrive) > 0.1)
            {
                joinSmooth = true;
            }
            else
            {
                joinSmooth = false;
                this.transform.localPosition = arrive;
            }
        }
        else
        {
            joinSmooth = false;
        }
    }
}





//On laisse la valeur y du joueur
//Vector3 arrive = new Vector3(origine.transform.localPosition.x, origine.transform.localPosition.y, origine.transform.localPosition.z);
//this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, arrive, 0.1f);
//if (Vector3.Distance(this.transform.localPosition, arrive) > 0.1)
//{
//    joinSmooth = true;
//}
//else
//{
//    joinSmooth = false;
//    this.transform.localPosition = arrive;
//}
