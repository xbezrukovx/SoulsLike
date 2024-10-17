using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListnerScript : MonoBehaviour
{
    
    private CharacterMovement _characterMovement;
    private bool _isRolling;
    private Animator _animator;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRollingStart()
    {
        _characterMovement.DisableMovement();
        _isRolling = true;
    }

    public void OnRollingContinue()
    {
        _isRolling = false;
        _characterMovement.EnableMovement();
    }

    public void OnRollingStop()
    {
    }
    
    void OnBack()
    {
        if (_isRolling) return;
        _animator.SetTrigger("Roll");
    }

}
