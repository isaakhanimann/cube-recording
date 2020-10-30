﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class HoloPlayerBehaviour : MonoBehaviour
{
    public GameObject prefabOfRecordedObject;

    private GameObject instanceOfRecordedObject;
    private Animator animatorOfInstance;
    private float lengthOfAnimationInSeconds;

    public void PutHoloRecordingIntoPlayer(HoloRecording holoRecording)
    {
        Debug.Log("PutHoloRecordingIntoPlayer");
        InstantiateRecordedObjectAndSetInactive();
        // There needs to be an AnimatorOverrideController for every animation clip to be played on the object with the Animator
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animatorOfInstance.runtimeAnimatorController);
        AnimationClip animationClip = GetAnimationClipFromPath(holoRecording.pathToAnimationClip);
        animatorOverrideController["Recorded"] = animationClip;
        animatorOfInstance.runtimeAnimatorController = animatorOverrideController;
        lengthOfAnimationInSeconds = animationClip.length;
    }
    private void InstantiateRecordedObjectAndSetInactive()
    {
        Debug.Log("InstantiateRecordedObjectAndSetInactive");
        Quaternion rotationToInstantiate = Quaternion.identity;
        Vector3 positionToInstantiate = Camera.main.transform.position + 0.3f * Vector3.forward;
        instanceOfRecordedObject = Instantiate(original: prefabOfRecordedObject, position: positionToInstantiate, rotation: rotationToInstantiate);
        animatorOfInstance = instanceOfRecordedObject.GetComponent<Animator>();
        instanceOfRecordedObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }

    public void Play()
    {
        Debug.Log("Play");
        instanceOfRecordedObject.SetActive(true);
        animatorOfInstance.SetTrigger("Play");
        StartCoroutine(SetInstanceInactive());
    }

    IEnumerator SetInstanceInactive()
    {
        yield return new WaitForSeconds(lengthOfAnimationInSeconds);
        instanceOfRecordedObject.SetActive(false);

    }


    public void Pause()
    {
        Debug.Log("Pause is not implemented yet");
    }

    public void Stop()
    {
        Debug.Log("Stop is not implemented yet");
        instanceOfRecordedObject.SetActive(false);
    }

    private AnimationClip GetAnimationClipFromPath(string path)
    {
        List<Keyframe> keyframes_root = LoadKeyframes("1" + path);
        List<Keyframe> keyframes_child = LoadKeyframes("2" + path);

        return GetAnimationClipFromRecordedKeyframes(keyframes_root, keyframes_child);
    }

    private List<Keyframe> LoadKeyframes(string path)
    {
        string keyframesAsJson = File.ReadAllText(path);
        AllKeyFrames allKeyFrames = JsonUtility.FromJson<AllKeyFrames>(keyframesAsJson);
        return allKeyFrames.GetKeyframes();
    }

    private AnimationClip GetAnimationClipFromRecordedKeyframes(List<Keyframe> translateXKeys, List<Keyframe> translateXKeysChild)
    {
        AnimationClip newClip = new AnimationClip();
           
        AnimationCurve translateX = new AnimationCurve(translateXKeys.ToArray());
        newClip.SetCurve("HandRig_L", typeof(Transform), "localPosition.x", translateX);
    
        AnimationCurve translateXChild = new AnimationCurve(translateXKeysChild.ToArray());
        newClip.SetCurve("HandRig_L/MainL_JNT/WristL_JNT", typeof(Transform), "localPosition.x", translateXChild);

        return newClip;
    }
}
