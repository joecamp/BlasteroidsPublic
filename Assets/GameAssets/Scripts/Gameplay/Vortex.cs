using System.Collections.Generic;
using System;

using UnityEngine;

public class Vortex : MonoBehaviour
{
    public bool IsActive { get; private set; } = false;

    [SerializeField] private List<ParticleSystem> vortexParticles;

    public static Action<Vortex> OnVortexPulse;

    private void Update()
    {
        if(IsActive)
        {
            transform.position = MouseManager.Instance.MouseWorldPosition;
            
            OnVortexPulse?.Invoke(this);
        }
    }

    public void SetActive(bool active)
    {
        IsActive = active;

        if(IsActive)
        {
            foreach (ParticleSystem particle in vortexParticles)
            {
                particle.Play();
            }
        }
        else
        {
            foreach (ParticleSystem particle in vortexParticles)
            {
                particle.Stop();
            }
        }
    }
}