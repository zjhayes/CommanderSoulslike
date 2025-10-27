using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreenManager : Singleton<TitleScreenManager>
{
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadGameMenu;

    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button noSaveSlotsOkayButton;
    [SerializeField] Button deleteSaveSlotPopUpConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] GameObject noSaveSlotsPopUp;
    [SerializeField] GameObject deleteSaveSlotPopUp;

    [Header("Save Slots")]
    [SerializeField] CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

    public CharacterSlot SelectedSlot { get { return currentSelectedSlot; } set { currentSelectedSlot = value; } }

    protected override void Awake()
    {
        persistent = false; // Should be destroyed on world load.
        base.Awake();
    }
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        PersistenceManager.Instance.AttemptToCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        mainMenu.SetActive(false);
        loadGameMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        loadGameMenu.SetActive(false);
        mainMenu.SetActive(true);
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeSaveSlotsPopUp()
    {
        noSaveSlotsPopUp.SetActive(true);
        noSaveSlotsOkayButton.Select();
    }

    public void CloseNoFreeSaveSlotsPopUp()
    {
        noSaveSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

    public void SelectSaveSlot(CharacterSlot slot)
    {
        currentSelectedSlot = slot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteSaveSlot()
    {
        if (currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteSaveSlotPopUp.SetActive(true);
            deleteSaveSlotPopUpConfirmButton.Select();
        }
    }

    public void DeleteSaveSlot()
    {
        deleteSaveSlotPopUp.SetActive(false);
        PersistenceManager.Instance.DeleteGame(currentSelectedSlot);
        RefreshSaveSlotList();
        loadMenuReturnButton.Select();
    }

    public void CloseDeleteSaveSlotPopUp()
    {
        deleteSaveSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

    private void RefreshSaveSlotList()
    {
        loadGameMenu.SetActive(false);
        loadGameMenu.SetActive(true);
    }
}
