using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    CharacterController controller;
    Animator animator;
    CharacterNetworkManager characterNetworkManager;

    // Flags.
    bool isPerformingAction = false;
    bool isGrounded = true;
    bool isJumping = false;
    bool canRotate = true;
    bool canMove = true;
    bool applyRootMotion = false;

    public CharacterController Controller { get { return controller; } }
    public Animator Animator { get { return animator; } }
    public CharacterNetworkManager CharacterNetwork { get { return characterNetworkManager; } }
    public bool IsPerformingAction { get { return isPerformingAction; } set { isPerformingAction = value; } }
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool CanRotate { get { return canRotate; } set { canRotate = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool ApplyRootMotion { get { return applyRootMotion; } set { applyRootMotion = value; } }


    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);
        if (IsOwner)
        {
            characterNetworkManager.position.Value = transform.position;
            characterNetworkManager.rotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.position.Value, ref characterNetworkManager.positionVelocity, characterNetworkManager.positionSmoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, characterNetworkManager.rotation.Value, characterNetworkManager.rotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }
}
