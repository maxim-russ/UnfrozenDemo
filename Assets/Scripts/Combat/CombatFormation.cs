using System.Collections.Generic;

[System.Serializable]
public class CombatFormation
{
    public List<CombatFormationSpot> Spots { get; private set; } = new List<CombatFormationSpot>();
}