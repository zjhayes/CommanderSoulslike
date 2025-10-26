using UnityEngine;
using TMPro;

public class UI_Save_Slot : MonoBehaviour
{
    SaveFileDataWriter saveFileWriter;

    [Header("Game Slot")]
    public CharacterSlot saveSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileWriter.saveFileName = PersistenceManager.Instance.GetSlotFileName(saveSlot);

        if (saveFileWriter.SaveFileExists())
        {
            characterName.text = PersistenceManager.Instance.characterSlot01.characterName;
        }
        else
        {
            // No save exists for this slot.
            gameObject.SetActive(false);
        }
    }

    public void LoadGameFromSaveSlot()
    {
        PersistenceManager.Instance.CurrentSlot = saveSlot;
        PersistenceManager.Instance.LoadGame();
    }
}
