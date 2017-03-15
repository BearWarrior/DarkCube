using UnityEngine;
using System.Collections;

public class FaceCamera180 : MonoBehaviour
{
    void LateUpdate()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
            gameObject.transform.GetChild(i).transform.eulerAngles = new Vector3(gameObject.transform.GetChild(i).transform.eulerAngles.x, 180, gameObject.transform.GetChild(i).transform.eulerAngles.z);
    }
}