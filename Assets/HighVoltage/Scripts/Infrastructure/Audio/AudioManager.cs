using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HighVoltage
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        private bool _isMuted;
        private float _volume;

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                source.volume = value ? 0 : _volume;
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                PlayerPrefs.SetFloat(nameof(Volume), _volume = value);
                PlayerPrefs.Save();
                if (!IsMuted)
                {
                    source.volume = value;
                }
            }
        }

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(nameof(IsMuted)))
            {
                PlayerPrefs.SetInt(nameof(IsMuted), 0);
                PlayerPrefs.Save();
            }
            else
            {
                IsMuted = PlayerPrefs.GetInt(nameof(IsMuted)) != 0;
            }
            if (!PlayerPrefs.HasKey(nameof(Volume)))
            {
                PlayerPrefs.SetFloat(nameof(Volume), Volume = 0.5f);
                PlayerPrefs.Save();
            }
            else
            {
                Volume = PlayerPrefs.GetFloat(nameof(Volume));
            }
        }

        public void Mute() => IsMuted = true;

        public void Unmute() => IsMuted = false;

        public void ToggleMute() => IsMuted = !IsMuted;
    }
}
