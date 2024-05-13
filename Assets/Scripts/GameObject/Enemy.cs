// Refactor 12.03.24

using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
public class Enemy : DamagableObjectBase
{
    [SerializeField] private GameObject visual;
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player damageFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;
    [SerializeField] private float durabilityMultiplier=50f;
    [SerializeField] private float massMultiplier = 1f;
    [SerializeField] private List<AudioClip> voiceClips;


    private AudioSource _audioSource;

    private bool _isDead;
    private Animator _animator;
    private bool _isDamageble;
    private void Awake()
    {
        _massMultiplier = massMultiplier;
        _durabilityMultiplier = durabilityMultiplier;
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

        if(_isDamageble)
        {
            var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
            if (durability > 0 && collisionForce > damageSensitivity)
            {
                damageFeedback.PlayFeedbacks();
            }

            DoDamage(collision);

            if (CheckDie())
                Die();
        }

    }

    public void PlayVoiceClip(int index)
    {
        _audioSource.PlayOneShot(voiceClips[index]);
    }

    public void SkipBeginning()
    {
        skipBeginningFeedback.PlayFeedbacks();
        EventBus.Unsubscribe(EventType.FirstClickInLevel, SkipBeginning);
    }

    public void SetIsDamageble()
    {
        _isDamageble = true;
    }


    public void SetIsDamagebleInvoke()
    {
        Invoke(nameof(SetIsDamageble), 0.5f);
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
        dieFeedback?.PlayFeedbacks(this.transform.position, ScoreManager.enemyScore);
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
