using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : Singleton<PersistenceManager>
{
    [SerializeField] int worldSceneIndex = 1;
    [SerializeField] CharacterSlot currentSlot;
    [SerializeField] CharacterSaveData currentGameData;
    [SerializeField] private PlayerManager player;

    [SerializeField]  bool saveGame = false;
    [SerializeField]  bool loadGame = false;

    [Header("Character Slots")]
    [SerializeField] CharacterSaveData characterSlot01;
    [SerializeField] CharacterSaveData characterSlot02;
    [SerializeField] CharacterSaveData characterSlot03;
    [SerializeField] CharacterSaveData characterSlot04;
    [SerializeField] CharacterSaveData characterSlot05;
    [SerializeField] CharacterSaveData characterSlot06;
    [SerializeField] CharacterSaveData characterSlot07;
    [SerializeField] CharacterSaveData characterSlot08;
    [SerializeField] CharacterSaveData characterSlot09;
    [SerializeField] CharacterSaveData characterSlot10;

    private string fileName;
    private SaveFileDataWriter saveFileDataWriter;

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }
        else if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    private void DecideCharacterFileName()
    {
        switch (currentSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "CharacterSlot_10";
                break;
            default:
                throw new System.Exception("Unable to determine file slot.");
        }
    }
    
    public void CreateNewGame()
    {
        DecideCharacterFileName();
        currentGameData = new CharacterSaveData();
    }

    public void LoadGame()
    {
        DecideCharacterFileName();
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;
        currentGameData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorld());
    }

    public void SaveGame()
    {
        DecideCharacterFileName();
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;

        player.PullPlayerData(ref currentGameData);

        saveFileDataWriter.CreateNewSaveFile(currentGameData);
    }
    
    public IEnumerator LoadWorld()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        yield return null;
    }

    public int WorldSceneIndex
    { 
        get { return worldSceneIndex; } 
    }
}
