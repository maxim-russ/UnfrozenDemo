using System.Collections;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName = "Action", menuName = "ScriptableObjects/CombatAction", order = 1)]
public class CombatAction : ScriptableObject
{
    [System.Serializable]
    public class CombatActionEvent
    {
        // Accessing parent's (CombatAction) `_character` field.
        [SpineEvent(dataField: "_character", fallbackToTextField: true)]
        [SerializeField]
        private string _triggerEvent;
        public string TriggerEvent { get => _triggerEvent; }
        // Can be heavily extended as to which targets in formation it affects,
        // what kind of [de]buffs it imposes, etc.
    }

    [System.Serializable]
    public class CombatActionTargets
    {
        [SerializeField]
        private bool[] _allies;
        [SerializeField]
        private bool[] _enemies;

        public bool CanTarget(int position, bool isEnemy)
        {
            return IsPositionTargetable(position, isEnemy ? _enemies : _allies);
        }

        private bool IsPositionTargetable(int position, bool[] formationTargets)
        {
            return (formationTargets != null) && (formationTargets.Length > position) && formationTargets[position];
        }
    }

    // Make sure, when actions are performed, their participants are rendered on top of all other sprites.
    private readonly Vector3 ActionPlaneOrigin = new Vector3(0, 0, -1);
    // The point where the camera will be focused when actions are performed.
    private readonly Vector2 ActionFocusPoint = new Vector2(3, 0);
    private const float ActionTransitionTime = 0.4f;

    [SerializeField]
    private SkeletonAnimation _character;

    [Header("Action definitions")]
    [SpineAnimation(dataField: "_character", fallbackToTextField: true)]
    [SerializeField]
    private string _animation;
    [Tooltip("The distance between the character performing this action and its targets.")]
    [SerializeField]
    private float _gap;
    [Tooltip("Formation positions this action is allowed to be performed upon.")]
    [SerializeField]
    private CombatActionTargets _targets;
    [Tooltip("Action events (such as hits, buffs, ...), triggered by their corresponding animation events.")]
    [SerializeField]
    private CombatActionEvent[] _events;

    public bool IsValid(Character actionInitiator, Character actionTarget)
    {
        CombatFormationSpot initiatorCombatSpot = actionInitiator.CombatSpot;
        CombatFormationSpot targetCombatSpot = actionTarget.CombatSpot;
        bool isEnemy = (initiatorCombatSpot.Team != targetCombatSpot.Team);
        return _targets.CanTarget(targetCombatSpot.Number, isEnemy);
    }

    private CombatActionEvent GetActionEvent(string animationEvent)
    {
        foreach (var actionEvent in _events)
        {
            if (actionEvent.TriggerEvent == animationEvent)
            {
                return actionEvent;
            }
        }
        return null;
    }

    private void TransitionIn(Character actionInitiator, Character actionTarget)
    {
        EventManager.BroadcastSetFocusZoomedIn(ActionFocusPoint);

        Vector3 newInitiatorPosition = ActionPlaneOrigin;
        Vector3 newTargetPosition = ActionPlaneOrigin;
        newInitiatorPosition.x -= actionInitiator.CombatSpot.Direction * _gap / 2;
        newTargetPosition.x += actionInitiator.CombatSpot.Direction * _gap / 2;
        actionInitiator.UpdatePosition(newInitiatorPosition, ActionTransitionTime);
        actionTarget.UpdatePosition(newTargetPosition, ActionTransitionTime);
    }

    private void TransitionOut(Character actionInitiator, Character actionTarget)
    {
        actionInitiator.UpdatePosition(actionInitiator.CombatSpot.WorldPosition, ActionTransitionTime);
        actionTarget.UpdatePosition(actionTarget.CombatSpot.WorldPosition, ActionTransitionTime);

        EventManager.BroadcastSetFocusZoomedOut(ActionPlaneOrigin);
    }

    public IEnumerator Execute(Character actionInitiator, Character actionTarget, System.Action onActionFinished)
    {
        TransitionIn(actionInitiator, actionTarget);

        Spine.TrackEntry animationTrack = actionInitiator.PlayAnimation(_animation);
        animationTrack.Event += (Spine.TrackEntry trackEntry, Spine.Event e) =>
        {
            if (GetActionEvent(e.Data.Name) != null)
            {
                actionTarget.PlayGettingDamagedAnimation();
            }
        };

        while (!animationTrack.IsComplete)
        {
            yield return null;
        }

        TransitionOut(actionInitiator, actionTarget);
        yield return new WaitForSeconds(ActionTransitionTime);

        onActionFinished();
    }
}