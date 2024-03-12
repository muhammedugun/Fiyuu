// Refactor 12.03.24
using MoreMountains.Feedbacks;
using System;
using UnityEngine;

/// <summary>
/// Yapý(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    /// <summary>
    /// Yapý parçalandý eventi. 
    /// <para> float: yapýnýn hacmi. BuildingMatter: yapýnýn zýrhý. </para>
    /// </summary>
    public static event Action<float, BuildingMatter> OnBuildSmashed;
    [Tooltip("Yapýnýn zýrhý")]
    public BuildingMatter armor;

    [SerializeField] private MMF_Player damageFeedbacks, beginningFeedbacks;

    /// <summary>
    /// Yapý zýrhýna göre dayanýklýlýk deðerlerini saklayan dizi. 
    /// <para>  Örnek: 0. index 1. yapý maddesi olan ahþapa denk gelir. </para>
    /// </summary>
    internal float[] armorDurabilitiy = new float[4] { 10000f, 40000f, 60000f, 80000f };


    /// <summary>
    /// Zýrhýn güçlü yönleri. Zýrhýn neye dayanýklý olup neye dayanýklý olmadýðý. 
    /// <para> Satýrlar: Bina maddesi(zýrhý), Sütunlar: mühimmat maddesi </para>
    /// </summary>
    private int[,] _armorStrengths = new int[4, 8]
    {
        // Mühimmat maddesi türleri (sütunlar).
        // Ahþap=1, taþ=2, demir=3, çelik=4, ateþ=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi türleri (satýrlar)
        { 0,0,0,0,0,0,0,1 }, // Ahþap
        { 1,0,0,0,1,0,0,1 }, // Taþ
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // Çelik
    };


    private void Awake()
    {
        _massMultiplier= (int)armor;
    }

    protected override void Start()
    {
        base.Start();
        AssignDurability();
        beginningFeedbacks?.PlayFeedbacks();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isSmash)
        {
            DoDamage(collision);
            if (CheckSmash())
            {
                _isSmash = true;
                Smash(collision);
            }
        }
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        if (collision.transform.CompareTag("Ammo"))
        {
            damageFeedbacks?.PlayFeedbacks();
            var ammo = collision.gameObject.GetComponent<Ammo>();
            var ammoArmor = ammo.matter;
            
            if (_armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
            {
                base.DoDamage(collision, ammo.power[(int)ammoArmor - 1]);
            }
        }
        else if(collision.gameObject.layer!=LayerMask.NameToLayer("Node"))
        {
            base.DoDamage(collision);
        }
    }



    protected override void AssignDurability(float durabilityMultiplier=1f)
    {
        base.AssignDurability(armorDurabilitiy[(int)armor - 1]);
    }

    public override void Smash(Collision collision)
    {
        base.Smash(collision);
        OnBuildSmashed?.Invoke(_volumeSize, armor);
    }
}



/// <summary>
/// Yapýnýn maddesi(zýrhý) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}