using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;
    bool isSprinting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleSprint();
        HandleCombatRoll();
    }

    void HandleMovement()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

    void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            animator.SetBool("isSprinting", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            animator.SetBool("isSprinting", false);
        }
    }

    void HandleCombatRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("combatRoll");
            PerformCombatRoll();
        }
    }

    void PerformCombatRoll()
    {
        // Simulate a forward roll by moving quickly in the forward direction
        Vector3 rollDirection = transform.forward * 2f; // Adjust the distance and speed
        transform.Translate(rollDirection, Space.World);
    }
}
