using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private SpriteRenderer cap;
    [SerializeField]
    private SpriteRenderer shirt;
    [SerializeField]
    private SpriteRenderer pant;
    [SerializeField]
    private SpriteRenderer left_shoe;
    [SerializeField]
    private SpriteRenderer right_shoe;
    [SerializeField]
    private Transform _trailPosition;
    [SerializeField]
    private float _moveSpeed=5f;
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Vector2 _moveDirection;
    private Transform _transform;
    private bool _insideShop;
    private GameManager gameManager;
    public ProductType productType = ProductType.CAP;
    private Coroutine coroutine;
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _transform= GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
            ChangePlayerLocalScale(1);
            _animator.SetBool("forward", false);
            _animator.SetBool("walk", false);
            _animator.SetBool("back", true);
        }
        else if(Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow))
        {
            moveY =-1f;
            ChangePlayerLocalScale(1);
            _animator.SetBool("forward", true);
            _animator.SetBool("walk", false);
            _animator.SetBool("back", false);
        }
        else if (Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
            ChangePlayerLocalScale(1);
            _animator.SetBool("walk", true);
            _animator.SetBool("forward", false);
            _animator.SetBool("back", false);
        }
        else if (Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
            ChangePlayerLocalScale(-1);
            _animator.SetBool("walk",true);
            _animator.SetBool("forward", false);
            _animator.SetBool("back", false);
        }
        else
        {
            _animator.SetBool("walk", false);
        }
        _moveDirection = new Vector2(moveX,moveY);
    }
    private void FixedUpdate()
    {
        _rigidbody2d.velocity = _moveDirection * _moveSpeed;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y,-10);
    }

    public void ChangePlayerLocalScale(float xScale)
    {
       
        var scale = _transform.localScale;
        scale.x = xScale;
        _transform.localScale = scale;
        gameManager.startTrail = false;
        _animator.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("door")) gameManager.SetActionPanelVisibility(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("door")) gameManager.SetActionPanelVisibility(false);
    }
    public void ReadForTrail()
    {
       
        transform.position = _trailPosition.position;
        _animator.SetBool("walk", false);
        _animator.SetBool("back", false);
        _animator.SetBool("forward",true);
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine=StartCoroutine(wait(2f));
    }
    private IEnumerator wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _animator.enabled = false;
    }
    public void ChangeDress(Sprite sprite,Color color)
    {
       
        if (productType==ProductType.CAP)
        {
            cap.sprite = sprite;
            cap.color = color;
        }
        else if (productType == ProductType.SHIRT)
        {
            shirt.sprite = sprite;
            shirt.color = color;
        }
        else if (productType == ProductType.PANT)
        {
            pant.sprite = sprite;
            pant.color = color;
        }
        else if (productType == ProductType.SHOES)
        {
            left_shoe.sprite = sprite;
            left_shoe.color = color;
            var scale = left_shoe.transform.localScale;
            scale.x =Mathf.Abs( scale.x)* - 1;
            left_shoe.transform.localScale=scale;
            right_shoe.sprite = sprite;
            right_shoe.color = color;
        }
    }
}
public enum ProductType
{
    CAP,
    SHIRT,
    PANT,
    SHOES
}
