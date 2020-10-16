using UnityEngine;
using UnityEditor;


public class HoloPlayerBehaviour : MonoBehaviour
{
    public GameObject prefabOfRecordedObject;

    private GameObject instanceOfRecordedObject;
    private Animator animatorOfInstance;
    private AnimatorOverrideController animatorOverrideController;

    public void PutHoloRecordingIntoPlayer(HoloRecording holoRecording)
    {
        Debug.Log("PutHoloRecordingIntoPlayer");
        InstantiateRecordedObjectAndSetInactive();
        // There needs to be an AnimatorOverrideController for every animation clip to be played on the object with the Animator
        animatorOverrideController = CreateAndSaveAnimatorOverrideController(name: "AnimatorOverrideControllerFor" + holoRecording.animationClipName);
        animatorOverrideController["Recorded"] = holoRecording.animationClip;
        animatorOfInstance.runtimeAnimatorController = animatorOverrideController;
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

    private AnimatorOverrideController CreateAndSaveAnimatorOverrideController(string name)
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animatorOfInstance.runtimeAnimatorController);
        AssetDatabase.CreateAsset(animatorOverrideController, $"Assets/Animation/AnimatorOverrideControllers/{name}.overrideController");
        AssetDatabase.SaveAssets();
        return animatorOverrideController;
    }

    public void Play()
    {
        Debug.Log("Play");
        instanceOfRecordedObject.SetActive(true);
        animatorOfInstance.SetTrigger("Play");
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

}
