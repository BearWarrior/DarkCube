using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    Rect crossHairRect;
    public Texture crossHairTexture;
    public bool display;

    // Use this for initialization
    void Start () {
        float crossHaireSize = Screen.width*0.065f;
       // crossHairTexture = Resources.Load("Textures/crosshair") as Texture;
        crossHairRect = new Rect(Screen.width / 2 - crossHaireSize / 2,
            Screen.height / 2 - crossHaireSize / 2 - 0.08f * Screen.height,
            crossHaireSize, crossHaireSize);
    }

    void OnGUI()
    {
        if(display)
            GUI.DrawTexture(crossHairRect, crossHairTexture);
    }
}
