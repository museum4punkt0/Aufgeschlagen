#if GUIDEPILOT_CORE_VIEWER3D

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ViewerAudioController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Ease fadeEase;

    private AudioSource source;

    private void Awake()
    {
    }

    public void PlayAudio(AudioClip audio, float fadeTime = 0.0f)
    {
        if (source != null)
        {
            source.clip = audio;
            source.Play();

            if (fadeTime > 0.0f)
            {
                if (DOTween.IsTweening(source))
                    source.DOKill();

                source.DOFade(1.0f, fadeTime).From(0.0f).SetEase(fadeEase);
            }
        }
    }

    public void PauseAudio(float fadeTime = 0.0f)
    {
        if (source != null)
        {
            if (source.clip == null) return;

            source.Pause();

            if (fadeTime > 0.0f)
            {
                if (DOTween.IsTweening(source))
                    source.DOKill();

                source.DOFade(0.0f, fadeTime).From(source.volume).SetEase(fadeEase);
            }
        }
    }

    public void StopAudio(float fadeTime = 0.0f)
    {
        if (source != null)
        {
            if (source.clip == null) return;

            source.Stop();

            if (fadeTime > 0.0f)
            {
                if (DOTween.IsTweening(source))
                    source.DOKill();

                source.DOFade(0.0f, fadeTime).From(source.volume).SetEase(fadeEase);
            }
        }
    }

    public void SetAudioSource(AudioSource source)
    {
        this.source = source;
    }
}

#endif
