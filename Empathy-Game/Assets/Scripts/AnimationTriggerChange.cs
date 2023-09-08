using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTriggerChange : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool isOpen = false;

    // Start is called before the first frame update
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    image = GetComponent<Image>();
    //}

    public void ButtonAnimation()
    {
        if(!isOpen)
        {
            animator.ResetTrigger("Close");
            animator.SetTrigger("Open");
            isOpen = true;
        }
        else
        {
            animator.ResetTrigger("Open");
            animator.SetTrigger("Close");
            isOpen = false;
        }
    }
}
