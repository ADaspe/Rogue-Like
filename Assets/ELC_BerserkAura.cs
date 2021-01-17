using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ELC_BerserkAura : MonoBehaviour
{
    public float maxLightIntensity;
    public float emissionMax;
    public ParticleSystem particleSys;
    public Light2D light2D;
    public ELC_PlayerStatManager playerStats;

    void Update()
    {
        var emission = particleSys.emission;
        emission.rateOverTime = emissionMax * (1 - (playerStats.currentHealth / playerStats.MaxHealth));
        light2D.intensity = maxLightIntensity * (1 - (playerStats.currentHealth / playerStats.MaxHealth));
    }
}
