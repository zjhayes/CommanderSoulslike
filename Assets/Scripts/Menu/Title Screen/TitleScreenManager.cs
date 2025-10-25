using UnityEngine;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        PersistenceManager.Instance.CreateNewGame();
        StartCoroutine(PersistenceManager.Instance.LoadWorld());
    }
}
