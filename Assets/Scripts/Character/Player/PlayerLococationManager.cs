using UnityEngine;

public class PlayerLococationManager : CharacterLocomotionManager
{ 
    PlayerManager player;
    
    float verticalMovement;
    float horizontalMovement;
    float moveAmount;

    Vector3 moveDirection;
    Vector3 targetRotationDirection = Vector3.zero;

    [Header("Movement Settings")]
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float sprintingStaminaCost = 2;

    [Header("Dodge")]
    [SerializeField] Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 5;
    [SerializeField] float jumpStaminaCost = 5;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.PlayerNetwork.horizontalMovement.Value = horizontalMovement;
            player.PlayerNetwork.verticalMovement.Value = verticalMovement;
            player.PlayerNetwork.moveAmount.Value = moveAmount;
        }
        else
        {
            horizontalMovement = player.PlayerNetwork.horizontalMovement.Value;
            verticalMovement = player.PlayerNetwork.verticalMovement.Value;
            moveAmount = player.PlayerNetwork.moveAmount.Value;

            player.AnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.PlayerNetwork.isSprinting.Value);
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.Instance.VerticalInput;
        horizontalMovement = PlayerInputManager.Instance.HorizontalInput;
        moveAmount = PlayerInputManager.Instance.MoveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (!player.CanMove)
            return;

        GetMovementValues();
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.PlayerNetwork.isSprinting.Value)
        {
            player.Controller.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.Instance.MoveAmount > 0.5f)
            {
                // Move at running speed
                player.Controller.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else
            {
                // Move at walking speed
                player.Controller.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleRotation()
    {
        if (!player.CanRotate)
            return;

        targetRotationDirection = PlayerCamera.Instance.Camera.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.Camera.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
        if (player.IsPerformingAction)
        {
            player.PlayerNetwork.isSprinting.Value = false;
            return;
        }

        if (player.PlayerNetwork.currentStamina.Value <= 0)
        {
            player.PlayerNetwork.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5)
        {
            player.PlayerNetwork.isSprinting.Value = true;
        }
        else
        {
            player.PlayerNetwork.isSprinting.Value = false;
        }

        if (player.PlayerNetwork.isSprinting.Value)
        {
            player.PlayerNetwork.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }

    public void AttemptToDodge()
    {
        if (player.IsPerformingAction)
            return;

        if (player.PlayerNetwork.currentStamina.Value <= 0)
            return;

        if (moveAmount > 0)
        {
            // Moving, perform roll.
            rollDirection = PlayerCamera.Instance.Camera.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.Instance.Camera.transform.right * horizontalMovement;

            rollDirection.y = 0;
            rollDirection.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.AnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true);
        }
        else
        {
            // Stationary, backstep.
            player.AnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
        }

        player.PlayerNetwork.currentStamina.Value -= dodgeStaminaCost;
    }

    public void AttemptToJump()
    {
        if (player.IsPerformingAction)
            return;

        if (player.PlayerNetwork.currentStamina.Value <= 0)
            return;

        if (player.IsJumping)
            return;

        if (!player.IsGrounded)
            return;

        player.AnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

        player.IsJumping = true;

        player.PlayerNetwork.currentStamina.Value -= jumpStaminaCost;
    }

    public void ApplyJumpingVelocity()
    {

    }
}
