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
    public static GameManager Instance;
    private float _raycastDsitance =Mathf.Infinity;
    public bool startTrail;
    private void Awake()
    {
        Instance = this;
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
                _characterController.ChangeDress(product.trailSprite,color);
            }
        }
    }
    public void StartTrial()
    {
        startTrail = true;
        ShowMessage("Please Drag And Drop Your Dress Which One You Want.");
        _characterController.ReadForTrail();
    }
    public void ShowMessage(string message)
    {
        _popup.SetActive(true);
        _popupMessage.text = message;
    }
    public void SetActionPanelVisibility(bool visibility)
    {
        _actionPanel.SetActive(visibility);
    }
}
