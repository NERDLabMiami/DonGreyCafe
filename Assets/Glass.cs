using UnityEngine;
using System.Collections;

public class Glass : MonoBehaviour {
	public GameObject liquid;
	public BoxCollider2D waterLevel;
	public int drops;
	private bool wrongtea;
	// Use this for initialization
	void Start () {
        wrongtea = false;	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Fill(Color c) {
		Color cc = GetComponentInParent<Person>().color;


        
        liquid.GetComponent<SpriteRenderer>().color = c;
        c.a = 1;
        if (cc.r == c.r && cc.g == c.g && cc.b == c.b) {
            Debug.Log("TEA MATCHES PERSON");
            liquid.GetComponent<SpriteRenderer>().color = c;
            drops++;
            if(CustomerSpawner.Instance.pot)
            {
                CustomerSpawner.Instance.pot.checkFlair();
            }

            if (drops == CustomerSpawner.Instance.dropsPerGlass)
            {
                if (gameObject.GetComponentInParent<AudioSource>())
                {
                    gameObject.GetComponentInParent<Animator>().SetTrigger("smile");
                    gameObject.GetComponentInParent<AudioSource>().Play();
                    gameObject.GetComponentInParent<Person>().Drink();
                }

                gameObject.GetComponentInParent<Person>().timesUp = true;
                CustomerSpawner.Instance.Served(gameObject.GetComponentInParent<Person>().column);
           }
        }
        else if (!wrongtea) {
            wrongtea = true;
            gameObject.GetComponentInParent<Person>().timesUp = true;
            if(CustomerSpawner.Instance.streak)
            {
                CustomerSpawner.Instance.FlashWarningMessage("WRONG TEA!");
                CustomerSpawner.Instance.EndStreak(gameObject.GetComponentInParent<Person>().column);
                CustomerSpawner.Instance.Discard(gameObject.GetComponentInParent<Person>().column);
            }
            else
            {
                /*
                spawner.pot.wrongTea();
            */
                }

        }


		if(liquid.transform.localScale.y < 10) {
			liquid.transform.localScale += new Vector3(0, .8f, 0);
			waterLevel.offset += new Vector2(0, .2f);
		}

    }
}
