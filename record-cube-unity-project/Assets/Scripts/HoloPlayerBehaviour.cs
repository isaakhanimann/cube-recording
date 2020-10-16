using UnityEngine;
using UnityEditor;


public class HoloPlayerBehaviour : MonoBehaviour
{
    public GameObject prefabToPlayAnimationOn;

    public void PutHoloRecordingIntoPlayer(HoloRecording holoRecording)
    {
        Debug.Log("PutHoloRecordingIntoPlayer");
        // There needs to be an AnimatorOverrideController for every animation clip to be played on the object with the Animator
        AnimatorOverrideController animatorOverrideController = CreateAndSaveAnimatorOverrideController(name: "AnimatorOverrideControllerFor" + holoRecording.animationClipName);
        animatorOverrideController["Recorded"] = holoRecording.animationClip;
        prefabToPlayAnimationOn.GetComponent<Animator>().runtimeAnimatorController = animatorOverrideController;
    }

    private AnimatorOverrideController CreateAndSaveAnimatorOverrideController(string name)
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(prefabToPlayAnimationOn.GetComponent<Animator>().runtimeAnimatorController);
        AssetDatabase.CreateAsset(animatorOverrideController, $"Assets/Animation/AnimationOverrideControllers/AnimatorOverrideController{name}.overrideController");
        AssetDatabase.SaveAssets();
        return animatorOverrideController;
    }

    public void Play()
    {
        Debug.Log("Play");
        prefabToPlayAnimationOn.GetComponent<Animator>().SetTrigger("Play");
    }

}
