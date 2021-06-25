using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private GameObject _actionPanel;
    [SerializeField]
    private TextMeshProUGUI _popupMessage;
    [SerializeField]
    private GameObject _popup;
    [SerializeField]
    private ProductDetails _productDetails;
    [SerializeField]
    private Transform _parent;
    [HideInInspector]
    public TextToSpeech textToSpeech;
    [SerializeField]
    private Texture2D cursor;
    public static GameManager Instance;
    private float _raycastDsitance =Mathf.Infinity;
    [HideInInspector]
    public bool startTrail;
    private List<ProductDetails> productDetailsList = new List<ProductDetails>();
    private Coroutine coroutine;
    [HideInInspector]
    public bool askPrice;
    private void Awake()
    {
        Instance = this;
        textToSpeech = GetComponent<TextToSpeech>();
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void Update()
    {
        if (startTrail&& Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
             string pattern= @"^(shirt|shoe|cap)$";
            if (hit.transform != null&& Regex.IsMatch(hit.transform.name, pattern))
            {

                string name = hit.transform.name;
                if (name.Equals("shirt"))
                {
                    _characterController.productType = ProductType.SHIRT;
                }
                else if (name.Equals("cap"))
                {
                    _characterController.productType = ProductType.CAP;
                }
                else if (name.Equals("shoe"))
                {
                    _characterController.productType = ProductType.SHOES;
                }
                Product product = hit.transform.GetComponent<Product>();
                Color color = hit.transform.GetComponent<SpriteRenderer>().color;
                product.color = color;
                if(!askPrice)_characterController.ChangeDress(product.trailSprite,color);
                else
                {
                    TextToSpeech($"{product.price}$");
                }
                ProductDetails productDetails = Instantiate(_productDetails,_parent);
                productDetails.transform.SetSiblingIndex(0);
                productDetails.Init(product);
                productDetailsList.Add(productDetails);
            }
        }
    }
    public void StartTrial()
    {
        startTrail = true;
        askPrice = false;
        ShowMessage("Please Select Product Which One You Want.");
        _characterController.ReadForTrail();
    }
    public void AskPrice()
    {
        askPrice = true;
        TextToSpeech("Please select your product to know price");
        ShowMessage("Please select your product to know price.");
    }
    public void ShowMessage(string message)
    {
        _popup.SetActive(true);
        _popupMessage.text = message;
    }
    public void SetActionPanelVisibility(bool visibility)
    {
        _actionPanel.SetActive(visibility);
        if (!visibility)
        {
            for (int i=0;i<productDetailsList.Count;i++)
            {
                Destroy(productDetailsList[i]);
            }
           
        }
        else
        {
            TextToSpeech("Welcome to our virtual shop");
        }
    }
    public void TextToSpeech(string text)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(textToSpeech.ProcessText(text));
    }
}
