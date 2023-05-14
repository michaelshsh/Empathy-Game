using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummeryScript : MonoBehaviour
{
    public static SummeryScript InstanceSummeryManager { get; private set; }
    
    private void Awake()
    {
        if (InstanceSummeryManager != null && InstanceSummeryManager != this)
        {
            Destroy(this);
        }
        else
        {
            InstanceSummeryManager = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InstanceSummeryManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
