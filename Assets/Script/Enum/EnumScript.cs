using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnumScript 
{
    public enum TypeSort { Projectile, Zone };
    public enum Element { Eau, Feu, Elec, Toxic};
	public enum EffetPhysique{ pousser, attirer, bondir };
	public enum DispersionProjectile{ cone, droit, obus, rafale };
    public enum PatternSortDeJet { Unique, Rafale, SimultaneLigne, SimultaneTriangle, SimultaneCarre };

    public enum CustomProj1 { Normal/*, Courbe, Marqueur*/ };
    public enum CustomProj2 { Normal, MultiProj};

    public enum GabaritSortDeZone { Cercle, Ligne, Cone};

    public enum Character { Player, Enemy};

    public static Element getElemFromStr(string nom)
    {
        switch(nom)
        {
            case "Eau":
                return Element.Eau;
            case "Feu":
                return Element.Feu;
            case "Elec":
                return Element.Elec;
            case "Toxic":
                return Element.Toxic;
            default:
                return Element.Eau;
        }
    }

    public static CustomProj1 getCustom1FromString(string nom)
    {
        switch (nom)
        {
            case "Normal":
                return CustomProj1.Normal;
            default:
                return CustomProj1.Normal;
        }
    }

    public static CustomProj2 getCustom2FromString(string nom)
    {
        switch (nom)
        {
            case "Normal":
                return CustomProj2.Normal;
            case "MultiProj":
                return CustomProj2.MultiProj;
            default:
                return CustomProj2.Normal;
        }
    }
}