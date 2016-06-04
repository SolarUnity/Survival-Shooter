using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;


    void Awake ()
    {
		//找到玩家坐标
        player = GameObject.FindGameObjectWithTag ("MainPlayer").transform;
       	//找到玩家和自己的血量
		playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
		//如果还都还存活，那就互相吸引
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
			//然后查找寻路路径，通过之前的Bake生成的
            nav.SetDestination (player.position);
        }
        else
        {
            nav.enabled = false;
       }
    }
}
