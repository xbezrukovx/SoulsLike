using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalScript : MonoBehaviour
{

    private volatile bool _isActive = false; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public bool IsActive()
    {
        return _isActive;
    }
}
