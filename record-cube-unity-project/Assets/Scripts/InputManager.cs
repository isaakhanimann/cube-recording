using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    UIRecorderFunctions uiRecorderFunctions;

    void Start()
    {
        uiRecorderFunctions = gameObject.GetComponent<UIRecorderFunctions>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            uiRecorderFunctions.StartRecordingAndInstantiateRepresentation();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            uiRecorderFunctions.StopRecordingAndPutRecordingIntoRepresentation();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            uiRecorderFunctions.CancelRecordingAndRemoveRepresentation();
        }
    }
}
