using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


//This needs to be a Monobehaviour because we need access to LateUpdate()
public class HoloRecorderBehaviour : MonoBehaviour
{

    public GameObject objectToRecord;

    private bool isRecording;

    public void StartRecording()
    {
        Debug.Log("StartRecording");
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
        string pathToAnimationClip = SaveKeyframes(animationClipName);
        Debug.Log($"AnimationClip saved under {pathToAnimationClip}");
        HoloRecording newRecording = new HoloRecording(pathToAnimationClip, animationClipName);
        return newRecording;
    }


    private string SaveKeyframes(string filename)
    {
        string path = Application.persistentDataPath + $"/{filename}.txt";
        AllKeyFrames allKeyFrames = new AllKeyFrames(translateXKeys);
        string keyframesAsJson = JsonUtility.ToJson(allKeyFrames, true);
        File.WriteAllText(path, keyframesAsJson);
        return path;
    }


    private int GetRandomNumberBetween1and100000()
    {
        System.Random random = new System.Random();
        int randomInt = random.Next(1, 100001);
        return randomInt;
    }

    void LateUpdate()
    {
        if (isRecording)
        {
            CaptureKeyFrame();
        }
    }

    private float timeOfLastUpdate = 0.0f;
    private List<Keyframe> translateXKeys = new List<Keyframe>();


    private void CaptureKeyFrame()
    {
        float timeOfKeyFrame = timeOfLastUpdate + Time.deltaTime;
        Keyframe newKey = new Keyframe(timeOfKeyFrame, objectToRecord.transform.localPosition.x);
        translateXKeys.Add(newKey);
        timeOfLastUpdate += Time.deltaTime;
    }


    private void ResetRecorder()
    {
        translateXKeys = new List<Keyframe>();
        timeOfLastUpdate = 0.0f;
    }

}