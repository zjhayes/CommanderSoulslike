using UnityEngine;
using UnityEngine.TextCore.Text;

public class ResetIsJumping : StateMachineBehaviour
{
    CharacterManager character;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null)
        {
            character = animator.GetComponent<CharacterManager>();
        }

        // Clear character action flag.
        character.IsJumping = false;
    }
}
