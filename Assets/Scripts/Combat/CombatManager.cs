using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private CombatQueue _combatQueue = new CombatQueue();
    private CombatBattleground _combatBattleground;

    public void StartCombat(CombatBattleground combatBattlefield)
    {
        _combatBattleground = combatBattlefield;
        StartNextCharacterTurn();
    }

    public Character GetCurrentCharacter()
    {
        return _combatQueue.Peek();
    }

    public void StartNextCharacterTurn()
    {
        if (_combatQueue.Count > 0)
        {
            Character previousCharacter = _combatQueue.Dequeue();
            previousCharacter.SetActive(false);
        }
        UpdateCombatQueue();
        GetCurrentCharacter().SetActive(true);
        EventManager.BroadcastSetFocusZoomedOut(GetCurrentCharacter().CombatSpot.WorldPosition);
    }

    private void UpdateCombatQueue()
    {
        if (_combatQueue.Count == 0)
        {
            foreach (Character character in _combatBattleground.GetAllCharacters())
            {
                _combatQueue.Enqueue(character);
            }
            _combatQueue.Shuffle();
        }
    }

    public void PerformCombatAction(CombatAction combatAction, Character targetCharacter)
    {
        StartCoroutine(combatAction.Execute(GetCurrentCharacter(), targetCharacter, OnPerformedCombatActionCallback));
    }

    public void OnPerformedCombatActionCallback()
    {
        StartNextCharacterTurn();
    }
}