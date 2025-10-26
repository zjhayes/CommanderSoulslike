using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadGameMenu;

    [Header("Buttons")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;


    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        PersistenceManager.Instance.CreateNewGame();
        StartCoroutine(PersistenceManager.Instance.LoadWorld());
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
}
