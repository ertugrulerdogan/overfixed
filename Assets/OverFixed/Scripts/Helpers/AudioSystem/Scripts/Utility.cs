using System;
using System.Reflection;
using OverFixed.Scripts.Helpers.CoroutineSystem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace OverFixed.Scripts.Helpers.AudioSystem.Scripts
{
    public static class IntExtensionMethods
    {
        public static bool ToBool(this int source)
        {
            return (source == 1);
        }
    }

    public static class BoolExtensionMethods
    {
        public static int ToInt(this bool source)
        {
            return source ? 1 : 0;
        }
    }

    public static class AudioHelperExtensionMethods
    {
        private static readonly MethodInfo ToggleSetMethod;
        private static readonly MethodInfo SliderSetMethod;
        private static readonly MethodInfo ScrollbarSetMethod;
     
        private static readonly FieldInfo DropdownValueField;
        private static readonly MethodInfo DropdownRefreshMethod; 
 
        static AudioHelperExtensionMethods()
        {
            ToggleSetMethod = FindSetMethod(typeof (Toggle));
            SliderSetMethod = FindSetMethod(typeof (Slider));     
            ScrollbarSetMethod = FindSetMethod(typeof (Scrollbar));     
            DropdownValueField = (typeof (Dropdown)).GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance);
            DropdownRefreshMethod = (typeof (Dropdown)).GetMethod("Refresh", BindingFlags.NonPublic | BindingFlags.Instance);
        }
 
        public static void Set(this Toggle instance, bool value, bool sendCallback = false)
        {
            ToggleSetMethod.Invoke(instance, new object[] {value, sendCallback});
        }
     
        public static void Set(this Slider instance, float value, bool sendCallback = false)
        {
            SliderSetMethod.Invoke(instance, new object[] {value, sendCallback});
        }
     
        public static void Set(this Scrollbar instance, float value, bool sendCallback = false)
        {
            ScrollbarSetMethod.Invoke(instance, new object[] {value, sendCallback});
        }
     
        public static void Set(this Dropdown instance, int value)
        {
            DropdownValueField.SetValue(instance, value);
            DropdownRefreshMethod.Invoke(instance, new object[] {});     
        }
     
        private static MethodInfo FindSetMethod(Type objectType)
        {
            var methods = objectType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            for (var i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
                {
                    return methods[i];
                }
            }
     
            return null;
        }
    }

    public static class AudioSourceExtensionMethods
    {
        public static AudioSource DevePlay(this AudioSource audioSource)
        {
            audioSource.Play();
            return audioSource;
        }

        public static AudioSource SetClip(this AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            return audioSource;
        }

        public static AudioSource SetPlaybackTime(this AudioSource audioSource, float timeInSeconds)
        {
            audioSource.time = timeInSeconds;
            return audioSource;
        }
        
        public static AudioSource SetLoop(this AudioSource audioSource, bool loop)
        {
            audioSource.loop = loop;
            return audioSource;
        }
        
        public static AudioSource SetVolume(this AudioSource audioSource, float volume)
        {
            audioSource.volume = volume;
            return audioSource;
        }

        public static AudioSource SetMute(this AudioSource audioSource, bool mute)
        {
            audioSource.mute = mute;
            return audioSource;
        }
        
        public static AudioSource SetPitch(this AudioSource audioSource, float pitch)
        {
            audioSource.pitch = pitch;
            return audioSource;
        }

        public static AudioSource SetSpatialBlend(this AudioSource audioSource, float spatialBlend)
        {
            audioSource.spatialBlend = spatialBlend;
            return audioSource;
        }
        
        public static AudioSource Set3DSettings(this AudioSource audioSource, AudioRolloffMode rolloffMode, float maxDistance)
        {
            audioSource.rolloffMode = rolloffMode;
            audioSource.maxDistance = maxDistance;
            return audioSource;
        }
        
        public static AudioSource SetMixer(this AudioSource audioSource, AudioMixerGroup mixerGroup)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
            return audioSource;
        }
        
        public static AudioSource SetOnComplete(this AudioSource audioSource, Action completeCallback)
        {
            CoroutineManager.DoAfterGivenTime(audioSource.clip.length, completeCallback.Invoke);
            return audioSource;
        }

        public static AudioSource SetBeforeComplete(this AudioSource audioSource, float time, Action completeCallback)
        {
            Assert.IsTrue(audioSource.clip.length - time > 0f);
            
            CoroutineManager.DoAfterGivenTime(audioSource.clip.length - time, completeCallback.Invoke);
            return audioSource;            
        }   
    }
}
