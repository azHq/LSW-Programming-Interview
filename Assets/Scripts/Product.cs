using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public string productId;
    public string productName;
    public string price;
    public ProductType productType = ProductType.CAP;
    public Sprite trailSprite;
    public Sprite showcaseSprite;
    public Color color;
    void Start()
    {
        
    }

}
