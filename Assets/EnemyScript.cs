using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float _range = 100f; 
    
    private int _maxHealth;
    private Animator _animator;
    private HealthBarScript _healthBar;
    private NavMeshAgent _agent;
    private GameObject _player;
    
    public int health = 100;
    
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _maxHealth = health;
        _animator = GetComponent<Animator>();
        _healthBar = GetComponentInChildren<HealthBarScript>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) return;
        
        var source = transform.position;
        var destination = _player.transform.position;
        var distance = Vector3.Distance(source, destination);
        
        _animator.SetFloat("speed", _agent.velocity.magnitude / _agent.speed);

        if (distance <= _range && health > 0)
        {
            var look = _player.transform.position;
            look.y = transform.position.y;
            GetComponent<Rigidbody>().transform.LookAt(look);
        }

        if (distance >= 2.5f && distance <= _range && health > 0)
        {
            _animator.SetBool("isCombat", true);
            _agent.destination = destination;
        }
        else
        {
            _animator.SetFloat("speed", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon")) TakeDamage(other);
    }

    private void TakeDamage(Collider other)
    {
        if (health <= 0) return;
        
        _animator.SetBool("isCombat", true);
        _animator.SetTrigger("TakeDamage");
        var damage = other.GetComponent<WeaponStat>().Damage;
        health -= damage;
        var impact = (float) damage / _maxHealth;
        _healthBar.GetDamageToBar(impact);

        if (health <= 0)
        {
            Die();
            var targetScript = GameObject.Find("State-Driven Camera").GetComponent<TargetScript>();
            targetScript.FindAnotherTarget(this);
        }
    }

    void Die()
    {
        _healthBar.gameObject.SetActive(false);
        _animator.SetTrigger("Die");
    }

    private void FollowToTarget()
    {
        
    }
}
