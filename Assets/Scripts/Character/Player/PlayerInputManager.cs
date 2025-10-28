using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    PlayerControls playerControls;
    PlayerManager player;

    [Header("Camera Movement Inputs")]
    [SerializeField] Vector2 cameraInput;
    float cameraVerticalInput;
    float cameraHorizontalInput;

    [Header("Player Movement Inputs")]
    [SerializeField] Vector2 movementInput;
    float verticalInput;
    float horizontalInput;
    float moveAmount;

    [Header("Player Action Input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;

    public PlayerManager Player {  get { return player; } set { player = value; } }
    public float VerticalInput { get { return verticalInput; } }
    public float HorizontalInput { get { return horizontalInput; } }
    public float MoveAmount { get { return moveAmount; } }
    public float CameraVerticalInput {  get { return cameraVerticalInput; } }
    public float CameraHorizontalInput { get { return cameraHorizontalInput; } }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == PersistenceManager.Instance.WorldSceneIndex)
        {
            // Enable controls in world scenes.
            Instance.enabled = true;
        }
        else
        {
            Instance.enabled = false; 
        }
    }

    private void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null)
            return;

        player.AnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.PlayerNetwork.isSprinting.Value);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            player.LocomotionManager.AttemptToDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprintInput)
        {
            player.LocomotionManager.HandleSprinting();
        }
        else
        {
            player.PlayerNetwork.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            player.LocomotionManager.AttemptToJump();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
