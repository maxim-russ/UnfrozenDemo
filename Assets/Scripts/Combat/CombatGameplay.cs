using UnityEngine;

public class CombatGameplay : MonoBehaviour
{
    [SerializeField]
    private Character _minerPrefab;
    [SerializeField]
    private CombatUI _combatUIPrefab;

    private void Start()
    {
        CombatBattleground combatBattleground = new GameObject("CombatBattleground", typeof(CombatBattleground)).GetComponent<CombatBattleground>();
        combatBattleground.transform.parent = transform;
        combatBattleground.CreateFormations(2);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Character character = Instantiate(_minerPrefab, Vector3.zero, Quaternion.identity);
                combatBattleground.AddCharacter(character, i);
            }
        }

        CombatManager combatManager = new GameObject("CombatManager", typeof(CombatManager)).GetComponent<CombatManager>();
        combatManager.transform.parent = transform;
        combatManager.StartCombat(combatBattleground);

        CombatUI combatUI = Instantiate(_combatUIPrefab, Vector3.zero, Quaternion.identity, transform);
        combatUI.name = "CombatUI";
        combatUI.InitializeUI(combatManager);
    }
}