using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerAnimatorManager animatorManager;
    PlayerLococationManager locomotionManager;
    PlayerNetworkManager playerNetworkManager;

    public PlayerAnimatorManager AnimatorManager {  get { return animatorManager; } }
    public PlayerLococationManager LocomotionManager {  get { return locomotionManager; } }
    public PlayerNetworkManager PlayerNetwork { get { return playerNetworkManager; } }

    protected override void Awake()
    {
        base.Awake();

        animatorManager = GetComponent<PlayerAnimatorManager>();
        locomotionManager = GetComponent<PlayerLococationManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        locomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if(!IsOwner) return;
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(IsOwner)
        {
            PlayerCamera.Instance.Player = this;
            PlayerInputManager.Instance.Player = this;
        }
    }
}
