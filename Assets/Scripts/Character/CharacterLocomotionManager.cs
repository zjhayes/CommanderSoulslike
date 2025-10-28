using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check & Jumping")]
    [SerializeField] float gravityForce = -5.55f;
    [SerializeField] float groundCheckSphereRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] protected Vector3 yVelocity; // Upward force.
    [SerializeField] protected float groundedVelocity = -20; // Gravity while grounded.
    [SerializeField] protected float fallStartYVelocity = -5;

    protected bool fallVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update() 
    {
        HandleGroundCheck();

        if (character.IsGrounded)
        {
            // Check if attempting to jump.
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallVelocityHasBeenSet = false;
                yVelocity.y = groundedVelocity;
            }
        }
        else
        {
            if (!character.IsJumping && !fallVelocityHasBeenSet)
            {
                fallVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;
            character.Animator.SetFloat("InAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;

        }

        // Apply vertical force.
        character.Controller.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.IsGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}
