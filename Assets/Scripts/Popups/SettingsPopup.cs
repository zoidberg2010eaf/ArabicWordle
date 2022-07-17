using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    public Sprite activeSoundSprite;
    public Sprite inactiveSoundSprite;
    public Button soundButton;
    
    public void OnSoundButtonClick()
    {
        SoundManager.Instance.ToggleMute();
        soundButton.image.sprite = (SoundManager.Instance.audioSource.mute) ? inactiveSoundSprite : activeSoundSprite;

    }
}
