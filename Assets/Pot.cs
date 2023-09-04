using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Pot : MonoBehaviour
{

    private Vector2 m_Move;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnMove(InputValue value)
    {
        Debug.Log("MOVE: " + value.Get<Vector2>());        
    }

    public void OnSwap(InputValue value)
    {
        Debug.Log("SWAP: " + value.Get());
    }

    public void OnPour(InputValue value)
    {
        Debug.Log("POUR: " + value.Get());

    }
}
