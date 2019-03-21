using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    GameObject player,floors;
    public int turn=1;
    public int catcount = 0;
    public GameObject enemyprefab;
    public List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindWithTag("Player");
//        enemy = GameObject.FindWithTag("Enemy");
        floors= GameObject.FindWithTag("FloorsObj");
        SpawnCats();

    }

    // Update is called once per frame
    void Update()
    {
        if (turn == 1)
        {
            if (player.GetComponent<PlayerScript>().IncrementCooldown())
                turn = 0;
            else
                turn = 2;
        }
        else if (turn == 2)
        {
            //increment cats' cooldowns
            foreach (GameObject enemy in enemyList)
            {
                if (enemy.GetComponent<EnemyScript>().IncrementCooldown())
                    turn = 0;
            }
            if (turn == 2)
                turn = 1;
        }
    }
    void SpawnCats()
    {
        int rand;
        foreach (Transform child in floors.transform)
        {
            rand = Random.Range(0, 40);
            if (rand == 9)
            {
                catcount++;
                enemyList.Add(Instantiate(enemyprefab, child.transform.position, child.transform.rotation));
                enemyList[catcount - 1].GetComponent<EnemyScript>().index = catcount - 1;
            }
        }
    }

}
