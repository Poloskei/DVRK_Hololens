using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MixedReality.Toolkit.UX;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class data_selector : MonoBehaviour
{
    [SerializeField] GameObject NearMenu;
    [SerializeField] GameObject ListMenu;
    [SerializeField] string PatientsFolder;
    [SerializeField] GameObject ActionButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        string[] fileList = Directory.GetFiles(PatientsFolder);
        
        foreach (string file in fileList)
        {
            //get filenames
            string label = CutPatientFile(file);
            Debug.Log("found patient: " + label);
            //create button for each filename
            GameObject dataButton = Instantiate(ActionButtonPrefab, new Vector3(0,0,0), Quaternion.identity);
            dataButton.transform.SetParent(ListMenu.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
            //set the buttons label to the name of the file
            foreach (var child in dataButton.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (child.gameObject.name == "Label")
                {
                    child.gameObject.SetActive(true);
                    child.text = label;

                }
            }
        }

        NearMenu.SetActive(false);
        ListMenu.SetActive(true);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string CutPatientFile(string patient)
    {
        return patient.Remove(0,PatientsFolder.Length+1);
    }
}
