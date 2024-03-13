// Refactor 12.03.24
using MoreMountains.Feedbacks;
using System;
using UnityEngine;

/// <summary>
/// Yap�(Bina) objelerini temsil eder
/// </summary>
public class Building : SmashableObjectBase
{
    /// <summary>
    /// Yap� par�aland� eventi. 
    /// <para> float: yap�n�n hacmi. BuildingMatter: yap�n�n z�rh�. </para>
    /// </summary>
    public static event Action<float, BuildingMatter> OnBuildSmashed;
    [Tooltip("Yap�n�n z�rh�")]
    public BuildingMatter armor;

    [SerializeField] private MMF_Player damageFeedbacks, beginningFeedbacks;

    /// <summary>
    /// Yap� z�rh�na g�re dayan�kl�l�k de�erlerini saklayan dizi. 
    /// <para>  �rnek: 0. index 1. yap� maddesi olan ah�apa denk gelir. </para>
    /// </summary>
    internal float[] armorDurabilitiy = new float[4] { 100f, 200f, 600f, 800f };


    /// <summary>
    /// Z�rh�n g��l� y�nleri. Z�rh�n neye dayan�kl� olup neye dayan�kl� olmad���. 
    /// <para> Sat�rlar: Bina maddesi(z�rh�), S�tunlar: m�himmat maddesi </para>
    /// </summary>
    private int[,] _armorStrengths = new int[4, 8]
    {
        // M�himmat maddesi t�rleri (s�tunlar).
        // Ah�ap=1, ta�=2, demir=3, �elik=4, ate�=5, buz=6, patlama=7, elektrik=8
        // Bina maddesi t�rleri (sat�rlar)
        { 0,0,0,0,0,0,0,1 }, // Ah�ap
        { 1,0,0,0,1,0,0,1 }, // Ta�
        { 1,1,0,0,1,0,0,0 }, // Demir
        { 1,1,1,1,1,1,1,0 }  // �elik
    };


    private void Awake()
    {
        _massMultiplier = (int)armor;
        _durabilityMultiplier = armorDurabilitiy[(int)armor - 1];
    }

    protected override void Start()
    {
        base.Start();
        beginningFeedbacks?.PlayFeedbacks();
    }

    private void OnCollisionEnter(Collision collision)
    {

        DoDamage(collision);
        if (CheckSmash())
        {
            Smash(collision);
        }
  
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (durability > 0 && collisionForce / _rigidbody.mass > 100f)
        {
            if (collision.transform.CompareTag("Ammo"))
            {
                damageFeedbacks?.PlayFeedbacks();
                var ammo = collision.gameObject.GetComponent<Ammo>();
                var ammoArmor = ammo.matter;

                if (_armorStrengths[(int)armor - 1, (int)ammoArmor - 1] == 0)
                {
                    base.DoDamage(collision);
                }
            }
            else if (collision.gameObject.layer != LayerMask.NameToLayer("Node"))
            {
                base.DoDamage(collision);
            }
        }
    }

    public override void Smash(Collision collision)
    {
        base.Smash(collision);
        OnBuildSmashed?.Invoke(_volumeSize, armor);
    }
}



/// <summary>
/// Yap�n�n maddesi(z�rh�) 
/// </summary>
public enum BuildingMatter
{
    Wood = 1,
    Stone,
    Iron,
    Steel
}