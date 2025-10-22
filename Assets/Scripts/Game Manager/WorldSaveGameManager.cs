using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : Singleton<WorldSaveGameManager>
{
    [SerializeField] int worldSceneIndex = 1;

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }

    public int WorldSceneIndex
    { 
        get { return worldSceneIndex; } 
    }
}
