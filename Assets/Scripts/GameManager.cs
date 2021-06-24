using System.Collections;
using System.Collections.Generic;
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

            if (hit.transform != null)
            {
                Debug.Log("collider:"+ hit.transform.name);
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
