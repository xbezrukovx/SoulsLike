using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentScript : MonoBehaviour
{
    
    public GameObject[] slots;
    public GameObject selector;
    public GameObject monitor;
    
    private GameObject[][] matrix;
    private int _specialX = 1;
    private int _specialY = 1;
    private int _pointerX;
    private int _pointerY;
    
    private Gamepad _gamepad;
    private RectTransform _rectTransform;
    private ModalScript _modalScript;
    
    // Start is called before the first frame update
    void Start()
    {
        _gamepad = Gamepad.current;
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
        
        _rectTransform = selector.GetComponent<RectTransform>();
        MoveSelector(_pointerX, _pointerY);
    }

    void OnSelectRight()
    {
        if (_modalScript.IsActive()) return;
        if (_pointerX < matrix[_pointerY].Length - 1)
        {
            Debug.Log("Right");
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
        if (_modalScript.IsActive()) return;
        if (_pointerX > 0)
        {
            Debug.Log("Left");
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
        if (_modalScript.IsActive()) return;
        if (_pointerY > 0)
        {
            Debug.Log("Up");
            _pointerY--;
            MoveSelector(_pointerX, _pointerY);
        }
    }
    
    void OnSelectDown()
    {
        if (_modalScript.IsActive()) return;
        if (_pointerY < matrix.Length - 1)
        {
            Debug.Log("Down");
            _pointerY++;
            if (_pointerX == _specialX && _pointerY == _specialY)
            {
                _pointerX--;    
            }
            MoveSelector(_pointerX, _pointerY);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    private void MoveSelector(int x, int y)
    {
        Debug.Log("x:" + x + "y:" + y);
        
        Vector3 endPos = matrix[y][x].transform.position;
        // Устанавливаем точное конечное положение
        _rectTransform.position = endPos;
    }
}
