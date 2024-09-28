using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModalSelectorScript : MonoBehaviour
{
    public GameObject contentView;  // Контейнер для элементов инвентаря
    public GameObject monitor;
    public Color selectedColor;
    
    private int _itemCount;
    private int _selectedIndex = 1; // Индекс выбранного элемента
    private ModalScript _modalScript;
    private List<Image> _items;

    // Start is called before the first frame update
    void Start()
    {
        var rect = contentView.GetComponent<Transform>();
        _modalScript = monitor.GetComponent<ModalScript>();
        _itemCount = rect.childCount;
        
        _items = rect
                .transform.Cast<Transform>()  // Преобразуем Transform в IEnumerable
                .Select(t =>
                {
                    selectedColor.a = 0;
                    var image = t.AddComponent<Image>();
                    image.color = selectedColor;
                    return image;
                })    // Получаем GameObject
                .ToList();
        
        if (_itemCount != 0)
        {
            var color = selectedColor;
            color.a = 0.5f;
            _items[_selectedIndex - 1].color = color;
        }
    }

    private void OnEnable()
    {
        _modalScript.Activate();
    }

    void OnBack()
    {
        _modalScript.Deactivate();
        gameObject.SetActive(false);
    }

    void OnEnter()
    {
        _modalScript.Deactivate();
        Debug.Log("Выбранное действие: " + _selectedIndex);
        Debug.Log("Выбранный предмет: " + IndexItem);
        gameObject.SetActive(false);
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
        _items.ForEach(i =>
        {
            var color = selectedColor;
            color.a = 0;
            i.color = color;
        });
        
        var color = selectedColor;
        color.a = 0.5f;
        _items[_selectedIndex - 1].color = color;
    }

    public int IndexItem { get; set; }
}
