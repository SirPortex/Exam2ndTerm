using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MarioScript : MonoBehaviour
{
    public KeyCode rightKey, leftKey, jumpKey;
    public float speed, rayDistance, jumpForce;
    public LayerMask groundMask;
    public AudioClip jumpClip;
    public GameObject fireworkPrefab;

    public int maxJumps = 2; //maximos saltos, se puede modificar desde el editor.

    private Rigidbody2D rb;
    private SpriteRenderer _rend;
    private Animator _animator;
    private Vector2 dir;
    private bool _intentionToJump;

    private int currentJumps = 0; // Saltos iniciales.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // print(GameManager.instance.GetTime());

        dir = Vector2.zero;
        if (Input.GetKey(rightKey))
        {
            _rend.flipX = false;
            dir = Vector2.right;
        }
        else if (Input.GetKey(leftKey))
        {
            _rend.flipX = true;
            dir = new Vector2(-1, 0);
        }

        // _intentionToJump = false;
        if (Input.GetKeyDown(jumpKey))
        {
            _intentionToJump = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(fireworkPrefab, transform.position, Quaternion.identity);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            print("ClickDerecho");
        }

        #region ANIMACIONES
        // ANIMACIONES (PROXIMA DIA ORGANIZARLO EN OTRO SCRIPT)
        if (dir != Vector2.zero)
        {
            // estamos andando
            _animator.SetBool("isWalking", true);
        }
        else
        {
            // estamos parados
            _animator.SetBool("isWalking", false);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        bool grnd = IsGrounded();
        float currentYVel = rb.velocity.y;
        Vector2 nVel = dir * speed;
        nVel.y = currentYVel;

        rb.velocity = nVel;


        if (_intentionToJump && (IsGrounded() || currentJumps < maxJumps))
        {
            //_animator.Play("jumpAnimation");
            AddJumpForce();
            currentJumps++; //Suma uno a la variable (+1).
        }
        _intentionToJump = false;

        //_animator.SetBool("isGrounded", grnd);
    }

    public void AddJumpForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce * rb.gravityScale, ForceMode2D.Impulse);
        AudioManager.instance.PlayAudio(jumpClip, "jumpSound");
    }

    private bool IsGrounded()
    {
        RaycastHit2D collision = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundMask);
        if (collision)
        {
            currentJumps = 0;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
    }
}
