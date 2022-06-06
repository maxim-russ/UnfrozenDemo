using System.Collections.Generic;
using UnityEngine;

public class CombatBattleground : MonoBehaviour
{
    private const float DistanceBetweenFormations = 8f;
    private const float DistanceBetweenCharacters = 5f;

    private List<CombatFormation> _formations;

    public void CreateFormations(int numberOfTeams)
    {
        _formations = new List<CombatFormation>();
        for (int i = 0; i < numberOfTeams; i++)
        {
            _formations.Add(new CombatFormation());
        }
    }

    public CombatFormationSpot CreateFormationSpot(int team)
    {
        int spotNumber = _formations[team].Spots.Count;
        float spotDirection = (team == 0) ? 1 : -1;
        float spotHorizontalPosition = -spotDirection * ((DistanceBetweenFormations / 2) + (DistanceBetweenCharacters * spotNumber));
        var spotWorldPosition = new Vector3(spotHorizontalPosition, 0, 0);

        var spot = new CombatFormationSpot(team, spotNumber, spotDirection, spotWorldPosition);

        _formations[team].Spots.Add(spot);
        return spot;
    }

    public void AddCharacter(Character character, int team)
    {
        character.transform.parent = transform;

        CombatFormationSpot spot = CreateFormationSpot(team);
        spot.AssignCharacter(character);
        character.AssignCombatSpot(spot);
    }

    public IEnumerable<Character> GetAllCharacters()
    {
        foreach (CombatFormation formation in _formations)
        {
            foreach (CombatFormationSpot spot in formation.Spots)
            {
                yield return spot.Character;
            }
        }
    }
}