using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 20f;
    public MosquitoController mosquito;
    private Vector2 targetPosition;
    private Vector2 clickPosition;
    public Sprite sprite_idle; // Drag your first sprite here
    public Sprite sprite_jump;
    public Sprite sprite_swim;
    private bool jump = false;
    private float distance = 0f;
    private Quaternion startRotation;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool onGround = true;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        startRotation = transform.rotation;
    }    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform"))
        {
            if (Vector3.Distance(collision.transform.position, transform.position) <= 0.001f) onGround = true;
        }
        else if (collision.gameObject.tag.Equals("Mosquito"))
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isSwim", false);
            animator.SetBool("isEat", true);
            mosquito.DoEat(transform.position);
            if (mosquito.transform.localScale.x <= 0.2f)
            {
                animator.SetBool("isEat", false);
                mosquito.isDead = true;
            }
        }

        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Mosquito"))
        {
            animator.SetBool("isEat", false);
        }
        else if(collision.gameObject.tag.Equals("Platform"))
        {
            onGround = false;
        }        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (onGround && hit && hit.transform.gameObject.tag.Equals("Platform"))
            {
                if(Vector3.Distance(transform.position, hit.transform.position) > 0.1f){
                    targetPosition = hit.transform.position;
                    clickPosition = targetPosition;
                    jump = true;
                }
                
            }

        }
        if(jump) distance = Vector3.Distance(transform.position, targetPosition);
        animator.SetBool("isJump", jump);
        if ((int)distance <= 5 && jump) { 
            if (distance > 0.001f)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                
            }
            else
            {
                mosquito.DoStep();
                jump = false;
            }
        }
        else
        {
                targetPosition = (Vector2)transform.position + (targetPosition - (Vector2)transform.position) * (5f / distance);
        }

        if((int)distance == 0 && !jump)
        {
            Vector2 newTarget = FindClosesPath();
            if (Vector3.Distance(transform.position, newTarget) > 0.001f)
            {
                Vector2 vectorToTarget = newTarget - (Vector2)transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                animator.SetBool("isSwim", true);
                transform.rotation = q;
                float step = 1 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, newTarget, step);
                
            }
            else
            {
                animator.SetBool("isSwim", false);
                transform.rotation = startRotation;
            }

        }
        if( mosquito.isDead && onGround)
        {
            Application.LoadLevel(2);
        }
    }

    private Vector2 FindClosesPath()
    {
        float distanceClosesPlatform = Mathf.Infinity;
        Vector2 closesPath = gameObject.transform.position;
        GameObject[] allGameObject = GameObject.FindGameObjectsWithTag("Platform");
        foreach(GameObject gameObject in allGameObject)
        {
            float dist = Vector3.Distance(gameObject.transform.position, transform.position);
            if(dist < distanceClosesPlatform)
            {
                distanceClosesPlatform = dist;
                closesPath = (Vector2)gameObject.transform.position;
            }
        }
        return closesPath;

    }
}
