using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MixedReality.Toolkit.UX;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityVolumeRendering;
using MixedReality.Toolkit.SpatialManipulation;



public class DataSelector : MonoBehaviour
{
    [SerializeField] GameObject NearMenu;
    [SerializeField] GameObject ListMenu;
    [SerializeField] string PatientsFolder;
    [SerializeField] GameObject ActionButtonPrefab;
    [SerializeField] GameObject PatientBase;
    // Start is called before the first frame update

    void Awake()
    {
        Debug.Log("Started");
        List<string> fileList = Directory.GetFiles(PatientsFolder).ToList();
        foreach (var item in Directory.GetDirectories(PatientsFolder))
        {
            fileList.Add(item);
        }

        foreach (string file in fileList)
        {
            //get filenames
            string label = CutPatientFile(file);

            if (label.EndsWith(".raw"))
            {
                Debug.Log("found patient: " + label);
                //create button for each filename
                GameObject dataButton = Instantiate(ActionButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
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
                dataButton.GetComponent<PressableButton>().OnClicked.AddListener(() => { PatientSelected(file); });
            }

        }
        //NearMenu.SetActive(false);
        //ListMenu.SetActive(true);

    }
    
    void OnEnable()
    {
        NearMenu.SetActive(false);
        ListMenu.SetActive(true);
        Debug.Log("Enabled");
    }
    
  
    

    private string CutPatientFile(string patient)
    {
        return patient.Remove(0,PatientsFolder.Length+1);
    }
    public void PatientSelected(string fileName)
    {
        Debug.Log("Loading " + fileName);
        
        DatasetIniData iniData = DatasetIniReader.ParseIniFile(fileName + ".ini");
        RawDatasetImporter importer = new RawDatasetImporter(fileName,
                                                            iniData.dimX,
                                                            iniData.dimY,
                                                            iniData.dimZ,
                                                            iniData.format,
                                                            iniData.endianness,
                                                            iniData.bytesToSkip);
        VolumeDataset dataset = importer.Import();
        Debug.Log(dataset.name);
        
        VolumeRenderedObject vro = VolumeObjectFactory.CreateObject(dataset);
        vro.AddComponent<BoxCollider>();
        vro.AddComponent<ObjectManipulator>();
        vro.AddComponent<TapToPlace>();
        vro.GetComponent<Transform>().transform.position = PatientBase.transform.position;
        
        vro.transform.SetParent(PatientBase.transform,true);
        //vro.SetParent(PatientBase.transform, false);
    }
}
