using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwitcher : MonoBehaviour
{
    
    public float timeToSwitch = 2.0f;
    public GameObject[] menuItems;
    public GameObject selector;
    public GameObject[] inventoryItems;
    public GameObject[] equipmentItems;
    
    private int currentItem = 1;
    private RectTransform _rectTransform;
    private static readonly object lockObject = new object();
    private bool isMoving = false; // Флаг для отслеживания состояния перемещения
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = selector.GetComponent<RectTransform>();
        ChangeItemColors(menuItems[currentItem]);
    }

    void OnLeft()
    {
        if (Monitor.TryEnter(lockObject) && currentItem > 0 && !isMoving)
        {
            var menuItem = menuItems[currentItem-1];
            var itemPosition = menuItem.transform.position.x;
            var selectorPosition = _rectTransform.position.x;
            var menuItemPositionDiff = itemPosition - selectorPosition;

            ChangeItemColors(menuItem);

            StartCoroutine(MoveSelector(menuItemPositionDiff));
            currentItem--; // Обновление текущего элемента
        }
    }

    void OnRight()
    {
        if (Monitor.TryEnter(lockObject) && currentItem < menuItems.Length - 1 && !isMoving)
        {
            var menuItem = menuItems[currentItem+1];
            var menuItemPositionDiff = menuItem.transform.position.x - _rectTransform.position.x;
            ChangeItemColors(menuItem);
            StartCoroutine(MoveSelector(menuItemPositionDiff));
            currentItem++; // Обновление текущего элемента
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChangeItemColors(GameObject menuItem)
    {
        try
        {
            foreach (var menuItem2 in menuItems)
            {
                var image = menuItem2.GetComponent<RawImage>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            }

            var currentItemImage = menuItem.GetComponent<RawImage>();
            currentItemImage.color = new Color(currentItemImage.color.r, currentItemImage.color.g,
                currentItemImage.color.b, 1f);
        }
        catch
        {
            Debug.LogError("Can't switch to item color");
        }
    }

    private IEnumerator MoveSelector(float moveAmount)
    {
        isMoving = true; // Устанавливаем флаг, что началось перемещение

        float elapsedTime = 0f;
        Vector3 startPos = _rectTransform.position;
        Vector3 endPos = new Vector3(startPos.x + moveAmount, startPos.y, startPos.z);

        // Плавное изменение позиции в течение timeToSwitch секунд
        while (elapsedTime < timeToSwitch)
        {
            _rectTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToSwitch);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        // Устанавливаем точное конечное положение
        _rectTransform.position = endPos;

        isMoving = false; // Перемещение завершено

        if (currentItem == 1)
        {
            foreach (var item in inventoryItems)
            {
                item.SetActive(false);
            }
            foreach (var item in equipmentItems)
            {
                item.SetActive(true);
            }
        }
        if (currentItem == 0)
        {
            foreach (var item in inventoryItems)
            {
                item.SetActive(true);
            }
            foreach (var item in equipmentItems)
            {
                item.SetActive(false);
            }
        }

        // Освобождаем блокировку
        Monitor.Exit(lockObject);
    }
    
}
