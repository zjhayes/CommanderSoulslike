using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerAnimatorManager animatorManager;
    PlayerLococationManager locomotionManager;
    PlayerNetworkManager playerNetworkManager;
    PlayerStatsManager playerStatsManager;

    public PlayerAnimatorManager AnimatorManager {  get { return animatorManager; } }
    public PlayerLococationManager LocomotionManager {  get { return locomotionManager; } }
    public PlayerNetworkManager PlayerNetwork { get { return playerNetworkManager; } }
    public PlayerStatsManager PlayerStats { get { return playerStatsManager; } }

    protected override void Awake()
    {
        base.Awake();

        animatorManager = GetComponent<PlayerAnimatorManager>();
        locomotionManager = GetComponent<PlayerLococationManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        locomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
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

            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.HUD.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            PlayerUIManager.Instance.HUD.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }
    }

    public void PullPlayerData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;
    }

    public void PushPlayerData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;

        Vector3 playerPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = playerPosition;
    }
}
