using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class AudioSourceTester : MonoBehaviour
{
    public Text itemName;

    private int pickedSourceNumber = 0;
    private AudioSource AudioSource;
    public List<AudioClip> audioClipsForTest = new List<AudioClip>();
    public string pickedSourceName;
    [Range(0, 1)]
    public float volume = 1;
    [Range(0, 3)]
    public float pitch = 1;
    public bool useRandomPitch;

    private void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
        pickedSourceName = audioClipsForTest[pickedSourceNumber].name;
        AudioSource.clip = audioClipsForTest[pickedSourceNumber];

        if (itemName != null) itemName.text = audioClipsForTest[pickedSourceNumber].name;

    }

    private void Update()
    {
        AudioSource.pitch = pitch;
        AudioSource.volume = volume;
    }

    public void SetNextItem()
    {
        if (pickedSourceNumber < audioClipsForTest.Count)
        {
            pickedSourceNumber++;
            pickedSourceName = audioClipsForTest[pickedSourceNumber].name;
            AudioSource.clip = audioClipsForTest[pickedSourceNumber];
            AudioSource.Play();

            if (itemName != null) itemName.text = audioClipsForTest[pickedSourceNumber].name;

        }
    }

    public void SetPreviousItem()
    {
        if (pickedSourceNumber > 0)
        {
            pickedSourceNumber--;
            pickedSourceName = audioClipsForTest[pickedSourceNumber].name;
            AudioSource.clip = audioClipsForTest[pickedSourceNumber];
            AudioSource.Play();

            if (itemName != null) itemName.text = audioClipsForTest[pickedSourceNumber].name;

        }
    }

    public void DeleteItem()
    {
        audioClipsForTest.RemoveAt(pickedSourceNumber);
        AudioSource.clip = audioClipsForTest[pickedSourceNumber];
        pickedSourceName = audioClipsForTest[pickedSourceNumber].name;
        AudioSource.Play();

        if (itemName != null) itemName.text = audioClipsForTest[pickedSourceNumber].name;

    }

    public Text textFor_pitchValue;
    public void OnValueChanged(float newValue)
    {
        if (newValue != 0)
        {
            pitch = newValue;
            textFor_pitchValue.text = newValue.ToString();
        }
    }
}
