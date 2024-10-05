using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool _isActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        OnOpenMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnOpenMenu()
    {
        if (!_isActive)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
        var transforms = gameObject.GetComponent<Transform>().transform.Cast<Transform>();
        var iterator = transforms.GetEnumerator();

        while (iterator.MoveNext())
        {
            iterator.Current.gameObject.SetActive(_isActive);
        }
    }
}
