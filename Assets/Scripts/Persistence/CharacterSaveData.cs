using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Data")]
    public int sceneIndex = 1;

    // Primitives only.
    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Corrdinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
