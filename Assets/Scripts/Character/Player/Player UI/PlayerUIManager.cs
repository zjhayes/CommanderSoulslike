using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : Singleton<PlayerUIManager>
{
    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    PlayerUIHudManager hudManager;

    public PlayerUIHudManager HUD {  get { return hudManager; } }

    protected override void Awake()
    {
        base.Awake();

        hudManager = GetComponent<PlayerUIHudManager>();
    }

    private void Update()
    {
        if(startGameAsClient)
        {
            startGameAsClient = false;
            // Shut down and start as client as title screen starts as host.
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}
