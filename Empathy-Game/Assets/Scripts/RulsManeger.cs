using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulsManeger : MonoBehaviour
{
    public static RulsManeger Singelton;

    public GameObject rulsPrefab;
    public Transform canva;
    void Start()
    {
        RulsManeger.Singelton = this;
    }

    public void ShowRuls()
    {
        Instantiate(rulsPrefab, canva.transform);
    }

    public void CloseRuls()
    {
        Destroy(rulsPrefab.gameObject);
    }


    
}
