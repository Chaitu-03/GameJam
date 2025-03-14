using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //=> start is gonna be called initially oinly while update is called in every frame 


//it's a good habit to make all values private and then make them partially public, which is adding [SerializeField] before the variable
    public Rigidbody2D rb;
    public Animator anim;
    //component of animator has been created in child object Animator of Player

    [SerializeField]private float movespeed;
    [SerializeField]private bool IsMoving;

    private bool isgrounded;
    [SerializeField]private float Ground_Check;
    //LayerMask helps to differentiate between the layers of the objects aka the player and ground
    [SerializeField]private LayerMask Ground_Layer;
    [SerializeField]private float jumpForce;
    private SpriteRenderer spriteRenderer;
    private float xInput;
    private float yInput;
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //making ball move up and down without any bounds -> your method (very slow movement of ball because it's constantly being called)
        // xInput= Input.GetAxisRaw("Horizontal");
        // yInput= Input.GetAxisRaw("Vertical");
        // rb.linearVelocity = new Vector2(xInput*movespeed, yInput*movespeed);
        
        //Sir's method for moving the ball up and down without any bounds
        Movement();
        
        Inputs();

        isgrounded=Physics2D.Raycast(transform.position, Vector2.down, Ground_Check, Ground_Layer);

        // this is how we can access the velocity of the rigidbody to push the ball to the right by 5 units
        /*
            Left arrow keyif class is made public, we can view it's stuff in the inspec tor
            whereas when it's private, we can't view it's stuff in the inspector
            also, if private, we can't access it from other scripts 
        */
        // if(Input.GetKey(KeyCode.Space)){
        //     Debug.Log("You are pressing the space key");
        // }

        // if (Input.GetKeyDown(KeyCode.Space)){
        //     Debug.Log("You pressed the space key");
        // }

        // if (Input.GetKeyUp(KeyCode.Space)){
        //     Debug.Log("You released the space key");
        // }
        // Debug.Log(Input.GetAxisRaw("Horizontal")); // Gradually increases from 0 to 1
        // GetAxisRaw returns a value between -1 and 1 and
        // GetAxis returns a value between -1 and 1 but it's more smooth
        Animator_controllers(); 
    }
    private void Inputs(){
        xInput= Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
    }

    private void Movement(){
        rb.linearVelocity=new Vector2(xInput*movespeed, rb.linearVelocity.y);
    }

    private void Jump(){
        if (isgrounded){
            rb.linearVelocity=new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void Animator_controllers(){
        bool movement_check(){
            if (rb.linearVelocity.x!=0) return true; 
            else return false;
        }

        //flipping the sprite based on the direction of the movement
        //issue: the collider is not flipping properly when the sprite is moving left
        if (xInput < 0){
            spriteRenderer.flipX = true; // Moving left
        }
        else if (xInput > 0)
            spriteRenderer.flipX = false; // Moving right
        IsMoving=movement_check();
        anim.SetBool("IsMoving",IsMoving); 
        anim.SetBool("IsGrounded", isgrounded);
    }

    private void OnDrawGizmos(){
        //transform.position represents the world position of a GameObject in Unity’s global coordinate system (X, Y, Z).
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y-Ground_Check));
    }
}
