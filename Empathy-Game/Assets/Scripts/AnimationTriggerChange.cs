using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTriggerChange : MonoBehaviour
{
    private Animator animator;
    private Image image;
    public Sprite upImg;
    public Sprite downImg;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("opend"))
        {
            animator.SetTrigger("Open");
            image.sprite = downImg;
        }
        else
        {
            animator.SetTrigger("Close");
            image.sprite = upImg;
        }

    }
}
