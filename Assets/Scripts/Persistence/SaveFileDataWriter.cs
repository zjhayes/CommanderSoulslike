using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool SaveFileExists()
    {
        return File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    public void DeleteSaveFile()
    {
        if (SaveFileExists())
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }
    }

    public void CreateNewSaveFile(CharacterSaveData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // Create save directory.
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating save file at: " + savePath);

            // Serialize into JSON.
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // Write file to system.
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("Unable to save character data. Game is not saved. " + savePath + "\n" + ex);
        }
    }

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (SaveFileExists())
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize JSON data.
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("File could not be loaded.\n" + ex);
            }
        }

        return characterData;
    }

}
