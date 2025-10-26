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

    [Header("Pop Ups")]
    [SerializeField] GameObject noSaveSlotsPopUp;
    [SerializeField] Button noSaveSlotsOkayButton;

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
        mainMenu.SetActive(false);
        noSaveSlotsPopUp.SetActive(true);
        noSaveSlotsOkayButton.Select();
    }

    public void CloseNoFreeSaveSlotsPopUp()
    {
        noSaveSlotsPopUp.SetActive(false);
        mainMenu.SetActive(true);
        mainMenuNewGameButton.Select();
    }
}
