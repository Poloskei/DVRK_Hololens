using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityVolumeRendering;

public class DeleteButton : MonoBehaviour
{
    [SerializeField] GameObject PatientBase;
    public void Delete()
    {
        Debug.Log("Removing " + PatientBase.GetComponentInChildren<VolumeRenderedObject>().name);
        Destroy(PatientBase.GetComponentInChildren<VolumeRenderedObject>().gameObject);
    }
}
