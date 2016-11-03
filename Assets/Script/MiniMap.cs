using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour 
{
    //La camera doit se placer au centre de la salle et avoir la bonne taille
    public void ChangePositionAndSize(Vector3 pos, float size)
    {
        transform.position = new Vector3(pos.x * .75f, transform.position.y, pos.z*.75f);
        GetComponent<Camera>().orthographicSize = 3.75f * size * .75f;
    }
}