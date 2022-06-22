using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    // The amount of health each tank starts with
    public float m_StartingHealth = 150;

    // A prefab that will be instantiated in Awake, then used whenever the tank dies
    public GameObject m_ExplosionPrefab;
    public GameObject m_EnemyTank;
    public GameObject m_EnemyTank2;
    public GameObject m_EnemyTank3;
    public GameObject m_BossTank;
    public HealthBar m_HealthBar;

    [SerializeField]
    private float m_CurrentHealth;
    private bool m_Dead;
    // The particle system that will play when the tank is destroyed
    private ParticleSystem m_ExplosionParticles;

    private void Awake()
    {
        // Instantiate the explosion prefab and get a reference to the particle system on it
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        // Disable the prefab so it can be activated when it's required
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // When the tank is enabled, reset the tank's health and whether or not it's dead
        m_CurrentHealth = m_StartingHealth;
        m_HealthBar.SetMaxHealth((int)m_StartingHealth); // set health bar to full
        m_Dead = false;
    }

    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done
        m_CurrentHealth -= amount;
        m_HealthBar.SetHealth((int)m_CurrentHealth);

        // if the current health is at or below zero and it has not yet been registered, call OnDeath
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        // Set the flag so that this function is only called once
        m_Dead = true;

        // Move the instantiated explosion prefab to the tank's position and turn it on
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        // Play the particle system of the tank exploding
        m_ExplosionParticles.Play();

        // Turn the player tank and enemy tanks off, also resets player follow and health
        gameObject.SetActive(false);
        m_EnemyTank.gameObject.SetActive(false);
        m_EnemyTank2.gameObject.SetActive(false);
        m_EnemyTank3.gameObject.SetActive(false);
        m_BossTank.gameObject.SetActive(false);
    }

    public void SetHealth(float percentHealth) // function to reset player health, called in game manager script
    {
        m_CurrentHealth = Mathf.Clamp(percentHealth * m_StartingHealth, 0, m_StartingHealth);
    }
}
