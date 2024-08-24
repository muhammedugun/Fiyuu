// Refactor 23.08.24
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

/// <summary>
/// Düþmanlar için ana sýnýf
/// </summary>
public class Enemy : DamagableObjectBase
{
    public float durabilityMultiplier = 50f;
    public float massMultiplier = 1f;

    [SerializeField] private GameObject _visual;
    [SerializeField] private MMF_Player _dieFeedback;
    [SerializeField] private MMF_Player _damageFeedback;
    [SerializeField] private MMF_Player _skipBeginningFeedback;
    [SerializeField] private List<AudioClip> _voiceClips;

    private AudioSource _audioSource;
    private bool _isDead;
    private Animator _animator;

    private void Awake()
    {
        base._massMultiplier = massMultiplier;
        base._durabilityMultiplier = durabilityMultiplier;
    }

    protected override void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _animator = GetComponent<Animator>();
        base.Start();
        gameObject.tag = "Enemy";
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (durability > 0 && collisionForce > damageSensitivity)
        {
            _damageFeedback.PlayFeedbacks();
        }

        DoDamage(collision);

        if (CheckDie())
            Die();
    }

    public override void DoDamage(Collision collision, float damageMultiplier = 1f)
    {
        Debug.Log("mainName: " + name + " colname: " + collision.gameObject.name);
        Debug.Log("mainName: " + name + " collisionForce: " + (collision.impulse.magnitude / Time.fixedDeltaTime));
        base.DoDamage(collision, damageMultiplier);
    }

    /// <summary>
    /// Konuþma klibini oynat
    /// </summary>
    /// <param name="index"></param>
    public void PlayVoiceClip(int index)
    {
        _audioSource.PlayOneShot(_voiceClips[index]);
    }

    /// <summary>
    /// Baþlangýç feedback'lerini atla
    /// </summary>
    public void SkipBeginning()
    {
        _skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    public void SetEnableAnimator()
    {
        _animator.enabled = true;
    }

    /// <summary>
    /// Ölmeyi gerçekleþtir
    /// </summary>
    /// <param name="smashableObject">Objenin parçalanabilir halinin örneði</param>
    private void Die()
    {
        _isDead = true;
        EventBus.Publish(EventType.EnemyDied);
        _dieFeedback?.PlayFeedbacks(this.transform.position, ScoreManager.enemyScore);
    }

    /// <summary>
    /// Ölünebilir mi? diye kontrol et
    /// </summary>
    /// <param name="collision"></param>
    private bool CheckDie()
    {
        if (!_isDead && durability <= 0) return true;
        else return false;
    }


}
