using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour {
	public GameObject[] columns;
    public bool[] occupiedColumns;
	public float timeBetweenSpawns;
    public float customerWaitTime;
	public bool shouldSpawn = false;
	public bool streak;
    public PotControl pot;
    public GameObject[] teaSelections;
    public GameObject streakParticleSystem;
    public Teachievement achievements;
    private int streakLength;
	private int longestStreak;
	private float timer;
	private int numberOfTeas;

	public GameObject[] customers;
	// Use this for initialization
	void Start () {
		numberOfTeas = PlayerPrefs.GetInt("number of teas", 1);
		pot.numberOfTeas = numberOfTeas;
		for(int i = 0; i < numberOfTeas; i++) {
			teaSelections[i].SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
        if(shouldSpawn)
        {
            int peopleCounter = 0;
            for(int i = 0; i < occupiedColumns.Length; i++)
            {
                if(occupiedColumns[i])
                {
                    peopleCounter++;
                }
            }
            timer += Time.deltaTime;
            if (timeBetweenSpawns < timer || peopleCounter == 0)
            {
                AnotherCustomer();
                timer = 0f;                
            }
        }
    }

    public void Reset()
    {
        longestStreak = 0;
        streakLength = 0;
        streak = false;
        streakParticleSystem.SetActive(false);

    }
    public void flashWarningMessage(string msg) {
        pot.badPourFx();
        pot.feedbackMessage.text = msg;
        pot.feedback.SetTrigger("show");
    }

    public void customerExpire()
    {
        pot.missed();
    }

    public void Discard(int column)
    {
        occupiedColumns[column] = false;
       // AnotherCustomer();
        timer = 0f;
    }
    public void Served(int column)
    {
        float tip = addTip();
        if (tip >= 1)
        {
            streakLength++;
        }
        if (tip < 1)
        {
            endStreak(column);
        }
        if (streakLength > 3)    {
            streak = true;
            tip += 1f;
            pot.poweredUpMusicSnapshot.TransitionTo(.01f);
            pot.perfectPour(streakLength);
            pot.GetComponent<Animator>().SetTrigger("perfect");
            if (streakLength > longestStreak){
                longestStreak = streakLength;
            }

        }
        occupiedColumns[column] = false;
        pot.cupsServed++;
        pot.moneyMade += 1;
        pot.moneyMade += tip;
        if(tip >= 1)
        {
            pot.feedbackMessage.text = tip.ToString("$0.00 Tip!");
            pot.feedback.SetTrigger("show");

        }
        LevelUp();
        pot.bonus = 0;
        //        AnotherCustomer();
        if (streakParticleSystem.activeSelf != streak){
            streakParticleSystem.SetActive(streak);
        }
    }
    private float addTip()
    {
        float tip = 0;
        switch(pot.bonus)
        {
            case 1:
                tip = .25f;
                break;
            case 2:
                tip = .50f;
                break;
            case 3:
                tip = .75f;
                break;
            case 4:
                tip = 1.0f;
                break;
            case 5:
                tip = 1.5f;
                break;
            case 6:
                tip = 2.0f;
                break;
            case 7:
                tip = 3f;
                break;
            case 8:
                tip = 4f;
                break;
            case 9:
                tip = 5f;
                break;
        }
        return tip;
    }

    public void endStreak(int column) {
        occupiedColumns[column] = false;
        streak = false;
		streakLength = 0;
		pot.backgroundMusicSnapshot.TransitionTo(.01f);
		pot.GetComponent<Animator>().SetTrigger("normal");
	}

	void LevelUp() {
		if(pot.cupsServed > 4 && pot.cupsServed < 15 && numberOfTeas < 2) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 2;
        }

		else if(pot.cupsServed > 15 && pot.cupsServed < 30 && numberOfTeas < 3) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 3;
        }

        else if(pot.cupsServed > 30 && pot.cupsServed < 40 && numberOfTeas < 4) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 4;

        }

        else if(pot.cupsServed > 40 && pot.cupsServed < 50 && numberOfTeas < 5) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 5;
        }

        else if(pot.cupsServed > 50 && pot.cupsServed < 60 && numberOfTeas < 6) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 6;
        }

        else if(pot.cupsServed > 60 && pot.cupsServed < 70 && numberOfTeas < 7) {
			//UNLOCK FIRST NEW TEA
			numberOfTeas = 7;
        }

        if (pot.cupsServed >= 3 && pot.cupsServed < 10) {
            customerWaitTime = 19;
            timeBetweenSpawns = 15;
        }
        else if(pot.cupsServed >= 10 && pot.cupsServed < 15) {
            customerWaitTime = 19;
            timeBetweenSpawns = 14;
        }
        else if(pot.cupsServed >= 15 && pot.cupsServed < 25) {
            customerWaitTime = 18;
            timeBetweenSpawns = 13;
        }
        else if(pot.cupsServed >= 25 && pot.cupsServed < 35) {
            customerWaitTime = 17;
            timeBetweenSpawns = 12;
        }
        else if(pot.cupsServed >= 35 && pot.cupsServed < 45) {
            customerWaitTime = 15;
            timeBetweenSpawns = 11;
        }
        else if(pot.cupsServed >= 45 && pot.cupsServed < 55) {
            customerWaitTime = 14;
            timeBetweenSpawns = 10;
        }
        else if(pot.cupsServed >= 55 && pot.cupsServed < 65) {
            customerWaitTime = 13;
            timeBetweenSpawns = 9;
        }
        else if(pot.cupsServed >= 65 && pot.cupsServed < 75) {
            customerWaitTime = 12;
            timeBetweenSpawns = 8;
        }
        else if(pot.cupsServed >= 75 && pot.cupsServed < 85) {
            customerWaitTime = 11;
            timeBetweenSpawns = 7;
        }
        else if(pot.cupsServed >= 85 && pot.cupsServed < 95) {
            customerWaitTime = 10;
            timeBetweenSpawns = 6;
        }
        else if(pot.cupsServed >= 95 && pot.cupsServed < 105) {
            customerWaitTime = 9.5f;
            timeBetweenSpawns = 6.5f;
        }
        else if(pot.cupsServed >= 105 && pot.cupsServed < 115) {
            customerWaitTime = 9.2f;
            timeBetweenSpawns = 6.2f;
        }
        else if(pot.cupsServed >= 115 && pot.cupsServed < 125) {
            customerWaitTime = 9f;
            timeBetweenSpawns = 6f;
        }
        else if(pot.cupsServed >= 125 && pot.cupsServed < 135) {
            customerWaitTime = 8.7f;
            timeBetweenSpawns = 5.7f;
        }
        else if(pot.cupsServed >= 135 && pot.cupsServed < 145) {
            customerWaitTime = 8.5f;
            timeBetweenSpawns = 5.5f;
        }
        else if(pot.cupsServed >= 145 && pot.cupsServed < 155) {
            customerWaitTime = 8.2f;
            timeBetweenSpawns = 5.2f;
        }

        else if(pot.cupsServed >= 155 && pot.cupsServed < 165) {
            customerWaitTime = 8f;
            timeBetweenSpawns = 5f;
        }
        else if(pot.cupsServed >= 165 && pot.cupsServed < 175) {
            timeBetweenSpawns = 4.7f;
        }
        else if(pot.cupsServed >= 175 && pot.cupsServed < 185) {
            timeBetweenSpawns = 4.6f;
		}

		else if(pot.cupsServed >= 185 && pot.cupsServed < 195) {
			timeBetweenSpawns = 4.5f;
		}

		else if(pot.cupsServed >= 195 && pot.cupsServed < 205) {
			timeBetweenSpawns = 4.4f;
		}

		else if(pot.cupsServed >= 205 && pot.cupsServed < 215) {
			timeBetweenSpawns = 4.3f;
		}

		else if(pot.cupsServed >= 215 && pot.cupsServed < 225) {
			timeBetweenSpawns = 4.2f;
		}
		else if(pot.cupsServed >= 225 && pot.cupsServed < 235) {
			timeBetweenSpawns = 4.1f;
		}
		else if(pot.cupsServed >= 235 && pot.cupsServed < 245) {
			timeBetweenSpawns = 4f;
		}
		else if(pot.cupsServed >= 245 && pot.cupsServed < 255) {
            customerWaitTime = 7.7f;
        }

		else if(pot.cupsServed >= 255 && pot.cupsServed < 265) {
			timeBetweenSpawns = 3.9f;
		}

		if(pot.numberOfTeas != numberOfTeas) {
			//leveled up!
			pot.numberOfTeas = numberOfTeas;
			for(int i = 0; i < numberOfTeas; i++) {
				teaSelections[i].SetActive(true);
			}
			achievements.Unlock(numberOfTeas);
		}
	}

	public void Resume() {
//		Time.timeScale = 1f;
	}
	public void ClearCustomers() {
		for(int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
			for(int j = 0; j < people.Length; j++) {
				Destroy(people[j].gameObject);
			}
		}

	}

    void AnotherCustomer()
    {
        timer = 0f;
        int randomCustomerIndex = Random.Range(0, occupiedColumns.Length);
        if (!occupiedColumns[randomCustomerIndex]) {
            occupiedColumns[randomCustomerIndex] = true;
         //SPAWN CUSTOMER
            spawnCustomer(customers[Random.Range(0, numberOfTeas)], columns[randomCustomerIndex].transform.position, columns[randomCustomerIndex], randomCustomerIndex);
        }
        else
        {
            Debug.Log("Tried occupied column");
            AnotherCustomer();
        }
    }

    void GenerateRow() {
        //RESETS ENTIRE ROW, SHOULD ONLY BE USED ON FIRST GO
        Debug.Log("TIMER: " + timer);
		for(int i = 0; i < columns.Length; i++) {
			Person[] people = columns[i].GetComponentsInChildren<Person>();
            occupiedColumns[i] = false;
            for (int j = 0; j < people.Length; j++) {
				people[j].timesUp = true;
            }
		}
		timer = 0f;
//		served = 0;
//		spawned = 0;
//		bugging = false;
        int selectedCol = Random.Range(0, columns.Length);
        spawnCustomer(customers[Random.Range(0, numberOfTeas)], columns[selectedCol].transform.position, columns[selectedCol], selectedCol);
//        spawned++;

	}

	public void StartSpawning() {
		GenerateRow();
		shouldSpawn = true;
	}
	private void spawnCustomer(GameObject go, Vector3 place, GameObject parent, int colNum) {
        occupiedColumns[colNum] = true;
        if (!parent.GetComponentInChildren<Person>())
        {
            place.y -= (Camera.main.orthographicSize * 2f);
            GameObject o = (GameObject)Instantiate(go, place, transform.rotation);
            o.transform.parent = parent.transform;
            o.GetComponent<Person>().column = colNum;
            o.GetComponent<Person>().StartMoving(customerWaitTime);
        }
	}

}
