using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    public const float CROSS_FADE_TIME = 0.2f;

    CharacterManager character;
    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;

        if (isSprinting)
        {
            verticalAmount = 2;
        }

        character.Animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.Animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.ApplyRootMotion = applyRootMotion;
        character.Animator.CrossFade(targetAnimation, CROSS_FADE_TIME);
        character.IsPerformingAction = isPerformingAction;
        character.CanRotate = canRotate;
        character.CanMove = canMove;

        character.CharacterNetwork.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
