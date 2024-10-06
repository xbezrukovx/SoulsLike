using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCategoryScript : MonoBehaviour
{
    
    public Texture InventoryImg;
    public Texture EquipmentImg;
    public Texture OptionsImg;
    
    public string invetoryTitle;
    public string equipmentTitle;
    public string optionsTitle;
    
    public string invetoryDescription;
    public string equipmentDescription;
    public string optionsDescription;

    private RawImage _image;
    private TextMeshProUGUI _titleText;
    private TextMeshProUGUI _descriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = transform.Find("CategoryImage").GetComponent<RawImage>();
        _titleText = transform.Find("CategoryTitle").Find("TextCategory").GetComponent<TextMeshProUGUI>();
        _descriptionText = transform.Find("CategoryOption").Find("TextOption").GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCategory(Random.Range(0, 3));
        }
    }
    
    public void SetCategory(int category) {
        if (category == 0)
        {
            _image.texture = InventoryImg;
            _titleText.text = invetoryTitle;
            _descriptionText.text = invetoryDescription;
        }
        if (category == 1)
        {
            _image.texture = EquipmentImg;
            _titleText.text = equipmentTitle;
            _descriptionText.text = equipmentDescription;
        }
        if (category == 2)
        {
            _image.texture = OptionsImg;
            _titleText.text = optionsTitle;
            _descriptionText.text = optionsDescription;
        }
    }
}
