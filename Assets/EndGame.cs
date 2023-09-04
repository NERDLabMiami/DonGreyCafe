using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {
	public PotControl pot;
	public GameObject holdUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restart() {
		pot.refresh();
		holdUp.SetActive(true);
	}

	public void backtoMain() {
        SceneManager.LoadScene(0);
//		Application.LoadLevel(0);
	}


}
