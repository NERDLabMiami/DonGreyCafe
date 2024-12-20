using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Chair
{
    public GameObject seat;
    public bool occupied;
}

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }

    public Chair[] chairs;
    public float timeBetweenSpawns;
    public float customerWaitTime = 9999f;
    public int dropsPerGlass = 7;
    public bool shouldSpawn = false;
    public bool streak;
    public PotControl pot;
    public GameObject[] teaSelections;
    public GameObject streakParticleSystem;
    public Teachievement achievements;
    public int maxCustomers = 4;

    private int customersBeingServed = 0;
    private int streakLength;
    private int longestStreak;
    private float timer;
    private float spawnDelay = 0f;
    public int numberOfTeas = 1;

    public GameObject[] customers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ActivateTeaSelections();
        shouldSpawn = true;
    }

    void Update()
    {
        if (!shouldSpawn) return;

        timer += Time.deltaTime;
        int peopleCounter = CountOccupiedChairs();

        if (timer >= timeBetweenSpawns + spawnDelay || peopleCounter == 0)
        {
            SpawnCustomer();
            timer = 0f;
            spawnDelay = 0f;
        }
    }

    public void Reset()
    {
        longestStreak = 0;
        streakLength = 0;
        streak = false;
        if (streakParticleSystem != null)
        {
            streakParticleSystem.SetActive(false);
        }
    }

    public void Discard(int column)
    {
        SetChairOccupied(column, false);
        timer = 0f;
    }

    public void FlashWarningMessage(string msg)
    {
        Debug.Log($"Warning: {msg}");
    }

    public void Served(int column)
    {
        float tip = 1;
        UpdateStreak(tip);
        customersBeingServed--;
        spawnDelay = GetPauseTimeFromColumn(column);
        SyncTeaSelections();
    }

    private void UpdateStreak(float tip)
    {
        if (tip >= 1)
        {
            streakLength++;
        }
        else
        {
            EndStreak();
        }

        if (streakLength > 3)
        {
            streak = true;
            if (streakLength > longestStreak) longestStreak = streakLength;
        }
    }

    public void EndStreak(int column)
    {
        streak = false;
        streakLength = 0;
        if (streakParticleSystem != null)
        {
            streakParticleSystem.SetActive(false);
        }
        if (column >= 0 && column < chairs.Length)
        {
            SetChairOccupied(column, false);
        }
    }

    public void EndStreak()
    {
        streak = false;
        streakLength = 0;
        if (streakParticleSystem != null)
        {
            streakParticleSystem.SetActive(false);
        }
    }

    private void SetChairOccupied(int index, bool isOccupied)
    {
        if (index >= 0 && index < chairs.Length)
        {
            chairs[index].occupied = isOccupied;
        }
    }

    private int CountOccupiedChairs()
    {
        int count = 0;
        foreach (var chair in chairs)
        {
            if (chair.occupied) count++;
        }
        return count;
    }

    private void SpawnCustomer()
    {
        if (customersBeingServed >= maxCustomers) return;

        int randomChairIndex = GetRandomAvailableChairIndex();
        if (randomChairIndex != -1)
        {
            SetChairOccupied(randomChairIndex, true);
            InstantiateCustomer(customers[Random.Range(0, numberOfTeas)], chairs[randomChairIndex].seat.transform.position, chairs[randomChairIndex].seat, randomChairIndex);
        }
    }

    private int GetRandomAvailableChairIndex()
    {
        List<int> availableChairs = new List<int>();
        for (int i = 0; i < chairs.Length; i++)
        {
            if (!chairs[i].occupied) availableChairs.Add(i);
        }
        if (availableChairs.Count > 0)
        {
            return availableChairs[Random.Range(0, availableChairs.Count)];
        }
        return -1;
    }

    private void InstantiateCustomer(GameObject customerPrefab, Vector3 position, GameObject parent, int chairIndex)
    {
        GameObject customerInstance = Instantiate(customerPrefab, position, Quaternion.identity, parent.transform);
        Person person = customerInstance.GetComponent<Person>();
        if (person != null)
        {
            person.column = chairIndex;
            person.StartMoving(customerWaitTime);
        }
        customersBeingServed++;
    }

    private void RemoveCustomerFromChair(int column)
    {
        if (column >= 0 && column < chairs.Length)
        {
            foreach (Transform child in chairs[column].seat.transform)
            {
                if (child.CompareTag("Customer"))
                {
                    Destroy(child.gameObject);
                }
            }
            SetChairOccupied(column, false);
        }
    }

    private void ActivateTeaSelections()
    {
        for (int i = 0; i < Mathf.Min(numberOfTeas, teaSelections.Length); i++)
        {
            teaSelections[i].SetActive(true);
        }
    }

    private float GetPauseTimeFromColumn(int column)
    {
        if (column >= 0 && column < chairs.Length)
        {
            GameObject chairObject = chairs[column].seat;
            if (chairObject != null)
            {
                Person person = chairObject.GetComponentInChildren<Person>();
                if (person != null)
                {
                    return person.pauseBeforeSpawning;
                }
            }
        }
        return 0f; // Default to no delay if person or chair is not found
    }

    private void SyncTeaSelections()
    {
        for (int i = 0; i < teaSelections.Length; i++)
        {
            teaSelections[i].SetActive(i < customers.Length && customers[i].activeSelf);
        }
    }

    public Color[] GetActiveCustomerColors()
    {
        List<Color> activeColors = new List<Color>();
        foreach(GameObject tea in teaSelections)
        {
            if(tea.activeSelf)
            {
                activeColors.Add(tea.GetComponent<Image>().color);

            }
        }
        return activeColors.ToArray();
    }
}
