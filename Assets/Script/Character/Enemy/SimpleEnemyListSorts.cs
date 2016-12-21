using UnityEngine;
using System.Collections;
using System;

public class SimpleEnemyListSorts : ListSorts
{
    public override void initSort()
    {
        attaqueEquiped = new SortDeJet("_", "Ball", EnumScript.Element.Elec, 1, EnumScript.CustomProj1.Normal, EnumScript.CustomProj2.Normal);
    }
}
