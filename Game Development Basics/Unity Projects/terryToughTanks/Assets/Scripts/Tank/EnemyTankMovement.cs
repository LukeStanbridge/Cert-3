﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankMovement : MonoBehaviour
{
    // List for the transforms
    public List<Transform> _waypoints = new List<Transform>();
    // integer for the current waypoint that the enemy tank is followings
    private int currentWaypoint;
    // The tank will stop moving towards the player once it reaches this distance
    public float m_CloseDistance = 8f;
    // The tank's turret object
    public Transform m_Turret;
    // Enable a spawn point for this tank
    public Transform spawnPoint;
    // A reference to the player -this will be set when the enemy is loaded
    private GameObject m_Player;
    // A reference to the nav mesh agent component
    private NavMeshAgent m_NavAgent;
    // A reference to the rigidbody component
    private Rigidbody m_Rigidbody;

    // Will be set to true when this tank should follow the player
    private bool m_Follow;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Follow = false;        
    }

    private void OnEnable()
    {
        // when the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false;
        m_Follow = false; // when enemy tank spawns/respawns, set to false so it doesn't follow player
    }

    private void OnDisable()
    {
        // when the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = false;
            m_NavAgent.SetDestination(spawnPoint.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Follow == false)
        {
            if (_waypoints.Count <= 0)
            {
                return;
            }

            if (currentWaypoint < _waypoints.Count)
            {
                if (Vector3.Distance(transform.position, _waypoints[currentWaypoint].position) > 2)
                {
                    m_NavAgent.SetDestination(_waypoints[currentWaypoint].position);
                }
                else
                {
                    currentWaypoint++;
                }
            }
            else
            {
                currentWaypoint = 0;
            }
        }
        else
        {
            // get distance from player to enemy tank
            float distance = (m_Player.transform.position - transform.position).magnitude;
            // if distance is less than stop distance, then stop moving
            if (distance > m_CloseDistance)
            {
                m_NavAgent.SetDestination(m_Player.transform.position);
                m_NavAgent.isStopped = false;
            }
            else
            {
                m_NavAgent.isStopped = true;
            }

            if (m_Turret != null)
            {
                m_Turret.LookAt(m_Player.transform);
            }
        }        
    }

    // add a secondary firing option to tank
    // despawn player/enemy tanks 3 seconds after win/lose use IEnumerator
    // high scores meant to save constantly?
    // make capture zones in the map
    // add power ups, health, shields
    // edit UI
    // comment all code
}
