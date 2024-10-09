using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlockAdviceButtons : MonoBehaviour
{
    public GameObject blockAdvicePrefab; 
        
    private Dictionary<AdviceType, Advice> _adviceDictionary = new Dictionary<AdviceType, Advice>();
    private Dictionary<AdviceType, GameObject> _blockAdviceObjects = new Dictionary<AdviceType, GameObject>();
    
    void Awake()
    {
        var aButton = AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Menu/ButtonsAdviceIcons/A Button.png");
        var bButton = AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Menu/ButtonsAdviceIcons/B Button.png");
        var xButton = AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Menu/ButtonsAdviceIcons/X Button.png");
        var crossPiece = AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Menu/ButtonsAdviceIcons/Сrosspiece.png");

        var enter = new Advice(aButton, "Ввод");
        var back = new Advice(bButton, "Назад");
        var unequip = new Advice(xButton, "Снять предмет");
        var select = new Advice(crossPiece, "Выбрать");
        
        _adviceDictionary.Add(AdviceType.ENTER, enter);
        _adviceDictionary.Add(AdviceType.BACK, back);
        _adviceDictionary.Add(AdviceType.UNEQUIP, unequip);
        _adviceDictionary.Add(AdviceType.SELECT, select);

        InitPrefab();
    }

    public void ShowAdvice(List<AdviceType> types)
    {
        foreach (AdviceType type in Enum.GetValues(typeof(AdviceType)))
        {
            _blockAdviceObjects[type].SetActive(types.Contains(type));
        }
    }
    
    private void InitPrefab()
    {
        foreach (AdviceType type in Enum.GetValues(typeof(AdviceType)))
        {
            var img = _adviceDictionary[type].Image;
            var text = _adviceDictionary[type].Text;
            
            var prefab = Instantiate(blockAdvicePrefab, transform);
            prefab.GetComponentInChildren<RawImage>().texture = img;
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = text;
            _blockAdviceObjects.Add(type, prefab);
        }
    }

    public enum AdviceType
    {
        SELECT,
        ENTER,
        BACK,
        UNEQUIP
    }
    
    private class Advice
    {
        private Texture image;
        private string text;

        public Advice(Texture image, string text)
        {
            this.image = image;
            this.text = text;
        }

        public Texture Image
        {
            get { return image; }
        }
        
        public string Text
        {
            get { return text; }
        }
    }
}
