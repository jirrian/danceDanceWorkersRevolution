using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterSelect : MonoBehaviour {

	public string character;
	public AudioSource select;

	private float duration;

	void OnMouseDown(){
        // load a new scene
        if(character == "dj"){
        	select.Play();
        	duration = select.clip.length;
        	StartCoroutine(WaitForSound());
        	
        }
    }

    IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(duration);
        print("FinishAudio");
        SceneManager.LoadScene("Stage_DJ");
    }
}
