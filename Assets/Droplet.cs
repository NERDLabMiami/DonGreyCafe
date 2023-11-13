using UnityEngine;
using System.Collections;

public class Droplet : MonoBehaviour {
	public int dropletType;
	private float spawnTime;
	public int maxAmount = 10;
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
			Debug.Log("FILLING GLASS " + glass.name + " with " + GetComponent<SpriteRenderer>().color);
			if(GetComponent<SpriteRenderer>().color != null)
            {
				glass.Fill(GetComponent<SpriteRenderer>().color);

			}
			Destroy(this.gameObject);
		}

		if (coll.gameObject.tag == "outside") {
			Destroy(this.gameObject);

		}

		if(coll.gameObject.tag == "kettle")
        {
            Debug.Log("found a kettle!");
			

            if (coll.gameObject.GetComponent<Kettle>())
            {

				if (coll.gameObject.GetComponent<Kettle>().HasRoom())
				{
					Debug.Log("Has room for tea");
					coll.gameObject.GetComponent<Kettle>().AddTea();
				}
				else
                {
					Debug.Log("No room for tea!");
                }
			}
			Destroy(this.gameObject);
        }

	}
}
