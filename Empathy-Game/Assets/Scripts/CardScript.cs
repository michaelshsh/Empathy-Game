using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Start is called before the first frame update 

    public bool drag;
    public SpriteRenderer mySpritRenderer;
    public TextMeshPro Time;
    public TextMeshPro PersonalPoints;
    public TextMeshPro TeamPoints;
    public TextMeshPro FreeText;

    void Start()
    {
        Time.text = "1:30";
        PersonalPoints.text = "+15";
        TeamPoints.text = "0";
        FreeText.text = "Hello im a free text let me live";
    }

    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            worldPosition.z = 0;
            gameObject.transform.position = (gameObject.transform.position + worldPosition) / 2;
            
        }
    }

    private void OnMouseDown()
    {
        drag = true;
        gameObject.transform.position += new Vector3(0, 0, -1);
    }

    private void OnMouseUp()
    {
        drag = false;
        gameObject.transform.position += new Vector3(0, 0, 1);
    }

}
