using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecorderFunctions : MonoBehaviour
{
    public GameObject recordingRepresentationPrefab;
    public GameObject holoRecorder;

    private GameObject recordingRepresentationInstance;
    private HoloRecorderBehaviour holoRecorderBehaviour;

    private void Start()
    {
        holoRecorderBehaviour = holoRecorder.GetComponent<HoloRecorderBehaviour>();
        holoRecorderBehaviour.InitializeRecorder();
    }

    public void StartRecordingAndInstantiateRepresentation()
    {
        Debug.Log("StartRecordingAndInstantiateRepresentation");
        holoRecorderBehaviour.StartRecording();
        InstantiateRecordingRepresentation();
    }

    private void InstantiateRecordingRepresentation()
    {
        Debug.Log("InstantiateRecordingRepresentation");
        Quaternion rotationToInstantiate = Quaternion.identity;
        Vector3 positionToInstantiate = Camera.main.transform.position + 0.5f * Vector3.forward;
        recordingRepresentationInstance = Instantiate(original: recordingRepresentationPrefab, position: positionToInstantiate, rotation: rotationToInstantiate);
    }

    public void StopRecordingAndPutRecordingIntoRepresentation()
    {
        Debug.Log("StopRecordingAndPutRecordingIntoRepresentation");
        HoloRecording newRecording = holoRecorderBehaviour.StopRecording();
        HoloPlayerBehaviour playerComponent = recordingRepresentationInstance.GetComponent<HoloPlayerBehaviour>();
        playerComponent.PutHoloRecordingIntoPlayer(newRecording);
    }

    public void CancelRecordingAndRemoveRepresentation()
    {
        Debug.Log("CancelRecordingAndRemoveRepresentation");
        holoRecorderBehaviour.CancelRecording();
        Destroy(recordingRepresentationInstance);

    }
}
