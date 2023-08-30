using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerChange : MonoBehaviour
{
    private Animator animator;
    private bool isOpend = true;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void OnClicked()
    {
        
        if (isOpend)
        {
            Debug.Log("trigger Open");
            animator.ResetTrigger("Close");
            animator.SetTrigger("Open");
            isOpend = false;
        }
        else
        {
            Debug.Log("trigger Close");
            animator.ResetTrigger("Open");
            animator.SetTrigger("Close");
            isOpend = true;
        }
       

    }
}
