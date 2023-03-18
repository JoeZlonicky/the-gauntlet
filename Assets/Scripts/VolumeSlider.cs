using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixer;

    private const float _minSliderValue = 0.0001f;  // Log10(0.0001) = -4, Log10(1.0) = 0
    private const float _sliderMulitplier = 20.0f;  // So we get between -80 and 0
    public void SetLevel(float sliderValue)
    {
        float logarithmicValue = sliderValue > _minSliderValue ? Mathf.Log10(sliderValue) : Mathf.Log10(_minSliderValue);
        mixer.SetFloat("volume", logarithmicValue * _sliderMulitplier);
    }
}
