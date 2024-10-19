using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float chasingRange = 10f;
    [SerializeField] private float agroRange = 5f;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isDebug = false;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private Animator _animator;
    private HealthBarScript _healthBar;
    private NavMeshAgent _agent;
    private GameObject _player;

    private Vector3 _awakePosition;
    private Quaternion _awakeRotation;
    
    private int _maxHealth;
    private bool _isAggressive;
    private bool _isMovementBlocked;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _maxHealth = health;
        _animator = GetComponent<Animator>();
        _healthBar = GetComponentInChildren<HealthBarScript>();
        _awakePosition = gameObject.transform.position;
        _awakeRotation = gameObject.transform.rotation;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) return;
        if (Input.GetKeyDown(KeyCode.Space)) Die();
        
        var source = transform.position;
        var destination = _player.transform.position;
        var distance = Vector3.Distance(source, destination);
        
        _animator.SetFloat("speed", _agent.velocity.magnitude / _agent.speed);
        if (distance < agroRange)
        {
            _isAggressive = true;
        }

        if (distance > chasingRange && !_isMovementBlocked)
        {
            _isAggressive = false;
            _agent.destination = _awakePosition;
        }
        
        if (_isAggressive && !_isMovementBlocked)
        {
            _agent.destination = destination;
            var look = _player.transform.position;
            look.y = transform.position.y;
            var targetRotation = Quaternion.LookRotation(look - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isMovementBlocked && _isAggressive)
        {
            _agent.destination = source;
        }

        if (_isAggressive)
        {
            _animator.SetBool("isCombat", true);
        }

        // Атакуем 
        Vector3 directionToTarget = destination - transform.position;
        var angle = Vector3.Angle(transform.forward, directionToTarget);
        if (_isAggressive && _agent.velocity.magnitude <= 0.1f && distance < attackRange && health > 0 && angle < 55 && !_isMovementBlocked)
        {
            var look = _player.transform.position;
            look.y = transform.position.y;
            GetComponent<Rigidbody>().transform.LookAt(look);
            _animator.SetTrigger("Slash");
        }
        
        var distanceToAwake = Vector3.Distance(source, _awakePosition);
        
        if (_agent.velocity.magnitude <= 0f && !_isAggressive && distanceToAwake < 2f && !_isMovementBlocked)
        {
            _animator.SetBool("isCombat", false);
            transform.rotation = _awakeRotation;
        }
    }

    public void OnAttackStart()
    {
        _isMovementBlocked = true;
    }

    public void OnAttackEnd()
    {
        _isMovementBlocked = false;
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
        health = 0;
        _healthBar.gameObject.SetActive(false);
        _animator.enabled = false;
    }

    public int Health
    {
        get { return health; }
    }
}
