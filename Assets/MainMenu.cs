using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void gotoWebsite() {
		//BUY SOME TEA!
		Application.OpenURL("http://www.lemoncitytea.com");
	}

	public void playGame() {
		Application.LoadLevel(1);
	}
}
