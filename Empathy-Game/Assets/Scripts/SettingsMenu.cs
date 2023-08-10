using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    Button mainButton;
    SettingsMenuItem[] menuItems;
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemCount;

    void Start()
    {
        itemCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemCount];
        for(int i = 0; i < itemCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.transform.position;

        ResetPosition();
    }

    void ResetPosition()
    {
        for(int i = 0; i < itemCount; i++)
        {
            menuItems[i].trans.position = mainButtonPosition;
        }
    }

    void ToggleMenu()
    {
        Debug.Log("Enter OnClick");
        isExpanded = !isExpanded;

        if(isExpanded)
        {
            for (int i = 0; i < itemCount; i++)
            {
                menuItems[i].trans.position = mainButtonPosition + spacing * (i + 1);
            }
        }
        else
        {
            for (int i = 0; i < itemCount; i++)
            {
                menuItems[i].trans.position = mainButtonPosition;
            }
        }
    }

    private void OnDestroy()
    {
        mainButton.onClick.RemoveListener(ToggleMenu);
    }
}
