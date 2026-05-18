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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("IsRun",true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("IsRun", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsDown", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("IsRun", false);
        }
    }
}
