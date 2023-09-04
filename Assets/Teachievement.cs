using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Teachievement : MonoBehaviour {
	public Color[] canisterColors;
	public string[] teaNames;
	public string[] teaDescriptions;
	public Text teaName;
	public Text description;
	public Image canister;
	public GameObject achievementPanel;
	public GameObject gameControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Unlock(int achievement) {
		PlayerPrefs.SetInt("number of teas", achievement);
		canister.color = canisterColors[achievement - 2];
		teaName.text = teaNames[achievement - 2];
		description.text = teaDescriptions[achievement - 2];
		gameControl.SetActive(false);
		achievementPanel.SetActive(true);
		Time.timeScale = 0f;

	}

	public void Resume() {
		Time.timeScale = 1f;
		gameControl.SetActive(true);
	}
}
