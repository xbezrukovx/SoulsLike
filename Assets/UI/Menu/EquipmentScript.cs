using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UI.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentScript : MonoBehaviour, PostInit
{
    
    public GameObject[] slots;
    public GameObject selector;
    public GameObject monitor;
    public GameObject modal;
    
    private GameObject[][] matrix;
    private int _specialX = 1;
    private int _specialY = 1;
    private int _pointerX;
    private int _pointerY;
    
    private RectTransform _rectTransform;
    private ModalScript _modalScript;
    
    // Start is called before the first frame update
    void Awake()
    {
        modal.SetActive(false);
        _modalScript = monitor.GetComponent<ModalScript>();
        
        matrix = new GameObject[2][];
        matrix[0] = new GameObject[4];
        matrix[1] = new GameObject[3];
        matrix[0][0] = slots[0];
        matrix[0][1] = slots[1];
        matrix[0][2] = slots[2];
        matrix[0][3] = slots[3];
        matrix[1][0] = slots[4];
        matrix[1][2] = slots[5];
    }

    public void PostInit()
    {
        _rectTransform = selector.GetComponent<RectTransform>();
        MoveSelector(_pointerX, _pointerY);
    }


    void OnSelectRight()
    {
        if (monitor.GetComponent<ModalScript>().IsActive()) return;
        if (_pointerX < matrix[_pointerY].Length - 1)
        {
            _pointerX++;
            if (_pointerX == _specialX && _pointerY == _specialY)
            {
                _pointerX++;    
            }
            MoveSelector(_pointerX, _pointerY);
        }
    }
    
    void OnSelectLeft()
    {
        if (monitor.GetComponent<ModalScript>().IsActive()) return;
        if (_pointerX > 0)
        {
            _pointerX--;
            if (_pointerX == _specialX && _pointerY == _specialY)
            {
                _pointerX--;    
            }
            MoveSelector(_pointerX, _pointerY);
        }
    }
    
    void OnSelectUp()
    {
        if (monitor.GetComponent<ModalScript>().IsActive()) return;
        if (_pointerY > 0)
        {
            _pointerY--;
            MoveSelector(_pointerX, _pointerY);
        }
    }
    
    void OnSelectDown()
    {
        if (monitor.GetComponent<ModalScript>().IsActive()) return;
        if (_pointerY < matrix.Length - 1)
        {
            _pointerY++;
            if (_pointerX == _specialX && _pointerY == _specialY)
            {
                _pointerX--;    
            }
            MoveSelector(_pointerX, _pointerY);
        }
    }

    void OnEnter()
    {
        if (_modalScript.IsActive()) return;
        modal.SetActive(true);
        _modalScript.Activate();
    }

    private void MoveSelector(int x, int y)
    {
        Vector3 endPos = matrix[y][x].transform.position;
        _rectTransform.position = endPos;
    }
}
