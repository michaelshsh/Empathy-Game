using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerProperties : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
