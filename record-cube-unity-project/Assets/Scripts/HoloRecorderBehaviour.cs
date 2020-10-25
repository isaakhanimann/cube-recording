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
        string pathToAnimationClip = $"Assets/Animation/AnimationClips/{animationClipName}.anim";
        AnimationClip recordedAnimationClip = GetAnimationClipFromRecordedKeyframes();
        AssetDatabase.CreateAsset(recordedAnimationClip, pathToAnimationClip);
        AssetDatabase.SaveAssets();
        HoloRecording newRecording = new HoloRecording(recordedAnimationClip, pathToAnimationClip, animationClipName);
        return newRecording;
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

    private AnimationClip GetAnimationClipFromRecordedKeyframes()
    {
        Debug.Log($"{translateXKeys.Count} keyframes were recorded");
        AnimationCurve translateX = new AnimationCurve(translateXKeys.ToArray());
        // AnimationCurve translateY
        // AnimationCurve translateZ
        // AnimationCurve rotateX
        // AnimationCurve rotateY
        // AnimationCurve rotateZ
        AnimationClip newClip = new AnimationClip();
        newClip.SetCurve("", typeof(Transform), "localPosition.x", translateX);
        return newClip;

    }

    private void ResetRecorder()
    {
        translateXKeys = new List<Keyframe>();
        timeOfLastUpdate = 0.0f;
    }

}