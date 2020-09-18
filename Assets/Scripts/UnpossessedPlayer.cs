using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpossessedPlayer : Possessable
{
    public static UnpossessedPlayer Instance;
    [SerializeField]
    ParticleSystem ps;
    new private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        base.Awake();
        PlayerController.OnPossession.AddListener(checkAndEnableParticleSystem);
    }

    new void Possess()
    {
        base.Possess();
    }

    void StopPlayerParticleSystem()
    {
        ps?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    void PlayPlayerParticleSystem()
    {
        ps?.Play();
    }

    void checkAndEnableParticleSystem()
    {
        if (PlayerController.CurrentPossessed == this)
            PlayPlayerParticleSystem();
        else
            StopPlayerParticleSystem();
    }

    
}
