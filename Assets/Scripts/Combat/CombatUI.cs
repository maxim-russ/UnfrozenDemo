using UnityEngine;

public class CombatUI : MonoBehaviour
{
    private CombatManager _combatManager;

    private CombatAction _selectedCombatAction;

    public void InitializeUI(CombatManager combatManager)
    {
        _combatManager = combatManager;
    }

    public void OnAttackOneButtonClicked()
    {
        _selectedCombatAction = _combatManager.GetCurrentCharacter().CombatActions[0];
    }

    public void OnAttackTwoButtonClicked()
    {
        _selectedCombatAction = _combatManager.GetCurrentCharacter().CombatActions[1];
    }

    public void OnSkipButtonClicked()
    {
        _combatManager.StartNextCharacterTurn();
    }

    public void OnCharacterSelectedCallback(Character selectedCharacter)
    {
        if (_selectedCombatAction == null)
        {
            Debug.Log("ACTION NOT SELECTED");
            return;
        }

        if (_selectedCombatAction.IsValid(_combatManager.GetCurrentCharacter(), selectedCharacter))
        {
            _combatManager.PerformCombatAction(_selectedCombatAction, selectedCharacter);
            _selectedCombatAction = null;
        }
        else
        {
            Debug.Log("INVALID TARGET");
        }
    }

    private void OnEnable()
    {
        EventManager.OnCharacterSelected += OnCharacterSelectedCallback;
    }

    private void OnDisable()
    {
        EventManager.OnCharacterSelected -= OnCharacterSelectedCallback;
    }
}