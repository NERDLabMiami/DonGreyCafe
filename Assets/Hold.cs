using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hold : MonoBehaviour {
	public AudioSource[] loops;
	public PotControl pot;
	public Text countdown;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void updateCountdown(string time) {
		countdown.text = time;
	}

	void begin() {
		for(int i = 0; i < loops.Length; i++) {
			loops[i].Play();
		}
		pot.Begin();
		pot.backgroundMusicSnapshot.TransitionTo(.01f);
		pot.spawner.StartSpawning();
		gameObject.SetActive(false);

	}
}
