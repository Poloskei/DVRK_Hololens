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
using System.Threading.Tasks;



public class DataSelector : MonoBehaviour
{
    //[SerializeField] GameObject ListMenu;
    [SerializeField] string PatientsFolder;
    [SerializeField] GameObject ActionButtonPrefab;
    [SerializeField] GameObject PatientBase;
    [SerializeField] GameObject PSM1Plane;
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("Started");
        List<string> fileList = Directory.GetFiles(PatientsFolder).ToList();
        List<string> folderList = Directory.GetDirectories(PatientsFolder).ToList();
        
        foreach (string file in fileList)
        {
            //get filenames
            string label = CutPatientFile(file);

            if (label.EndsWith(".raw"))
            {
                Debug.Log("found patient: " + label);
                //create button for each filename
                GameObject dataButton = CreateButton(label);

                dataButton.GetComponent<PressableButton>().OnClicked.AddListener(async() => { await RawLoader(file); });



            }
            else if (label.EndsWith(".nrrd"))
            {
                //imagefile not implemented
                //GameObject dataButton = CreateButton(label);
            }
        }
        Debug.Log(folderList.Count + " folders");
        foreach (string folder in folderList) 
        {
            //string label = CutPatientFile(file);
            List<string> filesInFolder = Directory.GetFiles(folder).ToList();
            Debug.Log(filesInFolder.Count + " files in folder");
            if (filesInFolder[0].EndsWith(".dcm"))
            {
                Debug.Log("Found DICOM folder");
                string label = CutPatientFile(folder);
                GameObject dataButton = CreateButton(label);
                dataButton.GetComponent<PressableButton>().OnClicked.AddListener(async() => { await DICOMLoader(folder); });
            }
        }
    }
    
    
  
    
    private GameObject CreateButton(string label)
    {
        GameObject dataButton = Instantiate(ActionButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        dataButton.transform.SetParent(this.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
        //set the buttons label to the name of the file
        foreach (var child in dataButton.GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (child.gameObject.name == "Label")
            {
                child.gameObject.SetActive(true);
                child.text = label;

            }
        }

        
        return dataButton;
    }
    private string CutPatientFile(string patient)
    {
        return patient.Remove(0,PatientsFolder.Length+1);
    }
    public async Task RawLoader(string fileName)
    {
        Debug.Log("Loading " + fileName);
        
        DatasetIniData iniData = DatasetIniReader.ParseIniFile(fileName + ".ini");
        RawDatasetImporter importer = new(fileName,
                                          iniData.dimX,
                                          iniData.dimY,
                                          iniData.dimZ,
                                          iniData.format,
                                          iniData.endianness,
                                          iniData.bytesToSkip);
        VolumeDataset dataset = await importer.ImportAsync();
        
        Debug.Log(dataset.name);
        
        VolumeRenderedObject vro = VolumeObjectFactory.CreateObject(dataset);
        AddComponents(vro);
    }

    public async Task DICOMLoader(string fileName)
    {
        Debug.Log("Loading " + fileName);
        List<string> filePaths = Directory.GetFiles(fileName).ToList();

        IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
        IEnumerable<IImageSequenceSeries> seriesList = await importer.LoadSeriesAsync(filePaths);

        foreach (IImageSequenceSeries series in seriesList)
        {
            VolumeDataset dataset = await importer.ImportSeriesAsync(series);
            
            AddComponents(VolumeObjectFactory.CreateObject(dataset));
        }
    }

    void AddComponents(VolumeRenderedObject vro)
    {
        vro.AddComponent<BoxCollider>();
        vro.AddComponent<ObjectManipulator>();
        vro.AddComponent<TapToPlace>();
        vro.AddComponent<CrossSectionManager>();
        vro.GetComponent<Transform>().transform.position = PatientBase.transform.position;

        vro.transform.SetParent(PatientBase.transform, true);

        PSM1Plane.GetComponent<CrossSectionPlane>().SetTargetObject(vro);
    }
}
