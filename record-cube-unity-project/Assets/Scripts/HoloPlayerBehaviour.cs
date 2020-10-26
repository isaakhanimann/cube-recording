using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        List<Keyframe> keyframes = LoadKeyframes(path);
        return GetAnimationClipFromRecordedKeyframes(keyframes);
    }

    private List<Keyframe> LoadKeyframes(string path)
    {
        string keyframesAsJson = File.ReadAllText(path);
        AllKeyFrames allKeyFrames = JsonUtility.FromJson<AllKeyFrames>(keyframesAsJson);
        return allKeyFrames.GetKeyframes();
    }

    private AnimationClip GetAnimationClipFromRecordedKeyframes(List<Keyframe> translateXKeys)
    {
        AnimationCurve translateX = new AnimationCurve(translateXKeys.ToArray());
        AnimationClip newClip = new AnimationClip();
        newClip.SetCurve("", typeof(Transform), "localPosition.x", translateX);
        return newClip;

    }

}
