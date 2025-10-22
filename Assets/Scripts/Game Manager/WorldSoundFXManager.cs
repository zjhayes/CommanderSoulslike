using UnityEngine;

public class WorldSoundFXManager : Singleton<WorldSoundFXManager>
{
    [Header("Action Sounds")]
    [SerializeField] AudioClip rollSFX;

    public AudioClip RollSoundFX { get { return rollSFX; } }
}
