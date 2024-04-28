// Refactor 12.03.24

using System;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Enemy : DamagableObjectBase
{
    [SerializeField] private GameObject visual;
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player damageFeedback;
    [SerializeField] private MMF_Player beginningFeedback;
    [SerializeField] private MMF_Player skipBeginningFeedback;
    [SerializeField] private float durabilityMultiplier=50f;
    [SerializeField] private float massMultiplier = 1f;
    [SerializeField] private List<AudioClip> voiceClips;

    private AudioSource _audioSource;

    private bool _isDead;
    private Animator _animator;
    private void Awake()
    {
        _massMultiplier = massMultiplier;
        _durabilityMultiplier = durabilityMultiplier;
    }

    protected override void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        ControllerManager.action.InLevel.Attack.started += SkipBeginning;
        _animator = GetComponent<Animator>();
        base.Start();
        gameObject.tag = "Enemy";


    }
    private void OnCollisionEnter(Collision collision)
    {
        DoDamage(collision);
        var collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (collisionForce / _rigidbody.mass > 100f)
        {
            damageFeedback.PlayFeedbacks();
        }
        if (CheckDie()) 
        Die();
    }

    public void PlayVoiceClip(int index)
    {
        _audioSource.PlayOneShot(voiceClips[index]);
    }

    public void SkipBeginning(InputAction.CallbackContext context)
    {
        ControllerManager.action.InLevel.Attack.started -= SkipBeginning;
        beginningFeedback.StopFeedbacks();
        skipBeginningFeedback.PlayFeedbacks();

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
        Debug.LogWarning(gameObject.name + " is dead");
        _isDead = true;
        EventBus.Publish(EventType.EnemyDied);
        dieFeedback?.PlayFeedbacks(this.transform.position, 200);
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
