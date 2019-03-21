using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    GameObject gameController;
    BoxCollider2D boxCollider;

    public LayerMask interactables;

    float tileSize = 1.6f;
    float moveSpeed = 10;
    float aggroRange = 5;

    public int cooldown=0;
    public int speed;
    public int damage;
    public int hp;
    public int index;

    Vector3 destination=new Vector3(0,0,-1);

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        boxCollider = GetComponent<BoxCollider2D>();

        speed = Random.Range(1, 4);
        damage = Random.Range(1, 3);
        hp = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (destination.z == 0)
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

        if (transform.position.Equals(destination))
        {
            destination = new Vector3(0, 0, -1);
            gameController.GetComponent<GameController>().turn = 1;
        }
    }


    public bool IncrementCooldown()
    {
        cooldown += speed;
        if (cooldown >= 10)
        {
            return ChooseDirection();
        }
        else
            return false;

    }
    private bool ChooseDirection()
    {
        RaycastHit2D hit;
        int direction;
        bool moved = true;

/*        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= aggroRange - tileSize &&
            Mathf.Abs(player.transform.position.y - transform.position.y) <= aggroRange - tileSize)
        {
            Debug.Log(player.transform.position.y + " " + transform.position.y);
            if (player.transform.position.y > transform.position.y)
                direction = 0;
            else if (player.transform.position.y < transform.position.y)
                direction = 1;
            else if (player.transform.position.x > transform.position.x)
                direction = 2;
            else
                direction = 3;
        }
        else*/
            direction = Random.Range(0, 4);

        boxCollider.enabled = false;

        switch (direction)
        {
            case 0:
                destination = transform.position + new Vector3(0, tileSize, 0);
                moved = Move();
                break;
            case 1:
                destination = transform.position + new Vector3(0, -tileSize, 0);
                moved = Move();
                break;
            case 2:
                destination = transform.position + new Vector3(tileSize, 0, 0);
                moved = Move();
                break;
            case 3:
                destination = transform.position + new Vector3(-tileSize, 0, 0);
                moved = Move();
                break;
        }

        boxCollider.enabled = true;
        cooldown -= 10;
        return moved;
    }

    private bool Move()
    {
        RaycastHit2D hit=Physics2D.Linecast(transform.position, destination, interactables);
        if (hit.transform != null)
        {
            if (hit.transform.tag.Equals("Player"))
                hit.transform.GetComponent<PlayerScript>().takeDamage(damage);
            destination = new Vector3(0, 0, -1);
            return false;
        }
        return true;
    }

    public void takeDamage(int damageTaken)
    {
        hp -= damageTaken;
        if (hp <= 0)
        {
//            gameController.GetComponent<GameController>().enemyList.RemoveAt(index);
            gameController.GetComponent<GameController>().catcount--;
            gameObject.SetActive(false);
        }
    }
}
