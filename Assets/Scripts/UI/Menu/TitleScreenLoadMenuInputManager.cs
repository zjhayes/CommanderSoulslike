using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteSaveSlot = false;

    private void Update()
    {
        if (deleteSaveSlot)
        {
            deleteSaveSlot = false;
            TitleScreenManager.Instance.AttemptToDeleteSaveSlot();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.UI.Delete.performed += i => deleteSaveSlot = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
