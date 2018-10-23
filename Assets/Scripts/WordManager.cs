using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
	adapted from Brackeys tutorial: https://www.youtube.com/watch?v=HvMrOoUeqO0
*/
public class WordManager : MonoBehaviour {

	public List<Word> words;

	private bool hasActiveWord;
	private Word activeWord;

	public GameObject wordPrefab;
	public Transform canvas;

	public float wordDelay = 10f;
	public float nextWordTime = 5f;

	public GameObject avatar;
	Animator avatarAnimator;
	public AudioSource finishWord;
	public AudioSource wrong;

	public AudioSource music;

	public Text gameOver;
	public AudioSource gameOverAudio;
	private float duration;

	// phrases
	private string[] phrases = new string[]{"hourly no benefits",
		 "im so tired", "gig economy", "lean in", "fake it until you make it", "exposure", "everything it takes",
		 "passion", "go getter", "freelance", "contract work", "pay your dues", "cultural fit", "work harder",
		 "have what it takes", "exploitation", "deadline", "follow up on invoice", "this is your dream", "economic stability",
		 "avocado toast", "burnout", "exhaustion", "sacrifice"};

	void Start(){
		avatarAnimator = avatar.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= nextWordTime){
			AddWord();
			nextWordTime = Time.time + wordDelay;
			wordDelay *= 0.95f;
		}

		if(avatarAnimator.GetFloat("Energy") <= 0.01f){
			Debug.Log("game over");
			gameOver.enabled = true;

			if(!gameOverAudio.isPlaying){
				music.Stop();
				gameOverAudio.Play();
				duration = gameOverAudio.clip.length;
				StartCoroutine(WaitForSound());
			}
        	
		}
	}

	private void AddWord(){
		int i = Random.Range(0, phrases.Length);
		words.Add(new Word(phrases[i], SpawnWord()));
	}

	public void TypeLetter(char letter){
		if(hasActiveWord){
			if(activeWord.GetNextLetter() == letter){
				activeWord.TypeLetter();
			}
			else{
					float e = avatarAnimator.GetFloat("Energy") * 0.9f;
					avatarAnimator.SetFloat("Energy", e);
					wrong.Play();
			}
		}
		else{
			foreach(Word word in words){
				if(word.GetNextLetter() == letter){
					activeWord = word;
					hasActiveWord = true;
					word.TypeLetter();

					float e = avatarAnimator.GetFloat("Energy") * 1.1f;
					avatarAnimator.SetFloat("Energy", e);
					break;
				}
				else{
					float e = avatarAnimator.GetFloat("Energy") * 0.9f;
					avatarAnimator.SetFloat("Energy", e);
					wrong.Play();
				}
			}
		}

		if(hasActiveWord && activeWord.WordTyped()){
			hasActiveWord = false;
			words.Remove(activeWord);
			finishWord.Play();
		}
	}


	void reshuffle(string[] texts){
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++ )
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
       	}
	}

	// spawn a word prefab at a random location
	public WordDisplay SpawnWord(){
		Vector3 randomPos = new Vector3(Random.Range(0f, 300f),1000f, 0f);

		GameObject wordObj = Instantiate(wordPrefab, canvas);
		wordObj.transform.InverseTransformPoint(transform.position);
		wordObj.transform.position = randomPos;
		WordDisplay wordDisplay = wordObj.GetComponent<WordDisplay>();

		return wordDisplay;
	}

	IEnumerator WaitForSound()
    {
    	
        yield return new WaitForSeconds(duration);
        print("FinishAudio");
        SceneManager.LoadScene("CharacterSelect");
    }
}