using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    // Primitives only.
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Corrdinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
