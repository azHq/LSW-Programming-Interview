using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float _moveSpeed=5f;
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Vector2 _moveDirection;
    private Transform _transform;
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _transform= GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
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
            _animator.SetBool("forward", false);
            _animator.SetBool("back", false);
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
    }
}
