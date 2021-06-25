using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductDetails : MonoBehaviour
{
    [SerializeField]
    private Image _productImage;
    [SerializeField]
    private TextMeshProUGUI _productId;
    [SerializeField]
    private TextMeshProUGUI _productName;
    [SerializeField]
    private TextMeshProUGUI _productprice;
    void Start()
    {
        
    }
    public void Init(Product product)
    {
        _productId.text = $"Product Id: {product.productId}";
        _productName.text = $"Product Name: {product.productName}";
        _productprice.text = $"Product Price: {product.price} $";
        _productImage.sprite = product.showcaseSprite;
        _productImage.color = product.color;
    }
}
