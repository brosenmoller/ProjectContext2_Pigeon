using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.H)) {
            animator.SetTrigger("Rest");
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            animator.SetTrigger("Flying");
        }
    }
}
