using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rig;
    Animator anim;
    SpriteRenderer sprRender;

    public MakerScript mS;
    public LayerMask GroundLayer;
    public float velocidade = 0;
    public float forcaPulo = 0;
    public bool isGrounded;

    public bool bhit1, bhit2;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*velocidade, rig.velocity.y);

        //var rc = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.2f), 0, Vector2.zero, GroundLayer);
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        Debug.DrawRay(new Vector2(transform.position.x - 0.25f, transform.position.y), direction, Color.green);
        Debug.DrawRay(new Vector2(transform.position.x + 0.25f, transform.position.y), direction, Color.green);
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x - 0.25f, transform.position.y), direction, distance, GroundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.25f, transform.position.y), direction, distance, GroundLayer);

        bhit1 = (hit1.collider != null);
        bhit2 = (hit2.collider != null);
        isGrounded = (hit1.collider != null|| hit2.collider != null);
        
        /*if (hit.collider != null)
        {
            isGrounded = (hit.collider.tag == "Ground");
            anim.SetBool("IsGrounded", (hit.collider.tag == "Ground"));
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rig.velocity = new Vector2(rig.velocity.x, 0);
            rig.AddForce(new Vector2(0, forcaPulo),ForceMode2D.Impulse);
        }

        anim.SetBool("Andando", rig.velocity.x != 0);
        anim.SetBool("IsGrounded", isGrounded);

        if (Input.GetAxis("Horizontal") > 0)
            sprRender.flipX = false;
        if (Input.GetAxis("Horizontal") < 0)
            sprRender.flipX = true;
    }

    public void OnDrawGizmos()
    {
        //var rc = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.2f), 0, Vector2.zero, GroundLayer);
        //Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.2f));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "endTile")
        {
            mS.loadMakerMode();
        }
        if (collision.tag == "espinho")
        {
            mS.Reset();
        }
    }

}
