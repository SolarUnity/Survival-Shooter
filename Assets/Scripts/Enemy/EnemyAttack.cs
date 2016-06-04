using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	//攻击间隔
    public float timeBetweenAttacks = 0.5f;
    //攻击伤害
	public int attackDamage = 10;

	//各种组件
    private Animator anim;
	private GameObject player;
	private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

	//距离是否足够近能够允许敌人进行攻击
	private bool playerInRange;
	//同步时间，避免过频
	private float timer;


    void Awake ()
    {
		//查找组件
        player = GameObject.FindGameObjectWithTag ("MainPlayer");
		//通过player来查找它的组件
        playerHealth = player.GetComponent <PlayerHealth> ();
		//得到当前的（敌人自己的）血量
       	enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


	//球形碰撞器
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
			//接触到了球形碰撞器后，判断可以攻击
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
			//离开了碰撞器之后，判断过远
            playerInRange = false;
        }
    }


    void Update ()
    {
		//同步时间（0.5s）
        timer += Time.deltaTime;
		//如果上次攻击间隔时间大于攻击间隔，同时位于攻击区域，同时它自己还没有死掉，攻击
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }
		//如果玩家血量小于0
        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
		//重置间隔计数器
        timer = 0f;
		//如果还活着
        if(playerHealth.currentHealth > 0)
        {
			//攻击
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
