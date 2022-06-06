using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;

[DisallowMultipleComponent]
[RequireComponent(typeof(SkeletonAnimation), typeof(BoxCollider2D))]
public class Character : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CharacterHUD _hud;
    [SerializeField]
    private SkeletonAnimation _skeletonAnimation;
    [Header("Character definitions")]
    [SpineAnimation]
    [SerializeField]
    private string _gettingDamagedAnimation;
    [SpineAnimation]
    [SerializeField]
    private string _idleAnimation;

    [field: SerializeField]
    public CombatAction[] CombatActions { get; private set; }

    public CombatFormationSpot CombatSpot { get; private set; }

    public void AssignCombatSpot(CombatFormationSpot combatSpot)
    {
        CombatSpot = combatSpot;
        UpdateDirection(combatSpot.Direction);
        UpdatePosition(combatSpot.WorldPosition, 0);
    }

    public void UpdatePosition(Vector3 worldPosition, float animationDurationInSeconds)
    {
        if (animationDurationInSeconds == 0)
        {
            transform.position = worldPosition;
        }
        else
        {
            StartCoroutine(UpdatePositionAnimated(worldPosition, animationDurationInSeconds));
        }
    }

    private IEnumerator UpdatePositionAnimated(Vector3 newPosition, float animationDurationInSeconds)
    {
        Vector3 currentPosition = transform.position;
        for (float progress = 0; progress <= 1; progress += (Time.deltaTime / animationDurationInSeconds))
        {
            transform.position = Vector3.Lerp(currentPosition, newPosition, Mathf.SmoothStep(0, 1, progress));
            yield return null;
        }
    }

    public void UpdateDirection(float direction)
    {
        _skeletonAnimation.skeleton.ScaleX = direction;
    }

    public Spine.TrackEntry PlayAnimation(string animation)
    {
        Spine.TrackEntry entry = _skeletonAnimation.AnimationState.SetAnimation(0, animation, false);
        QueueIdleAnimation();
        return entry;
    }

    public Spine.TrackEntry PlayGettingDamagedAnimation()
    {
        return PlayAnimation(_gettingDamagedAnimation);
    }

    private void QueueIdleAnimation()
    {
        _skeletonAnimation.AnimationState.AddAnimation(0, _idleAnimation, true, 0);
    }

    // Crude fix: SkeletonAnimation almost never survives Serialization, but sometimes do. What?
    private void EnsureSkeletonAnimationValidity()
    {
        if (!_skeletonAnimation.valid)
        {
            _skeletonAnimation.Initialize(true);
            UpdateDirection(CombatSpot.Direction);
        }
    }

    public void SetActive(bool status)
    {
        _hud.SetFocused(status);
    }

    private void OnEnable()
    {
        EnsureSkeletonAnimationValidity();
        QueueIdleAnimation();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        EventManager.BroadcastCharacterSelected(this);
    }
}