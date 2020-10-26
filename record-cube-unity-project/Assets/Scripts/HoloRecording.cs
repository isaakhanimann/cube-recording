using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HoloRecording
{
    public string pathToAnimationClip;
    public string animationClipName;
    public HoloRecording(string pathToAnimationClip, string animationClipName)
    {
        this.pathToAnimationClip = pathToAnimationClip;
        this.animationClipName = animationClipName;
    }

    public override string ToString() => $"HoloRecording: animation clip is called {animationClipName}";

}

[System.Serializable]
public class AllKeyFrames
{
    public List<SerializableKeyframe> serializableKeyframes;

    public AllKeyFrames(List<Keyframe> keyframes)
    {
        serializableKeyframes = new List<SerializableKeyframe>();
        for (int index = 0; index < keyframes.Count; index++)
        {
            Keyframe key = keyframes[index];
            serializableKeyframes.Add(new SerializableKeyframe(key));
        }
    }

    public List<Keyframe> GetKeyframes()
    {
        List<Keyframe> keyframes = new List<Keyframe>();
        for (int index = 0; index < serializableKeyframes.Count; index++)
        {
            SerializableKeyframe serializableKeyframe = serializableKeyframes[index];
            keyframes.Add(new Keyframe(serializableKeyframe.time, serializableKeyframe.value));
        }

        return keyframes;
    }
}


[System.Serializable]
public struct SerializableKeyframe
{
    public SerializableKeyframe(Keyframe keyframe)
    {
        this.time = keyframe.time;
        this.value = keyframe.value;
    }

    public Keyframe GetKeyframe()
    {
        Keyframe keyframe = new Keyframe(this.time, this.value);
        return keyframe;
    }


    public float time;
    public float value;

}