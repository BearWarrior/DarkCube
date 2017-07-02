using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    Rect crossHairRect;
    public Texture crossHairTexture;
    public bool display;

    // Use this for initialization
    void Start () {
        float crossHaireSize = Screen.width*0.065f;
        crossHairRect = new Rect(Screen.width / 2 - crossHaireSize / 2,
            Screen.height / 2 - crossHaireSize / 2 - 0.08f * Screen.height,
            crossHaireSize, crossHaireSize);
    }

    void OnGUI()
    {
        if(display)
            GUI.DrawTexture(crossHairRect, crossHairTexture);
    }

    public void ReInitPosition()
    {
        float crossHaireSize = Screen.width * 0.065f;
        crossHairRect = new Rect(Screen.width / 2 - crossHaireSize / 2,
            Screen.height / 2 - crossHaireSize / 2 - 0.08f * Screen.height,
            crossHaireSize, crossHaireSize);
    }
}
