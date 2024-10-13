using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject rightSocket;
    public GameObject defaultWeapon;
        
    private GameObject _weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack()
    {
        if (_weapon == null)
        {
            SetWeapon(defaultWeapon);   
        }
        
        var animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Slash");
    }

    public void OnAttackEvent()
    {
    }

    public void SetWeapon(GameObject gameObject)
    {
        foreach (GameObject child in rightSocket.transform)
        {
            Destroy(child);
        }
        _weapon = Instantiate(gameObject, rightSocket.transform);
        _weapon.tag = "Player";
        var collider = _weapon.GetComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
    }
}
