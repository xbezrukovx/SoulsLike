using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScrollbar : MonoBehaviour
{
    public GameObject contentView;  // Контейнер для элементов инвентаря
    private int _itemCount;      // Общее количество элементов в инвентаре
    public GameObject cursor;
    
    private Scrollbar _scrollbar;
    private RectTransform _contentRect;
    private float _itemHeight;      // Высота одного элемента
    private float _contentHeight;   // Высота всего контента
    private float _hiddenHeight;
    private int _selectedIndex = 1; // Индекс выбранного элемента

    private float _visibleSpace = 0;
    private float _minVisibleSpace = 0;
    private float _maxVisibleSpace = 0;
    private float _currentHeight = 0;
    
    void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
        _contentRect = contentView.GetComponent<RectTransform>();
        _itemCount = _contentRect.childCount;
    }

    public void OnSelectDown()
    {
        if (_selectedIndex < _itemCount)  // Если индекс меньше количества элементов
        {
            _selectedIndex++;  // Переходим к следующему элементу
            Move();
        }
    }

    public void OnSelectUp()
    {
        if (_selectedIndex > 1)  // Если индекс больше 0
        {
            _selectedIndex--;  // Переходим к предыдущему элементу
            Move();
        }
    }

    private void Move()
    {
        _visibleSpace = _scrollbar.GetComponent<RectTransform>().rect.height;
        _contentHeight = contentView.GetComponent<RectTransform>().rect.height;
        _itemHeight = _contentHeight / _itemCount;
        if (_maxVisibleSpace == 0)
        {
            _maxVisibleSpace = _visibleSpace;
            _hiddenHeight = _contentHeight - _visibleSpace;
        }
        
        _currentHeight = _selectedIndex * _itemHeight;
        
        var downDiff = _currentHeight - _maxVisibleSpace;
        var upDiff = _minVisibleSpace - _currentHeight + _itemHeight;

        if (downDiff > 0)
        {
            var percent = 1 - (_currentHeight - _visibleSpace) / _hiddenHeight;
            _scrollbar.value = percent;
            _minVisibleSpace += downDiff;
            _maxVisibleSpace += downDiff;
            return;
        }
        
        if (upDiff > 0)
        {
            var percent = 1 - (_currentHeight - _itemHeight) / _hiddenHeight;
            _scrollbar.value = percent;
            _minVisibleSpace -= upDiff;
            _maxVisibleSpace -= upDiff;
        }
        
    }
    
}
