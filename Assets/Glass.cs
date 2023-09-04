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
        CustomerSpawner spawner = transform.parent.GetComponentInParent<CustomerSpawner>();

        if (cc.r == c.r && cc.g == c.g && cc.b == c.b) {
            liquid.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b);
            drops++;
            spawner.pot.checkFlair();

            if (drops == 7)
            {
                if (gameObject.GetComponentInParent<AudioSource>())
                {
                    gameObject.GetComponentInParent<Animator>().SetTrigger("smile");
                    gameObject.GetComponentInParent<AudioSource>().Play();
                    gameObject.GetComponentInParent<Person>().Drink();
                }

                gameObject.GetComponentInParent<Person>().timesUp = true;
                spawner.Served(gameObject.GetComponentInParent<Person>().column);
           }
        }
        else if (!wrongtea) {
            wrongtea = true;
            gameObject.GetComponentInParent<Person>().timesUp = true;
            if(spawner.streak)
            {
                spawner.flashWarningMessage("WRONG TEA!");
                spawner.endStreak(gameObject.GetComponentInParent<Person>().column);
                spawner.Discard(gameObject.GetComponentInParent<Person>().column);
            }
            else
            {
                spawner.pot.wrongTea();
            }

        }


		if(liquid.transform.localScale.y < 10) {
			liquid.transform.localScale += new Vector3(0, .8f, 0);
			waterLevel.offset += new Vector2(0, .2f);
		}

    }
}
