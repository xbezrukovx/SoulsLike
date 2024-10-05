using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSelector : MonoBehaviour
{
    public GameObject contentView;  // Контейнер для элементов инвентаря
    public Color selectedColor;
    
    private int _itemCount;
    private int _selectedIndex = 1; // Индекс выбранного элемента
    private List<Image> _items;

    public int GetSelectedIndex()
    {
        return _selectedIndex;
    }

    // Start is called before the first frame update
    void Awake()
    {
        var rect = contentView.GetComponent<Transform>();
        _itemCount = rect.childCount;
        
        _items = rect
                .transform.Cast<Transform>()  // Преобразуем Transform в IEnumerable
                .Select(t =>
                {
                    selectedColor.a = 0;
                    t.AddComponent<Image>();
                    var image = t.GetComponent<Image>();
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
