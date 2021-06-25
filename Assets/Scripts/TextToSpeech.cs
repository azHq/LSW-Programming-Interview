
using System;
using System.Collections;
using System.Collections.Generic;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.TextToSpeech.V1;

using UnityEngine;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    public WatsonSettings settings;
    public enum IBM_voices {
        GB_KateV3Voice,
        US_AllisonVoice,
        US_AllisonV3Voice,
        US_EmilyV3Voice,
        US_HenryV3Voice,
        US_KevinV3Voice,
        US_LisaVoice,
        US_LisaV3Voice,
        US_MichaelVoice,
        US_MichaelV3Voice,
        US_OliviaV3Voice
    }
    [SerializeField]
    private IBM_voices voice = IBM_voices.US_MichaelV3Voice;

    private TextToSpeechService tts_service; 
    private IamAuthenticator tts_authenticator;
    public enum ProcessingStatus { Processing, Idle };
    private ProcessingStatus audioStatus;

    [SerializeField]
    private AudioSource outputAudioSource;
    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    private void Start()
    {
        audioStatus = ProcessingStatus.Idle;
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
        if (outputAudioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            outputAudioSource = gameObject.GetComponent<AudioSource>();
        }
        
    }
    public IEnumerator CreateService()
    {
        
        tts_authenticator = new IamAuthenticator(apikey: settings.tts_apikey);
      
        while (!tts_authenticator.CanAuthenticate())
            yield return null;

        tts_service = new TextToSpeechService(tts_authenticator);
        if (!string.IsNullOrEmpty(settings.tts_serviceUrl))
        {
            tts_service.SetServiceUrl(settings.tts_serviceUrl);
        }
    }

    public IEnumerator ProcessText(string text)
    {
        Debug.Log("ProcessText");

        string nextText = text;

        audioStatus = ProcessingStatus.Processing;

        if (outputAudioSource.isPlaying)
        {
            yield return null;
        }

        if (String.IsNullOrEmpty(nextText))
        {
            yield return null;
        }

        byte[] synthesizeResponse = null;
        AudioClip clip = null;
        tts_service.Synthesize(
            callback: (DetailedResponse<byte[]> response, IBMError error) =>
            {
                synthesizeResponse = response.Result;
                clip = WaveFile.ParseWAV("myClip", synthesizeResponse);

                //audioQueue.Enqueue(clip);
                PlayClip(clip);
            },
            text: nextText,
            voice: "en-" + voice,
            accept: "audio/wav"
        );

        while (synthesizeResponse == null)
            yield return null;

        audioStatus = ProcessingStatus.Idle;

    }

    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            outputAudioSource.spatialBlend = 0.0f;
            outputAudioSource.loop = false;
            outputAudioSource.clip = clip;
            outputAudioSource.Play();
        }
    }

    
    
    public ProcessingStatus Status()
    {
        return audioStatus;
    }

    public bool ServiceReady()
    {
        return tts_service != null;
    }
}
