using UnityEngine;
using System.Collections;
using System;

public class SimpleEnemyListSorts : ListSorts
{
    public override void initSort()
    {
        attaqueEquiped = new SortDeJet(10, 5f, 5, EnumScript.Element.Elec, "Shot", "BLBLBL osed", "osed aussi", 1);
    }
}
