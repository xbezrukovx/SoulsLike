using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    private Animator _animator;

    private bool _isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTargetLock()
    {
        if (_isLocked)
        {
            _isLocked = false;
            FollowCamera();
        }
        else
        {
            _isLocked = true;
            TargetCamera();
        }
    }

    private void FollowCamera()
    {
        _animator.Play("FollowCam");
    }
    
    private void TargetCamera()
    {
        _animator.Play("TargetCam");
    }
}
