using System;
using TMPro;
using UnityEngine;

public class SoulsCointer : MonoBehaviour
{
    private int _counter;
    public TextMeshProUGUI text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddCount(100);
        }
    }

    public int GetCount()
    {
        return _counter;
    }

    public void AddCount(int value)
    {
        _counter += value;
        text.text = _counter.ToString();
    }
}
