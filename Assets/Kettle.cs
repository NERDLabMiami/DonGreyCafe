using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public struct Vessel
{
    public int dropCapacity;
    public int dropAmount;
}

[RequireComponent(typeof(PlayerInput))]
public class Kettle : MonoBehaviour
{


    [Range(0.01f, .1f)]
    public float potDampening;

    [Range(0.05f, 1)]
    public float pourSpeed = 1;
    
    public GameObject spigot;
    public GameObject[] droplet;
    private Vessel[] vessels;

    private int selectedDroplet = 0;
    private bool pouring = false;
    private bool refilling = false;
    private float speedX = 0;
    private float laneY = 2f;



    private void Start()
    {
        vessels = new Vessel[droplet.Length];
        for(int i = 0; i < droplet.Length; i++)
        {
            Vessel vessel = new Vessel();
            vessel.dropCapacity = droplet[i].GetComponent<Droplet>().maxAmount;
            vessel.dropAmount = vessel.dropCapacity;

            Debug.Log("MAX AMOUNT FOR " + i + " : " + droplet[i].GetComponent<Droplet>().maxAmount);
            vessels[i] = vessel;
        }
        laneY = transform.position.y;

    }

    private void Update()
    {

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        float x = speedX * potDampening;
        pos.x += x;


        if (pos.x < 10 && pos.x > -10)
        {
            transform.position = pos;
        }


        if (!IsInvoking("CheckPour"))
        {
            InvokeRepeating("CheckPour", 0, pourSpeed);
        }


        if (pouring)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -90), Time.deltaTime * 10f);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10f);
        }
        
        if(refilling)
        {
            Vector3 refillPosition = transform.position;
            refillPosition.y += 5;
            GameObject go = Instantiate(droplet[selectedDroplet], transform.parent);
            go.transform.position = refillPosition;
            if(vessels[selectedDroplet].dropCapacity <= vessels[selectedDroplet].dropAmount)
            {
                refilling = false;
            }           
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, laneY, transform.position.z), .1f);


    }

    void OnMove(InputValue value)
    {
        speedX = value.Get<Vector2>().x;
        if(value.Get<Vector2>().y == -1)
        {
            //down
            laneY--;
        }
        if(value.Get<Vector2>().y == 1)
        {
            //up
            laneY++;
        }

        //CENTER
        if(value.Get<Vector2>().y == 0)
        {
            laneY = 2f;
        }
        


        if(laneY <= 0)
        {
            laneY = 0;
        }

        if(laneY >= 4)
        {
            laneY = 4;
        }
        

        Debug.Log("Moving " + value.Get<Vector2>().x + " IN LANE " + laneY);
    }

    void OnSwap(InputValue value)
    {
        selectedDroplet = (int)(selectedDroplet + value.Get<float>());
        if (selectedDroplet >= droplet.Length)
        {
            selectedDroplet = 0;
        }

        if(selectedDroplet < 0)
        {
            selectedDroplet = droplet.Length - 1;
        }

        Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
        float alpha = (float)vessels[selectedDroplet].dropAmount / (float)vessels[selectedDroplet].dropCapacity;
        c.a = alpha;

        GetComponent<SpriteRenderer>().color = c;
        //        highlightCanister(selectedDroplet);

    }

    void OnPour(InputValue value)
    {
        if (value.Get<float>() == 1)
        {
            pouring = true;
            Debug.Log("START POUR");

        }
        else
        {
            pouring = false;
            Debug.Log("STOP POUR");

        }
    }

    void OnRefill(InputValue value)
    {
        if(value.Get<float>() == 1)
        {
            refilling = true;
        }

    }

    public void chooseTea()
    {

    }
    void CheckPour()
    {
        if (transform.rotation.eulerAngles.z <= 290 && transform.rotation.eulerAngles.z >= 200 && vessels[selectedDroplet].dropAmount > 0)
        {
            Pour();
        }
    }

    public void Pour()
    {
        if (Time.realtimeSinceStartup > 1)
        {
            Vector3 pos = spigot.transform.position;
            Quaternion qua = new Quaternion(0, 0, 0, 0);
            GameObject tea = (GameObject)Instantiate(droplet[selectedDroplet], pos, qua);
            vessels[selectedDroplet].dropAmount--;        
            if (vessels[selectedDroplet].dropAmount <= 0)
            {
                CancelInvoke("CheckPour");
            }
            Color c = tea.GetComponent<SpriteRenderer>().color;
            float alpha = (float)vessels[selectedDroplet].dropAmount / (float)vessels[selectedDroplet].dropCapacity;
            Debug.Log("ALPHA: " + alpha);
            c.a = alpha + .1f;
            GetComponent<SpriteRenderer>().color = c;

        }
    }

    public void selectTea(int tea)
    {
        selectedDroplet = tea;
        Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
        c.a = 100f;
        GetComponent<SpriteRenderer>().color = c;
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


    public bool HasRoom()
    {
      //  Debug.Log("VESSEL HAS " + vessels[selectedDroplet].dropAmount + " AND CAPACITY OF " + vessels[selectedDroplet].dropCapacity);
        return vessels[selectedDroplet].dropAmount < vessels[selectedDroplet].dropCapacity;
    }

    public void AddTea()
    {
        vessels[selectedDroplet].dropAmount++;
        Color c = droplet[selectedDroplet].GetComponent<SpriteRenderer>().color;
        float alpha = (float)vessels[selectedDroplet].dropAmount / (float)vessels[selectedDroplet].dropCapacity;
        c.a = alpha;
        GetComponent<SpriteRenderer>().color = c;
    }


}
