using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

//This needs to be a Monobehaviour because we need access to LateUpdate()
public class HoloRecorderBehaviour : MonoBehaviour
{

    public GameObject objectToRecord;

    private GameObjectRecorder gameObjectRecorder;
    private bool isRecording;
    private AnimationClip currentlyRecordedAnimationClip;

    private void Start()
    {
        InitializeRecorder();
    }

    public void InitializeRecorder()
    {
        Debug.Log("InitializeRecorder");
        gameObjectRecorder = new GameObjectRecorder(objectToRecord);
        gameObjectRecorder.BindComponentsOfType<Transform>(target: objectToRecord, recursive: true);
    }


    public void StartRecording()
    {
        Debug.Log("StartRecording");
        currentlyRecordedAnimationClip = new AnimationClip();
        isRecording = true;
    }

    public void CancelRecording()
    {
        Debug.Log("CancelRecording");
        isRecording = false;
        ResetRecorder();
    }

    public HoloRecording StopRecording()
    {
        Debug.Log("StopRecording");
        isRecording = false;
        HoloRecording newRecording = SaveRecording();
        ResetRecorder();
        return newRecording;
    }

    private HoloRecording SaveRecording()
    {
        string animationClipName = "AnimationClip" + GetRandomNumberBetween1and100000();
        string pathToAnimationClip = $"Assets/Animation/AnimationClips/{animationClipName}.anim";
        AssetDatabase.CreateAsset(currentlyRecordedAnimationClip, pathToAnimationClip);
        AssetDatabase.SaveAssets();
        gameObjectRecorder.SaveToClip(currentlyRecordedAnimationClip);
        HoloRecording newRecording = new HoloRecording(currentlyRecordedAnimationClip, pathToAnimationClip, animationClipName);
        return newRecording;
    }

    private int GetRandomNumberBetween1and100000()
    {
        System.Random random = new System.Random();
        int randomInt = random.Next(1, 100001);
        return randomInt;
    }

    private void ResetRecorder()
    {
        gameObjectRecorder.ResetRecording();
        gameObjectRecorder.BindComponentsOfType<Transform>(objectToRecord, true);
    }

    void LateUpdate()
    {
        if (isRecording)
        {
            gameObjectRecorder.TakeSnapshot(Time.deltaTime);
        }
    }

}