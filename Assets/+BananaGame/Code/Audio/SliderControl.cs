using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BananaSoup
{
    public class SliderControl : MonoBehaviour
    {
        public AudioMixer mixer;
        public AudioMixerGroup masterGroup;
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup sfxGroup;

        [SerializeField] private TextMeshProUGUI _masterSliderText;
        [SerializeField] private TextMeshProUGUI _musicSliderText;
        [SerializeField] private TextMeshProUGUI _sfxSliderText;
        
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;


        private void Start()
        {
            _masterSlider.onValueChanged.AddListener((v) => { _masterSliderText.text = v.ToString("0.0"); });
            _musicSlider.onValueChanged.AddListener((v) => { _musicSliderText.text = v.ToString("0.0"); });
            _sfxSlider.onValueChanged.AddListener((v) => { _sfxSliderText.text = v.ToString("0.0"); });
        }

        public void SetMasterLevel (float sliderValue)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
        
        public void SetMusicLevel (float sliderValue)
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        }
        
        public void SetSFXLevel (float sliderValue)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        }
        
        
    }
}
