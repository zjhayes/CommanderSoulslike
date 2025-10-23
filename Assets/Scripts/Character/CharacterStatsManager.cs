using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [SerializeField]
    float staminaRegenerationDelay = 2;
    [SerializeField]
    float staminaRegenerationAmount = 2;

    CharacterManager character;
    float staminaRegenerationTimer = 0;
    float staminaTickTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;

        if (character.CharacterNetwork.isSprinting.Value)
            return;

        if (character.IsPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.CharacterNetwork.currentStamina.Value < character.CharacterNetwork.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.CharacterNetwork.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }

    }

    public virtual void ResetStaminaRegenTimer(float oldValue, float newValue)
    {
        if (newValue < oldValue)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
