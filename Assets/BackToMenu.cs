using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    [SerializeField] GameObject NearMenu;
    [SerializeField] GameObject ListMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        NearMenu.SetActive(true);
        ListMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
