using UnityEngine;

public struct HoloRecording
{
    public AnimationClip animationClip;
    public string pathToAnimationClip;
    public string animationClipName;
    public HoloRecording(AnimationClip animationClip, string pathToAnimationClip, string animationClipName)
    {
        this.animationClip = animationClip;
        this.pathToAnimationClip = pathToAnimationClip;
        this.animationClipName = animationClipName;
    }

    public override string ToString() => $"HoloRecording: animation clip is called {animationClipName}";

}
