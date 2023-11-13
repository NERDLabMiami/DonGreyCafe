using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {
	public SpriteRenderer shirt;
	public Color color;
	public bool spawning = false;
	public bool timesUp = false;
	private bool buggedOut = false;
	public float pauseBeforeSpawning;
    private float timer;
    private float timeLeft;
    public float patience;
    private Vector3 origin;
    public int column;

	// Use this for initialization
	void Start () {
		color.a = 255;
		shirt.color = color;
		origin = transform.position;
        timer = 9999f;
	}

    public void StartMoving(float timeRemaining) {
        timeLeft = timeRemaining;
        pauseBeforeSpawning += Time.time;
		spawning = true;
	}

	public void Bugout() {

        if (!buggedOut) {
			Glass glass = GetComponentInChildren<Glass>();
			//TODO: Variable number of drops / game setting
			if(glass.drops < transform.parent.GetComponentInParent<CustomerSpawner>().dropsPerGlass) {
				gameObject.GetComponent<Animator>().SetTrigger("bugging");
			}
			buggedOut = true;
			if (GetComponentInParent<CustomerSpawner>().pot)
            {
				GetComponentInParent<CustomerSpawner>().pot.alarm();
            }
			else
            {
				Debug.Log("NO POT SET.");
            }
        }
	}

	public void Drink() {
		gameObject.GetComponent<Animator>().SetTrigger("perfect");
		gameObject.GetComponentInChildren<ParticleSystem>().Play();
	}

	public void Clear() {
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if ((timeLeft - 3) < timer)
        {
                Bugout();
        }
        if(timeLeft < timer && !timesUp)
        {
//            timesUp = true;
			//TODO: STREAKS VS NO DROPS ALLOWED
			/*
            if(GetComponentInParent<CustomerSpawner>().streak)
            {
                GetComponentInParent<CustomerSpawner>().flashWarningMessage("POUR FASTER!");
                GetComponentInParent<CustomerSpawner>().endStreak(column);
            }
            else
            {
                GetComponentInParent<CustomerSpawner>().customerExpire();
            }
			*/
        }

        if (spawning) {
			if(pauseBeforeSpawning < Time.time) {
				
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),transform.parent.transform.position, 20 * Time.deltaTime);
				/*
				GameObject glass = GetComponentInChildren<Glass>().gameObject;

				if(glass.transform.localScale.x >= 2.5f) {
					glass.transform.localScale *= .99f;
				}
				*/
				if(transform.position.y >= 2) {
					spawning = false;
				}
			}
		}

		if(timesUp) {
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), origin, 10 * Time.deltaTime);
			if(transform.position.y <= -5) {
				GetComponentInParent<CustomerSpawner>().chairs[column].occupied = false;
                Destroy(gameObject);
			}

		}
	}
		
}
