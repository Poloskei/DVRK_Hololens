using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_selector : MonoBehaviour
{
    [SerializeField] GameObject nearmenu;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        foreach (var item in nearmenu.GetComponentsInChildren<Transform>())
        {
            //Debug.Log("Found child:" + item.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
