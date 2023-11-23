/* Copyright (c) 2016-2017 Lewis Ward
// Fire Propagation System
// author: Lewis Ward
// date  : 03/02/2017
*/
using UnityEngine;
using System.Collections;

public class FireNodeChain : MonoBehaviour {
    [Tooltip("Değer ne kadar yüksek olursa, yangın yakıtı da o kadar çabuk tutuşturur")]
    public float m_firePropagationSpeed = 20.0f;
    [Tooltip("Bu zincirdeki düğümleri temsil etmektedir. Not: yangınların doğru şekilde başlaması için tüm düğümlere sahip olmalıdır")]
    public FireNode[] m_fireNodes = null;
    [Tooltip("Tüm düğümler ateşe verildikten sonra GameObject'in yok edilmesi gerekiyorsa etkinleştirin, ağaçlar için etkinleştirmeyin!!")]
    public bool m_destroyAfterFire = false;
    [Tooltip("Tüm düğümler ateşlendikten sonra GameObject'in başka bir ağ ile değiştirilmesi gerekiyorsa etkinleştirin")]
    public bool m_replaceAfterFire = false;
    [Tooltip("Bu nesnenin değiştirilmesi gereken GameObject")]
    public GameObject m_replacementMesh;
    /// <summary>
    /// Yanma hızı değeri
    /// </summary>
    private float m_combustionRateValue = 1.0f;
    private bool m_destroyedAlready = false;
    private bool m_replacedAlready = false;
    /// <summary>
    /// m_fireNodes'in üzerindeki tüm değerler atanmışsa true döndürür
    /// </summary>
    private bool m_validChain = true;

    public float propagationSpeed
    {
        get { return m_firePropagationSpeed; }
        set { m_firePropagationSpeed = value; }
    }

    // Use this for initialization
    void Start () 
    {
        try
        {
            //FireManager'i taşıyan objeyi buluyor
            GameObject manager = GameObject.FindWithTag("Fire");

            if (manager != null)
            {
                FireManager fireManager = manager.GetComponent<FireManager>();
                if (fireManager != null)
                    m_combustionRateValue = fireManager.nodeCombustionRate;
            }
        }
        catch
        {
            // get the terrain from the fire manager
            FireManager fireManager = FindObjectOfType<FireManager>();
            if (fireManager != null)
                m_combustionRateValue = fireManager.nodeCombustionRate;

            Debug.LogWarning("No 'Fire' tag set, looking for Fire Manager.");
        }

        // fireNodes'teki tüm düğümlerin atandığını kontrol edilir
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i] == null)
            {
                Debug.LogError("Fire Node Chain on " + gameObject.GetComponentInParent<Transform>().name + " has missing Fire Nodes!");
                m_validChain = false;
                break;
            }
        }
    }

    void Update () {
        if (m_validChain)
        {
            PropagateFire();

            if (m_destroyAfterFire && !m_destroyedAlready)
                DestroyAfterFire();

            if (m_replaceAfterFire && !m_replacedAlready)
                ReplaceAfterFire();
        }
    }

    /// <summary>
    /// Brief, düğüm (node) konumlarında yangın particle system oluşturur
    /// </summary>
    void PropagateFire()
    {
        // Hangi düğümlerin şuanda yandığını, yanmadığını ve daha önce yanmış olup olmadıklarını temel alarak, düğümler üzerinde yangını yay
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].isAlight)
            {
                for (int child = 0; child < m_fireNodes[i].m_links.Count; child++)
                {
                    if (m_fireNodes[i].m_links[child].GetComponent<FireNode>().HP > 0.0f)
                    {
                        m_fireNodes[i].m_links[child].GetComponent<FireNode>().HP -= m_firePropagationSpeed * Time.deltaTime;
                    }
                }
            }

            m_fireNodes[i].ForceUpdate();
        }
    }

    // brief Find the closest node to the fire as set it alight
    // param Vector3 fire position
    public void StartFire(Vector3 firePoisition)
    {
        float shortestDistance = float.MaxValue;
        int shortestIndex = 0;
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            float distance = Vector3.Distance(m_fireNodes[i].GetComponent<Transform>().position, firePoisition);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                shortestIndex = i;
            }
        }

        m_fireNodes[shortestIndex].HP -= m_combustionRateValue * Time.deltaTime;
    }

    // brief Destroys the object once all FireNode's have run out of fuel
    void DestroyAfterFire()
    {
        bool allBurnt = false;

        // Check all nodes have had they fuel used up
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].m_fuel <= 0.0f)
            {
                // Got to the end, all have ran out of fuel
                if (i == m_fireNodes.Length - 1)
                    allBurnt = true;

                // Need to check next node
                continue;
            }
            else // Still have fuel
            {
                break;
            }
        }

        // If so, delete the gameoject
        if (allBurnt)
        {
            Destroy(gameObject);
            m_destroyedAlready = true;
        }
    }

    // brief Replaces the object with another once all FireNode's have run out of fuel
    void ReplaceAfterFire()
    {
        bool allBurnt = false;

        // Check all nodes have had they fuel used up
        for (int i = 0; i < m_fireNodes.Length; i++)
        {
            if (m_fireNodes[i].NodeConsumed() == true)
            {
                // Got to the end, all have ran out of fuel
                if (i == m_fireNodes.Length - 1)
                    allBurnt = true;

                // Need to check next node
                continue;
            }
            else
            {
                break;
            }
        }

        // If so, delete the gameoject and replace it
        if (allBurnt && m_replacementMesh != null)
        {
            if (m_replacementMesh != null)
            {
                Transform trans = gameObject.transform;
                Destroy(gameObject);
                Instantiate(m_replacementMesh, trans.position, trans.rotation);
            }
            else
            {
                Debug.Log("Failed to replace the gameobject");
            }


            m_replacedAlready = true;
        }
    }
}
