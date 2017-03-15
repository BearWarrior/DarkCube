using UnityEngine;
using System.Collections;

public class OffsetTextureAnimate : MonoBehaviour {

    public double SunGlobeSpeedX = 0.015;
    public double SunGlobeSpeedY = 0.015;
    public double SunOuterlayerSpeedX = 0.015;
    public double SunOuterlayerSpeedY = 0.015;

    float offsetX;
    float offsetY;
    float offset2X;
    float offset2Y;

    // Update is called once per frame
    void Update () {
        offsetX = (float)(Time.time * SunGlobeSpeedX % 1);
        offsetY = (float)(Time.time * SunGlobeSpeedY % 1);
        offset2X = (float)(Time.time * SunOuterlayerSpeedX % 1);
        offset2Y = (float)(Time.time * SunOuterlayerSpeedY % 1);
        gameObject.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap",new Vector2(offsetX, offsetY));
        gameObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex",new Vector2(offsetX, offsetY));
        if (gameObject.GetComponent<Renderer>().materials.Length > 1)
        {
            gameObject.GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex",new Vector2(offset2X, offset2Y));
            gameObject.GetComponent<Renderer>().materials[1].SetTextureOffset("_BumpMap",new Vector2(offset2X, offset2Y));
        }
    }
}
