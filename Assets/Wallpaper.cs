using UnityEngine;
using System.Collections;

public class Wallpaper : MonoBehaviour {
	public float speed = .0001f;
	public Material night;
	public Material day;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,speed,0);
	}

	public void setNight() {
//		GetComponent<MeshRenderer>().renderer.material = m_aMaterials[nIdx];
		GetComponent<MeshRenderer>().material = night;
		Camera.main.backgroundColor = Color.black;
	}

	public void setDay() {
		Camera.main.backgroundColor = Color.white;

		GetComponent<MeshRenderer>().material = day;

	}
}
