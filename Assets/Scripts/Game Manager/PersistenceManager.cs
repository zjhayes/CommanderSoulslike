using System;
using System.Collections;
using System.Collections.Generic;
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
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;

    private string fileName;
    private SaveFileDataWriter saveFileDataWriter;
    private Dictionary<CharacterSlot, Action<CharacterSaveData>> characterSlotSetters;

    protected override void Awake()
    {
        base.Awake();

        characterSlotSetters = new Dictionary<CharacterSlot, Action<CharacterSaveData>>
        {
            { CharacterSlot.CharacterSlot_01, data => characterSlot01 = data },
            { CharacterSlot.CharacterSlot_02, data => characterSlot02 = data },
            { CharacterSlot.CharacterSlot_03, data => characterSlot03 = data },
            { CharacterSlot.CharacterSlot_04, data => characterSlot04 = data },
            { CharacterSlot.CharacterSlot_05, data => characterSlot05 = data },
            { CharacterSlot.CharacterSlot_06, data => characterSlot06 = data },
            { CharacterSlot.CharacterSlot_07, data => characterSlot07 = data },
            { CharacterSlot.CharacterSlot_08, data => characterSlot08 = data },
            { CharacterSlot.CharacterSlot_09, data => characterSlot09 = data },
            { CharacterSlot.CharacterSlot_10, data => characterSlot10 = data },
        };
    }

    private void Start()
    {
        LoadAllSaveSlots();
    }

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

    public string GetSlotFileName(CharacterSlot currentSlot)
    {
        return CharacterSlotNames.TryGetValue(currentSlot, out var name)
            ? name
            : "UnknownSlot";
    }

    private void DecideCharacterFileName()
    {
        fileName = GetSlotFileName(currentSlot);
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

    private void LoadAllSaveSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter
        {
            saveDataDirectoryPath = Application.persistentDataPath
        };

        foreach (CharacterSlot slot in Enum.GetValues(typeof(CharacterSlot)))
        {
            saveFileDataWriter.saveFileName = GetSlotFileName(slot);
            CharacterSaveData saveData = saveFileDataWriter.LoadSaveFile();

            if (characterSlotSetters.TryGetValue(slot, out var setter))
            {
                setter(saveData);
            }
            else
            {
                Debug.LogWarning($"No field mapped for {slot}");
            }
        }
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

    private static readonly Dictionary<CharacterSlot, string> CharacterSlotNames = new()
    {
        { CharacterSlot.CharacterSlot_01, "CharacterSlot_01" },
        { CharacterSlot.CharacterSlot_02, "CharacterSlot_02" },
        { CharacterSlot.CharacterSlot_03, "CharacterSlot_03" },
        { CharacterSlot.CharacterSlot_04, "CharacterSlot_04" },
        { CharacterSlot.CharacterSlot_05, "CharacterSlot_05" },
        { CharacterSlot.CharacterSlot_06, "CharacterSlot_06" },
        { CharacterSlot.CharacterSlot_07, "CharacterSlot_07" },
        { CharacterSlot.CharacterSlot_08, "CharacterSlot_08" },
        { CharacterSlot.CharacterSlot_09, "CharacterSlot_09" },
        { CharacterSlot.CharacterSlot_10, "CharacterSlot_10" },
    };
}
