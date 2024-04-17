using UnityEngine;
using System.Collections.Generic;

public class FireNodeChain : MonoBehaviour {
    [Tooltip("Yangın yayılım hızı")]
    public float firePropagationSpeed = 20.0f;
    [SerializeField] private GameObject[] _fireFaces;
    [Tooltip("Bu zincirdeki düğümleri temsil etmektedir. Not: yangınların doğru şekilde başlaması için tüm düğümlere sahip olmalıdır")]
    public FireNode[] fireNodes = null;
    [Tooltip("Tüm düğümler ateşe verildikten sonra GameObject'in yok edilmesi gerekiyorsa etkinleştirin")]
    public bool destroyAfterFire = false;
    [Tooltip("Tüm düğümler ateşlendikten sonra GameObject'in başka bir mesh ile değiştirilmesi gerekiyorsa etkinleştirin")]
    public bool replaceAfterFire = false;
    [Tooltip("Bu nesnenin değiştirilmesi gereken GameObject")]
    public GameObject replacementMesh;
    /// <summary>
    /// zaten destroy edildi mi?
    /// </summary>
    private bool destroyedAlready = false;
    /// <summary>
    /// zaten mesh replace edildi mi?
    /// </summary>
    private bool replacedAlready = false;
    /// <summary>
    /// fireNodes'in üzerindeki tüm değerler atanmışsa true döndürür
    /// </summary>
    private bool validChain = true;
    /// <summary>
    /// Zincirdeki tüm FireNode'lar yandı mı?
    /// </summary>
    private bool _chainBurned = false;


    void Start () {

        AssignFireNodes();
        CheckFireNodes();
    }

    void Update () {
        if (validChain)
        {
            PropagateFire();
            if(destroyAfterFire || replaceAfterFire)
            {
                CheckChainBurn();

                if (!replacedAlready)
                    ReplaceAfterFire();
                if (!destroyedAlready)
                    DestroyAfterFire();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // çarpan obje mühimmatsa ve maddesi ateşse
        if(collision.transform.CompareTag("Ammo") && collision.gameObject.GetComponent<Ammo>()?.matter==AmmoMatter.Fire)
        {
            Vector3 differentPos = Vector3.up * 100;
            int index = 100;
            
            for (int i = 0; i < fireNodes.Length; i++)
            {
                if (!fireNodes[i].isAlight)
                {
                    var tempDifPos = collision.transform.position - fireNodes[i].transform.position;
                    if(tempDifPos.magnitude<differentPos.magnitude)
                    {
                        differentPos = tempDifPos;
                        index = i;
                    }
                }
            }
            fireNodes[index].fireJustStarted=true;
        }
    }

    /// <summary>
    /// _fireFaces altındaki düğümlerin fireNode'larını fireNodes'e ekler
    /// </summary>
    void AssignFireNodes()
    {
        List<FireNode> tempList = new List<FireNode>();
        foreach (var fireFace in _fireFaces)
        {
            for (int i = 0; i < fireFace.transform.childCount; i++)
            {
                tempList.Add(fireFace.transform.GetChild(i).GetComponent<FireNode>());
            }
        }
        foreach (var fireNode in fireNodes)
        {
            tempList.Add(fireNode);
        }

        fireNodes = tempList.ToArray();
    }
    /// <summary>
    /// fireNodes'teki tüm düğümlerin atandığını kontrol edilir
    /// </summary>
    void CheckFireNodes()
    {
        for (int i = 0; i < fireNodes.Length; i++)
        {
            if (fireNodes[i] == null)
            {
                Debug.LogError(gameObject.GetComponentInParent<Transform>().name + " isimli objede FireNodeChain bileşeni üzerinde " +
                    "bazı FireNode'lar atanmamış");
                validChain = false;
                break;
            }
            else
            {
                validChain = true;
            }
        }
    }

    /// <summary>
    /// Düğüm (node) konumlarında yangın particle system oluşturur
    /// </summary>
    void PropagateFire()
    {
        // Hangi düğümlerin şuanda yandığını, yanmadığını ve daha önce yanmış olup olmadıklarını temel alarak, düğümler üzerinde yangını yay
        for (int i = 0; i < fireNodes.Length; i++)
        {
            if (fireNodes[i].isAlight)
            {
                for (int child = 0; child < fireNodes[i].links.Count; child++)
                {
                    if (fireNodes[i].links[child] != null && fireNodes[i].links[child].GetComponent<FireNode>().hp > 0.0f)
                    {
                        fireNodes[i].links[child].GetComponent<FireNode>().hp -= firePropagationSpeed * Time.deltaTime;
                    }
                }
            }

            fireNodes[i].ForceUpdate();
        }
    }

    /// <summary>
    /// Tüm FireNode'ların yanıp yanmadığını kontrol eder
    /// </summary>
    /// <returns>Tüm FireNode'lar yandıysa true döndürür</returns>
    bool CheckChainBurn()
    {
        for (int i = 0; i < fireNodes.Length; i++)
        {
            if (fireNodes[i].fuel > 0.0f)
            {
                _chainBurned = false;
                return false;
            }
        }
        _chainBurned = true;
        return true;
    }

    /// <summary>
    /// Tüm FireNode'ların yakıtı bittiğinde nesneyi yok eder
    /// </summary>
    void DestroyAfterFire()
    {
        if (_chainBurned)
        {
            Destroy(gameObject);
            destroyedAlready = true;
        }
    }

    /// <summary>
    /// Tüm FireNode'ların yakıtı bittiğinde nesneyi bir başkasıyla değiştirir
    /// </summary>
    void ReplaceAfterFire()
    {
        if (_chainBurned && replacementMesh != null)
        {
            if (replacementMesh != null)
            {
                Transform trans = gameObject.transform;
                Destroy(gameObject);
                Instantiate(replacementMesh, trans.position, trans.rotation);
            }
            else
            {
                Debug.LogWarning("Yanmış objenin meshi değiştirilemedi");
            }
            replacedAlready = true;
        }
    }
}
