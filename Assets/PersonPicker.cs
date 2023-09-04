using UnityEngine;
using System.Collections;

public class PersonPicker : MonoBehaviour {
	public GameObject[] people;
	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		int chosenPerson = (int)Random.Range(0, people.Length);
		GameObject p = (GameObject)Instantiate(people[chosenPerson], pos, transform.rotation);	
		p.transform.SetParent(gameObject.transform);
	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
