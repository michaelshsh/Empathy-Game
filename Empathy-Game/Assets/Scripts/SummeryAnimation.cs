using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummeryAnimation : MonoBehaviour
{
    public static SummeryAnimation Singelton;

    public Transform box;
    public CanvasGroup background;
    public GameObject summeryGroup;
    public TextMeshProUGUI SummeryText;
    public Button MainMenuButton;

    private void Awake()
    {
        SummeryAnimation.Singelton = this;
    }
    public void OnOpeningWindow()
    {
        summeryGroup.SetActive(true);

        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void OnClosingWindow()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        summeryGroup.SetActive(false);
    }

    public void ButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
