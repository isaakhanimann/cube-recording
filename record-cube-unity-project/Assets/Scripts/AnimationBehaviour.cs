using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animation))]
public class AnimationBehaviour : MonoBehaviour
{
    public Animation anim;
    AnimationClip animationClip;

    void Start()
    {
        anim = GetComponent<Animation>();
        // define animation curve
        AnimationCurve translateX = AnimationCurve.Linear(0.0f, 0.0f, 2.0f, 2.0f);
        AnimationCurve scaleX = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 3.0f);
        AnimationCurve scaleY = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 3.0f);
        AnimationCurve scaleZ = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 3.0f);
        animationClip = new AnimationClip();
        // set animation clip to be legacy
        animationClip.legacy = true;
        animationClip.SetCurve("", typeof(Transform), "localPosition.x", translateX);
        animationClip.SetCurve("", typeof(Transform), "localScale.x", scaleX);
        animationClip.SetCurve("", typeof(Transform), "localScale.y", scaleY);
        animationClip.SetCurve("", typeof(Transform), "localScale.z", scaleZ);
        anim.AddClip(animationClip, "test");
        anim.Play("test");
    }
}