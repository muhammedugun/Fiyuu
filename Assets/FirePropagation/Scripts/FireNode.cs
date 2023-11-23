/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 01/02/2017
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireNode : MonoBehaviour
{
    [Tooltip("Yangında kullanılacak prefab.")]
    public GameObject m_fire;
    [Tooltip("Bu düğümle bağlantılı diğer düğümleri içerir. Bu düğüm ateşlendiğinde bağlantılı düğümler ısınmaya başlayacaktır. " +
        "\nNot: Eklediğiniz objelerde FireNode olmasına dikkat edin")]
    public List<GameObject> m_links = null;
    [Tooltip("Node'un Can Puanı ne kadar yüksek olursa, hücrenin ısınmasını ve tutuşmasını yavaşlatır.")]
    public float m_HP = 50.0f;
    [Tooltip("Node'taki yakıt miktarı.")]
    public float m_fuel = 50.0f;
    /// <summary>
    /// Ateşi söndürme eşiği
    /// </summary>
    private float m_extinguishThreshold;
    [SerializeField] private float m_combustionRateValue; // normalde serialize değil
    /// <summary>
    /// Yangın yeni(şimdi) başladı
    /// </summary>
    private bool m_fireJustStarted = false;
    /// <summary>
    /// Yanıyor mu?
    /// </summary>
    [SerializeField] private bool m_isAlight = false; // normalde serializefield değil
    /// <summary>
    /// Yangın sönmüş
    /// </summary>
    private bool m_extingushed = false;
    private bool m_clean = false;
    private FireVisualManager m_visualMgr = null;
    public GameObject flames { get { return m_fire; } }
    /// <summary>
    /// Yanıyor mu?
    /// </summary>
    public bool isAlight { get { return m_isAlight; } }
    public bool fireJustStarted
    {
        get { return m_fireJustStarted; }
        set { m_fireJustStarted = value; }
    }
    public float HP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }
    public float extinguishThreshold { get { return m_extinguishThreshold; } }


    // Use this for initialization
    void Start()
    {
        // If a tag was not set in the editor then fallback to slower why of finding the object
        try
        {
            GameObject manager = GameObject.FindWithTag("Fire");

            if (manager != null)
            {
                FireManager fireManager = manager.GetComponent<FireManager>();

                if (fireManager != null)
                {
                    m_extinguishThreshold = m_fuel * fireManager.visualExtinguishThreshold;
                    m_combustionRateValue = fireManager.nodeCombustionRate;
                }
                else
                {
                    m_extinguishThreshold = m_fuel;
                    m_combustionRateValue = 1.0f;
                }
            }
        }
        catch
        {
            // Get the terrain from the fire manager
            FireManager fireManager = FindObjectOfType<FireManager>();

            if (fireManager != null)
            {
                m_extinguishThreshold = m_fuel * fireManager.visualExtinguishThreshold;
                m_combustionRateValue = fireManager.nodeCombustionRate;
            }
            else
            {
                m_extinguishThreshold = m_fuel;
                m_combustionRateValue = 1.0f;
            }
        }
    }

    // brief Kills the attached child particle systems
    private void KillFlames()
    {
        Destroy(m_fire);
    }


    void Update()
    {
        // can sıfır ya da daha azsa ve yanmıyorsam yangını başlat
        if (m_HP <= 0.0f && !m_isAlight)
            m_fireJustStarted = true;

        Ingition();
        Combustion();
    }

    // brief Has this node ran out of fire fuel
    // return bool true if it has
    public bool NodeConsumed()
    {
        if (m_clean == true)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Update fonkisyonunu 1 kez çalıştırmaya zorlar
    /// </summary>
    public void ForceUpdate()
    {
        Update();
    }

    /// <summary>
    /// Fire Manager içindeki Fire Prefab setinin bir yangınını yaratır
    /// </summary>
    /// <param name="position">Ateş oluşturacak konum</param>
    /// <param name="Fire">Ateş objesi</param>
    public void InstantiateFire(Vector3 position, GameObject Fire)
    {
        if (m_fireJustStarted)
        {
            m_fire = (GameObject)Instantiate(Fire, position, new Quaternion());

            // Should be set after fire extinguished
            m_isAlight = true;
        }
    }

    /// <summary>
    /// Ateşlemeyi başlatır yani node içindeki yakıtın yanmasını sağlar. 
    /// </summary>
    void Ingition()
    {
        if (m_fireJustStarted && !m_extingushed)
        {
            InstantiateFire(transform.position, m_fire);
            m_fireJustStarted = false;

            GetVisualManager();

            if(m_visualMgr != null)
                m_visualMgr.SetIgnitionState();
        }
    }

    /// <summary>
    /// Yangının alevlenmesinden sonra tetiklenen yanma adımı
    /// </summary>
    void Combustion()
    {
        if (m_isAlight)
        {
            m_fireJustStarted = false;

            m_fuel -= m_combustionRateValue * Time.deltaTime;

            if (m_fuel < m_extinguishThreshold)
            {
                // Bu işlev çağrılmadan önce Ignition(ateşleme)'da çağrılan getFireManager() kadar geçerli olmalıdır
                if (m_visualMgr != null)
                    m_visualMgr.SetExtingushState();
            }

            if (m_fuel <= 0.0f)
            {
                m_isAlight = false;
                m_extingushed = true;

                KillFlames();
                m_clean = true;
            }
        }
    }

    // VisualManager'i alır ve ona bir referans ayarlar
    void GetVisualManager()
    {
        if (m_visualMgr == null)
            m_visualMgr = m_fire.GetComponent<FireVisualManager>();
    }
}
