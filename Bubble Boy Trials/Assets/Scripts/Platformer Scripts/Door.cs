using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public AudioClip audio;

    public void PlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
	
}
