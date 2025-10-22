using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<CharacterManager>();
        }

        // Clear character action flag.
        character.IsPerformingAction = false;
        character.CanMove = true;
        character.CanRotate = true;
        character.ApplyRootMotion = false;
    }
}
