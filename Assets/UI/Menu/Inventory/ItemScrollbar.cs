using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class ItemScrollbar : MonoBehaviour
{
    public GameObject contentView;  // Контейнер для элементов инвентаря
    public GameObject emptyPrefab;
    private int _itemCount;      // Общее количество элементов в инвентаре
    public GameObject monitor;
    public GameObject modalWindow;
    public bool isIgnoreMutex;
    public GameObject parent;
    
    private RectTransform _contentRect;
    private float _itemHeight;      // Высота одного элемента
    private float _contentHeight;   // Высота всего контента
    private float _hiddenHeight;
    private int _selectedIndex = 1; // Индекс выбранного элемента

    private float _visibleSpace = 0;
    private float _minVisibleSpace = 0;
    private float _maxVisibleSpace = 0;
    private float _currentHeight = 0;
    private List<Image> _items;
    
    void Start()
    {
        if (modalWindow != null)
        {
            modalWindow.SetActive(false);
        }
        
        _contentRect = contentView.GetComponent<RectTransform>();
        _items = contentView.GetComponentInChildren<Transform>(true)
            .transform.Cast<Transform>()  // Преобразуем Transform в IEnumerable
            .Select(t =>
            {
                var color = Color.black;
                color.a = 0;
                var image = t.AddComponent<Image>();
                image.color = color;
                return image;
            })    // Получаем GameObject
            .ToList();
        _itemCount = _contentRect.childCount;

        if (_itemCount != 0)
        {
            var color = Color.black;
            color.a = 0.5f;
            _items[_selectedIndex - 1].color = color;
        }
        else
        {
            Instantiate(emptyPrefab, contentView.transform);
        }
    }

    public void OnSelectDown()
    {
        if (monitor.GetComponent<ModalScript>().IsActive() && !isIgnoreMutex) return;
        if (_selectedIndex <  contentView.GetComponent<RectTransform>().childCount)  // Если индекс меньше количества элементов
        {
            _selectedIndex++;  // Переходим к следующему элементу
            Move();
        }
    }

    public void OnSelectUp()
    {
        if (monitor.GetComponent<ModalScript>().IsActive() && !isIgnoreMutex) return;
        if (_selectedIndex > 1)  // Если индекс больше 0
        {
            _selectedIndex--;  // Переходим к предыдущему элементу
            Move();
        }
    }

    void OnEnter()
    {
        if (modalWindow == null) return;
        monitor.GetComponent<ModalScript>().Activate();
        modalWindow.SetActive(true);
        modalWindow.GetComponent<ModalSelectorScript>().IndexItem = _selectedIndex - 1;
    }

    void OnBack()
    {
        if (modalWindow != null) return;
        monitor.GetComponent<ModalScript>().Deactivate();
        parent.SetActive(false);
    }

    private void Move()
    {
        _visibleSpace = GetComponent<Scrollbar>().GetComponent<RectTransform>().rect.height;
        _contentHeight = contentView.GetComponent<RectTransform>().rect.height;
        _itemHeight = _contentHeight / _itemCount;
        if (_maxVisibleSpace == 0)
        {
            _maxVisibleSpace = _visibleSpace;
            _hiddenHeight = _contentHeight - _visibleSpace;
        }
        
        _currentHeight = _selectedIndex * _itemHeight;
        
        _items.ForEach(i =>
        {
            var color = Color.black;
            color.a = 0;
            i.color = color;
        });
        
        var color = Color.black;
        color.a = 0.5f;
        _items[_selectedIndex - 1].color = color;
        
        var downDiff = _currentHeight - _maxVisibleSpace;
        var upDiff = _minVisibleSpace - _currentHeight + _itemHeight;

        if (downDiff > 0)
        {
            var percent = 1 - (_currentHeight - _visibleSpace) / _hiddenHeight;
            GetComponent<Scrollbar>().value = percent;
            _minVisibleSpace += downDiff;
            _maxVisibleSpace += downDiff;
            return;
        }
        
        if (upDiff > 0)
        {
            var percent = 1 - (_currentHeight - _itemHeight) / _hiddenHeight;
            GetComponent<Scrollbar>().value = percent;
            _minVisibleSpace -= upDiff;
            _maxVisibleSpace -= upDiff;
        }
        
    }
    
}
