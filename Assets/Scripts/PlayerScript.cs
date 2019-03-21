using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Vector3 end=new Vector3(0,0,-1);
    float tileSize = 1.6f;
    float moveSpeed = 10;
    private BoxCollider2D boxCollider;
    public LayerMask interactables;
    GameObject gameController;

    public bool playerTurn=true;

    public int cooldown=0;

    public int speed = 1;
    public int hp = 10;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        gameController = GameObject.FindWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn)
        {
            if (end.z == -1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Move(transform.position + new Vector3(0, tileSize, 0));
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Move(transform.position + new Vector3(0, -tileSize, 0));
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Move(transform.position + new Vector3(tileSize, 0, 0));
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Move(transform.position + new Vector3(-tileSize, 0, 0));
                }
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * moveSpeed);
        }
        if (transform.position.Equals(end))
        {
            playerTurn = false;
            end = new Vector3(0, 0, -1);
            gameController.GetComponent<GameController>().turn = 2;
        }
    }

    void Move(Vector3 destination)
    {
        RaycastHit2D hit;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(transform.position, destination, interactables);

        if (hit.transform == null)
        {
            end = destination;
            cooldown -= 10;
        }
        else if (hit.transform.tag.Equals("Enemy"))
        {
            hit.transform.GetComponent<EnemyScript>().takeDamage(damage);
            cooldown -= 10;
            gameController.GetComponent<GameController>().turn = 2;
        }
        boxCollider.enabled = true;
    }

    public bool IncrementCooldown()
    {
        cooldown+=speed;
        if (cooldown >= 10)
        {
            playerTurn = true;
            return true;
        }
        else
            return false;
    }
    public void takeDamage(int damageTaken)
    {
        hp -= damageTaken;
        if (hp <= 0)
        {
            SceneManager.LoadScene(0);
            Debug.Log("You are dead");
        }
    }
}
