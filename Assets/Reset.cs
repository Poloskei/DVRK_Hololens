using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityVolumeRendering;

public class ResetButton : MonoBehaviour
{
    [SerializeField] GameObject PatientBase;
    public void Reset()
    {
        PatientBase.GetComponentInChildren<VolumeRenderedObject>().transform.position = PatientBase.transform.position;
        Debug.Log("Resetting object " + PatientBase.GetComponentInChildren<VolumeRenderedObject>().name);
    }
}
