using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	//玩家血量
    public PlayerHealth playerHealth;
    //杂鱼
	public GameObject[] enemies;
	//中Boss
	public GameObject[] mediumBosses;

    //刷新时间
	public float spawnTime = 3f;
    //多个重生点
	public Transform[] spawnPoints;


    void Start ()
    {
		//不停的调用Spawn方法,在3s之后，每3s生成一个
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
		//如果角色死亡，停止生产
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }
		//随机选择出生点enemies
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		//生成enemy，在选择的生成点的position，rotation
		GameObject enemy ;
		if (Random.Range (0, 10) <= 7) {
			enemy = enemies [Random.Range (0, enemies.Length)];
		} else {
			enemy = mediumBosses [Random.Range (0, mediumBosses.Length)];
		}


        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
