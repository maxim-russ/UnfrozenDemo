using UnityEngine;

[System.Serializable]
public class CombatFormationSpot
{
    public int Team { get; private set; }
    public int Number { get; private set; }
    public float Direction { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Character Character { get; private set; }

    public CombatFormationSpot(int team, int number, float direction, Vector3 worldPosition)
    {
        Team = team;
        Number = number;
        Direction = direction;
        WorldPosition = worldPosition;
    }

    public void AssignCharacter(Character character)
    {
        Character = character;
    }
}