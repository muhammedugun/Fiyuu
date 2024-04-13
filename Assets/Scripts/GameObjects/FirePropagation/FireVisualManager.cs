using UnityEngine;

/// <summary>
/// bu sınıfı yangın particle sistemi kullanır. <br>Bu sınıfın genel amacı ısınma, yanma ve sönme 
/// durumlarında hangi partikul efektlerin aktif olacağını ayarlamaktır</br>
/// </summary>
public class FireVisualManager : MonoBehaviour {
    private ParticleSystem[] m_particleSystems;
    [Tooltip("Yangında kullanılan parçacık sistemleriyle aynı sayıda olmalı ve bu parçacık sistemlerinden hangisi simülasyonun ısınma adımında aktif olmalıdır.")]
    public bool[] m_heatUp;
    [Tooltip("Yangında kullanılan parçacık sistemleri ile aynı sayıda olmalı ve bu parçacık sistemlerinden hangisi simülasyonun ateşleme adımında aktif olmalıdır.")]
    public bool[] m_ignition;
    [Tooltip("Yangında kullanılan parçacık sistemleri ile aynı sayıda olmalı ve simülasyonun söndürme adımında bu parçacık sistemlerinden hangisi aktif olmalıdır.")]
    public bool[] m_extinguish;
    /// <summary>
    /// ısı durumu
    /// </summary>
    private bool m_heatState = false;
    /// <summary>
    /// ateşleme durumu
    /// </summary>
    private bool m_ignitionState = false;
    /// <summary>
    /// söndürme durumu
    /// </summary>
    private bool m_extinguishState = false;

    private bool m_heatStateSet = false;
    private bool m_ignitionStateSet = false;
    private bool m_extinguishStateSet = false;

    // Use this for initialization
    void Start () {

        m_particleSystems = GetComponentsInChildren<ParticleSystem>(); //partlicle system objesindeki tüm particle sistemler diziye aktarılır
        //dizilerin boyutu yanlış sayıdaysa hata mesajları fırlatır
        if (m_heatUp.Length > m_particleSystems.Length)
            Debug.LogError(gameObject.name + " FireVisualManager::heatUp bigger then the number of children with Particle Systems");

        if (m_ignition.Length > m_particleSystems.Length)
            Debug.LogError(gameObject.name + " FireVisualManager::ignition bigger then the number of children with Particle Systems");

        if (m_extinguish.Length > m_particleSystems.Length)
            Debug.LogError(gameObject.name + " FireVisualManager::extingush bigger then the number of children with Particle Systems");

        // start off by turning all particle systems off
        for (int i = 0; i < m_particleSystems.Length; i++)
            m_particleSystems[i].gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
        if(m_heatState && m_heatStateSet == false)
        {
            for(int i = 0; i < m_particleSystems.Length; i++)
                m_particleSystems[i].gameObject.SetActive(m_heatUp[i]);

            m_heatStateSet = true;
        }
        else if(m_ignitionState && m_ignitionStateSet == false)
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
                m_particleSystems[i].gameObject.SetActive(m_ignition[i]);

            m_ignitionStateSet = true;
        }
        else if(m_extinguishState && m_extinguishStateSet == false)
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
                m_particleSystems[i].gameObject.SetActive(m_extinguish[i]);

            m_extinguishStateSet = true;
        }
	}

    // brief Set the state to the heat state
    public void SetHeatState()
    {
        m_heatState = true;
        m_ignitionState = false;
        m_extinguishState = false;
    }

    /// <summary>
    /// Durumu ateşleme durumuna ayarla
    /// </summary>
    public void SetIgnitionState()
    {
        m_heatState = false;
        m_ignitionState = true;
        m_extinguishState = false;
    }

    /// <summary>
    /// State'ti söndürme durumuna ayarla
    /// </summary>
    public void SetExtingushState()
    {
        m_heatState = false;
        m_ignitionState = false;
        m_extinguishState = true;
    }
}
