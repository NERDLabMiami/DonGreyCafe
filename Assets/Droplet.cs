using UnityEngine;
using System.Collections;

public class Droplet : MonoBehaviour {
	public int dropletType;
	private float spawnTime;
	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > spawnTime + 1f) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "glass") {
			Glass glass = coll.gameObject.GetComponentInParent<Glass>();
			glass.Fill(GetComponent<SpriteRenderer>().color);
			Destroy(this.gameObject);
		}

		if (coll.gameObject.tag == "outside") {
			Destroy(this.gameObject);

		}

	}
}
