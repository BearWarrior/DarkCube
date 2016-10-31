using UnityEngine;
using System.Collections;
using System;

public class SimpleEnemyListSorts : ListSorts
{
    public override void initSort()
    {
        attaqueEquiped = new SortDeJet(4, 5f, 5, EnumScript.Element.Elec, "Ball", "BLBLBL osed", "osef aussi", 1);
    }
}
