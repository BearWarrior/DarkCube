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

    public enum GabaritSortDeZone { Cercle, Ligne, Cone};

    public enum Character { Player, Enemy};
}