using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testShooterController : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRun", false);
        animator.SetBool("isDown", false);
        animator.SetBool("isOver", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("isRun",true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isRun", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isDown", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("isDown", false);
        }
    }
}
