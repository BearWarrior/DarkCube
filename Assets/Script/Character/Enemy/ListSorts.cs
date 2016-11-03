using UnityEngine;
using System.Collections;

public abstract class ListSorts : MonoBehaviour
{
    protected Attaque attaqueEquiped;

    public Attaque getAttaqueEquiped()
    {
        return attaqueEquiped;
    }

    public abstract void initSort();
}
