using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CristalManager : MonoBehaviour
{
    public Cristal[] AllCristals;
    public int ClosestCristalsToShow = 3;
    public int RandomCristalsToShow = 3;
    
    private Camera m_Camera;
    private Cristal[] m_ClosestCristals;
    private List<Cristal> m_RandomCristals = new List<Cristal>();
    
    private float m_RandomCristalTimer = 0f;
    private float m_RandomCristalDuration = 10f;
    private bool m_RandomCristalsActive = false;
    
    private void Start()
    {
        m_Camera = Camera.main;
        m_ClosestCristals = new Cristal[ClosestCristalsToShow];
    }

    private void Update()
    {
        if (CheckpointManager.Instance.CurrentCheckpoint < 3)
        {
            return;
        }

        // Find 3 closest cristals
        FindClosestCristals();
        
        // Handle random crystal timing
        UpdateRandomCristalTimer();
        
        // Update crystal visibility or effects based on proximity and random selection
        UpdateCristalEffects();
    }
    
    private void FindClosestCristals()
    {
        // Get camera position
        Vector3 cameraPosition = m_Camera.transform.position;
        
        // Calculate distances for all crystals
        var cristalsWithDistances = new System.Collections.Generic.List<(Cristal cristal, float distance)>();
        
        foreach (var cristal in AllCristals)
        {
            if (cristal == null) continue;
            
            float distance = Vector3.Distance(cameraPosition, cristal.transform.position);
            cristalsWithDistances.Add((cristal, distance));
        }
        
        // Sort by distance
        cristalsWithDistances.Sort((a, b) => a.distance.CompareTo(b.distance));
        
        // Get the closest ones
        for (int i = 0; i < ClosestCristalsToShow && i < cristalsWithDistances.Count; i++)
        {
            m_ClosestCristals[i] = cristalsWithDistances[i].cristal;
        }
    }
    
    private void UpdateRandomCristalTimer()
    {
        m_RandomCristalTimer += Time.deltaTime;
        
        // If random crystals are not active and timer reached duration, activate new random crystals
        if (!m_RandomCristalsActive && m_RandomCristalTimer >= m_RandomCristalDuration)
        {
            ActivateRandomCristals();
            m_RandomCristalsActive = true;
            m_RandomCristalTimer = 0f;
        }
        // If random crystals are active and timer reached duration, deactivate them
        else if (m_RandomCristalsActive && m_RandomCristalTimer >= m_RandomCristalDuration)
        {
            m_RandomCristalsActive = false;
            m_RandomCristalTimer = 0f;
            m_RandomCristals.Clear();
        }
    }
    
    private void ActivateRandomCristals()
    {
        m_RandomCristals.Clear();
        
        // Create a temporary list of all crystals
        var availableCristals = new List<Cristal>(AllCristals);
        
        // Randomly select crystals
        for (int i = 0; i < RandomCristalsToShow && availableCristals.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableCristals.Count);
            m_RandomCristals.Add(availableCristals[randomIndex]);
            availableCristals.RemoveAt(randomIndex);
        }
    }
    
    private void UpdateCristalEffects()
    {
        // Disable effects for all crystals first
        foreach (var cristal in AllCristals)
        {
            if (cristal == null) continue;
            cristal.SetHighlighted(false);
        }
        
        // Enable effects for closest crystals
        foreach (var cristal in m_ClosestCristals)
        {
            if (cristal == null) continue;
            cristal.SetHighlighted(true);
        }
        
        // Enable effects for random crystals if active
        if (m_RandomCristalsActive)
        {
            foreach (var cristal in m_RandomCristals)
            {
                if (cristal == null) continue;
                cristal.SetHighlighted(true);
            }
        }
    }
    
    // For debugging
    public Cristal[] GetClosestCristals()
    {
        return m_ClosestCristals;
    }
    
    // For debugging
    public List<Cristal> GetRandomCristals()
    {
        return m_RandomCristals;
    }
}