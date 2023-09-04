using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PotControl : MonoBehaviour {
	public GameObject[] droplet;
    public GameObject[] canisters;
	public int selectedDroplet = 0;
	public int numberOfTeas;
	public GameObject spigot;
	public Text served;
	public Text endServed;
	public Text failureMessage;
	public GameObject completePanel;
	public GameObject gameControl;
	public CustomerSpawner spawner;
	public AudioClip perfectPourFx;
    public AudioClip alarmFx;
	public AudioClip wrongTeaFx;
	public AudioClip missedFx;

	public Animator feedback;
	public Animator hud;
	public Text feedbackMessage;
	public int streak = 0;
	public int startingPourAngle = 255;
	public int endingPourAngle = 325;
	public int cupsServed = 0;
    public float moneyMade;
    public int strikes = 0;
	public int dropsLeft = 100;
	public float tiltSpeed = 1.5f;
	public bool gravity = false;
    public bool playing = false;
    private bool gameOver = false;
    private bool pouring = false;
    private float debounce = 0.0f;
    private float previousAccelerationY = 0.0f;
	public float repeat = 1f;
    public int bonus;
	public float potDampening = .5f;
	public AudioMixerSnapshot backgroundMusicSnapshot;
	public AudioMixerSnapshot poweredUpMusicSnapshot;
	public AudioMixerSnapshot musicOffSnapshot;

	// Use this for initialization
	void Start () {
		refresh();
		musicOffSnapshot.TransitionTo(.01f);
	}
    public void checkFlair()
    {
        if (transform.position.y > 5)
        {
            bonus++;
        }
    }
    public void alarm()
    {
        GetComponent<AudioSource>().PlayOneShot(alarmFx, 2f);
    }

    public void perfectPour(int count) {
		streak = count;
		if (streak > 4) {
			GetComponent<AudioSource>().PlayOneShot(perfectPourFx,2f);
			feedbackMessage.text = "STREAK x" + count;
		}
		else {
			GetComponent<AudioSource>().PlayOneShot(perfectPourFx,2f);
			feedbackMessage.text = "AMAZING!";
		}
		feedback.SetTrigger("show");
	}


	public void refresh() {
        playing = false;
		pouring = false;
		gameOver = false;
		gameControl.SetActive(true);
		hud.SetTrigger("normal");
		dropsLeft = 105;
        bonus = 0;
        cupsServed = 0;
        moneyMade = 0;
		served.text = moneyMade.ToString("$0.00");
		streak = 0;
        spawner.Reset();
		selectedDroplet = 0;
        highlightCanister(0);
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;
	}


	public void badPourFx() {
		GetComponent<AudioSource>().PlayOneShot(missedFx,2f);
	}

	public void wrongTea() {
		if (!gameOver) {
			GetComponent<AudioSource>().PlayOneShot(wrongTeaFx,2f);
			musicOffSnapshot.TransitionTo(.01f);
		}
		gameOver = true;
		spawner.shouldSpawn = false;
		failureMessage.text = "You must be exhausted, serving customers the wrong tea and all. Maybe some Lemon City Tea would lift your spirits!";
		endServed.text = moneyMade.ToString("$0.00");
		completePanel.SetActive(true);
		gameControl.SetActive(false);
		pouring = false;
        playing = false;
	}

	public void buyLemonCityTea() {
		Application.OpenURL ("http://www.lemoncitytea.com/collections/tea-blends");
	}

	public void missed() {
		if(!gameOver) {
			GetComponent<AudioSource>().PlayOneShot(missedFx,2f);
			musicOffSnapshot.TransitionTo(.01f);
			Debug.Log("MUSIC OFF SNAPSHOT");

		}
		gameOver = true;
		spawner.shouldSpawn = false;
		failureMessage.text = "Better close up your shop, looks like you've got too many customers to handle. Maybe some Lemon City Tea would lift your spirits!";
		endServed.text = cupsServed + " Customers Served";
		completePanel.SetActive(true);
		gameControl.SetActive(false);
		pouring = false;
        playing = false;
	}


	public void chooseTea() {
		selectedDroplet++;
		if (selectedDroplet >= numberOfTeas) {
			selectedDroplet = 0;
		}
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;
        highlightCanister(selectedDroplet);

	}

    private void highlightCanister(int tea)
    {
        for (int i = 0; i < canisters.Length; i++)
        {
            canisters[i].GetComponent<Outline>().enabled = false;
            canisters[i].GetComponent<Shadow>().enabled = false;

        }
        canisters[tea].GetComponent<Outline>().enabled = true;
        canisters[tea].GetComponent<Shadow>().enabled = true;

    }

    public void selectTea(int tea) {
		selectedDroplet = tea;
		Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
		c.a = 100f;
		GetComponent<SpriteRenderer>().color = c;
        highlightCanister(tea);    
	}

    public void lift()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (transform.position.y >= 2 && transform.position.y < 7)
        {
                pos.y += .1f;
        }
        transform.position = pos;


    }

    public void move(InputAction.CallbackContext context) {

		Debug.Log("Movement Called:" + context);
		/*
				Vector3 pos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

				if (transform.position.x > -8 && transform.position.x < 8) {
					if (Input.acceleration.x != 0) {
						pos.x += (Input.acceleration.x * .7f);
					} else {
						pos.x += (Input.GetAxis ("Horizontal") * .1f);
					}
				} else {
					pos = Vector3.MoveTowards(transform.position, new Vector3(0f,transform.position.y, transform.position.z), .01f);
				}
				transform.position = pos;
		*/
		Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		pos.x += context.ReadValue<Vector2>().x;
	}

    void Update () {
		if (playing) {
            if (!pouring)
            {
					Vector2 pos = new Vector2(transform.position.x, transform.position.y);
					float x = Gamepad.current.leftStick.value.x * potDampening;
					pos.x += x;

				if (pos.x < 10 && pos.x > -10)
				{
					transform.position = pos;
				}


			}
			else
            {
                lift();
            }
			
			if(Gamepad.current.buttonEast.wasPressedThisFrame)
            {
				pouring = true;
            }
			if(Gamepad.current.buttonEast.wasReleasedThisFrame)
            {
				pouring = false;
            }

			if(Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
				chooseTea();

            }
			
            if (!IsInvoking("CheckPour")) {
				InvokeRepeating("CheckPour",0,repeat);
			}

			
			if(pouring) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * 10f);
			}
			else {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10f);
			}
			
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 2f, transform.position.z), .1f);
            served.text = moneyMade.ToString("$0.00");
		}
		else {
			CancelInvoke("CheckPour");
		}
	}

	public void Pour(InputAction.CallbackContext context)
    {
		Debug.Log("POURING!");
	
		if(context.started)
        {
			pouring = true;
        }
		if(context.performed)
        {
			pouring = false;
        }
		if(context.canceled)
        {
			pouring = false;
        }

    }


    public void setPouring(bool p)
    {
        pouring = p;
    }

	void CheckPour() {
		if (transform.rotation.eulerAngles.z <= 290 && transform.rotation.eulerAngles.z  >= 200) {
				Pour();
		}

	}
	public void Pour() {
		if(Time.realtimeSinceStartup - debounce > repeat) { 
			Vector3 pos = spigot.transform.position;
			Quaternion qua = new Quaternion(0,0,0,0);
			GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);	
		}
	}

	public void Begin() {
		gameOver = false;
        playing = true;
	}
}
