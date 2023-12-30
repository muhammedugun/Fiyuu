using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Ateş Düğümü: Düğümlerin olduğu yer yanabilir ve etrafında düğümler varsa onlarında yanmasına sebep olabilir
/// </summary>
public class FireNode : MonoBehaviour
{
    [Tooltip("Yangında kullanılacak prefab.")]
    public GameObject fire;
    [Tooltip("Bu düğümle bağlantılı diğer düğümleri içerir. Bu düğüm ateşlendiğinde bağlantılı düğümler ısınmaya başlayacaktır. " +
        "\nNot: Eklediğiniz objelerde FireNode olmasına dikkat edin")]
    public List<GameObject> links = null;
    [Tooltip("Can puanı ne kadar yüksek olursa, hücrenin ısınması ve tutuşması o kadar geç olur")]
    public float hp = 10.0f;
    [Tooltip("Yakıt miktarı, yanma süresini belirler")]
    public float fuel = 15.0f;
    /// <summary>
    /// Ateşi söndürme eşiği
    /// </summary>
    private float extinguishThreshold;
    /// <summary>
    /// Yanma hızı
    /// </summary>
    [SerializeField] private float combustionRate=1.0f;
    /// <summary>
    /// Yangın yeni(şimdi) başladı. <br> Not: Yangını başlatan değişken budur. </br>
    /// </summary>
    [SerializeField] public bool fireJustStarted = false;
    /// <summary>
    /// Yanıyor mu?
    /// </summary>
    public bool isAlight = false;
    /// <summary>
    /// Yangın sönmüş
    /// </summary>
    private bool extingushed = false;
    private FireVisualManager visualMgr = null;

    void Start()
    {
        ConnectLink();
    }

    /// <summary>
    /// Node'ları reverse şeklinde birbirine bağlar
    /// </summary>
    void ConnectLink()
    {
        var nodeInSphere = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Node"));
        bool isthere = false;
        foreach (var node in nodeInSphere)
        {
            if(node.gameObject!=gameObject)
            {
                isthere = false;
                foreach (var link in links)
                {
                    if (node == link)
                    {
                        isthere = true;
                        return;
                    }
                }
                if (!isthere)
                    links.Add(node.gameObject);
            }
        }
    }

    // brief Kills the attached child particle systems
    private void KillFlames()
    {
        Destroy(fire);
    }


    void Update()
    {
        // can sıfır ya da daha azsa ve yanmıyorsam yangını başlat
        if (hp <= 0.0f && !isAlight)
            fireJustStarted = true;

        Ingition();
        Combustion();
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
    /// <param name="fire">Ateş objesi</param>
    public void InstantiateFire(Vector3 position, GameObject fire)
    {
        if (fireJustStarted)
        {
            this.fire = Instantiate(fire, position, new Quaternion());

            // Should be set after fire extinguished
            isAlight = true;
        }
    }

    /// <summary>
    /// Ateşlemeyi başlatır yani node içindeki yakıtın yanmasını sağlar. 
    /// </summary>
    void Ingition()
    {
        if (fireJustStarted && !extingushed)
        {
            InstantiateFire(transform.position, fire);
            fireJustStarted = false;

            GetVisualManager();

            if(visualMgr != null)
                visualMgr.SetIgnitionState();
        }
    }

    /// <summary>
    /// Yangının alevlenmesinden sonra tetiklenen yanma adımı
    /// </summary>
    void Combustion()
    {
        if (isAlight)
        {
            fireJustStarted = false;

            fuel -= combustionRate * Time.deltaTime;

            if (fuel < extinguishThreshold)
            {
                // Bu işlev çağrılmadan önce Ignition(ateşleme)'da çağrılan getFireManager() kadar geçerli olmalıdır
                if (visualMgr != null)
                    visualMgr.SetExtingushState();
            }

            if (fuel <= 0.0f)
            {
                isAlight = false;
                extingushed = true;

                KillFlames();
            }
        }
    }

    // VisualManager'i alır ve ona bir referans ayarlar
    void GetVisualManager()
    {
        if (visualMgr == null)
            visualMgr = fire.GetComponent<FireVisualManager>();
    }
}
