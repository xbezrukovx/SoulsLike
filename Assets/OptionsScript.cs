using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{

    void OnEnter()
    {
        var selector = gameObject.GetComponent<SimpleSelector>();

        Action action = selector.GetSelectedIndex() switch
        {
            1 => () => Debug.Log("Яркость"),
            2 => () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1)
        };

        action();
    }
    
}
