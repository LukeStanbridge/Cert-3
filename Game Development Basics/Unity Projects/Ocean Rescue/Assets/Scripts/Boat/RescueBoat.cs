using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueBoat : MonoBehaviour
{
    public GameObject m_passenger = null;
    private float m_timer = 0;
    public float m_rescueTime = 3;
    public float m_droppoffTime = 3;

    private bool m_hasPassenger = false;

    // Start is called before the first frame update
    void Start()
    {
        m_passenger.SetActive(false); // sets passenger in boat to invisible
        m_hasPassenger = false; // dictates no passenger in boat
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        // if statement to determine if player collides with a swimmer in the water and doesn't already have a passenger on the boat
        if (collider.gameObject.tag == "Swimmer" && m_hasPassenger == false)
        {
            m_timer = 0;
        }
        // if statement to determine if player collides with the drop zone and has a passenger on board
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger == true)
        {
            m_timer = 0;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Swimmer" && m_hasPassenger == false)
        {
            m_timer += Time.deltaTime;
            Debug.Log((m_rescueTime / m_timer));
            if (m_timer >= m_rescueTime)
            {
                PickupSwimmer(collider.gameObject);
            }
        }
        else if (collider.gameObject.tag == "DropZone" && m_hasPassenger == true)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_droppoffTime)
            {
                DropoffSwimmer();
            }
        }   
    }

    public void PickupSwimmer(GameObject swimmer)
    {
        swimmer.SetActive(false);
        m_passenger.SetActive(true);
        m_hasPassenger = true;
    }

    public void DropoffSwimmer()
    {
        m_passenger.SetActive(false);
        m_hasPassenger = false;
    }

    //add passenger on droppoff zone
    //comment all code
    //fix level design
    //change camera follow
}
