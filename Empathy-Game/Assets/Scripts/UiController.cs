using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController Instance;
    public Button DrawButton;
    public VisualElement SummeryPenal;
    public Button EndGameButton;
    [SerializeField] private TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        var root = GetComponent<UIDocument>().rootVisualElement;
        DrawButton = root.Q<Button>("Draw_BT");
        SummeryPenal = root.Q<VisualElement>("SummeryPanel");
        EndGameButton = root.Q<Button>("EndGame_BT");

        DrawButton.clicked += CardSlotsManager.InstanceSlotManager.DrawCard;
        GameLogicScript.OnStateChange += Showsummery;
        //EndGameButton.clicked += 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Showsummery(GameState state)
    {
        if (state == GameState.RoundEnd)
            SummeryPenal.visible = true;
        else
            SummeryPenal.visible = false;
    }

    private void OnDestroy()
    {
        GameLogicScript.OnStateChange -= Showsummery;
    }

    private void OnEndGameButtonClick()
    {

    }
}
