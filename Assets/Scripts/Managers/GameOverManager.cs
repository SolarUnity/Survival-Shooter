using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	//当前引用的玩家对象
    public PlayerHealth playerHealth;
	//重新开始的延迟时间
	public float restartDelay;

	private Animator anim;
	private float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
		//发现玩家死了，在动画的触发器中设置GameOver
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");

			restartTimer += Time.deltaTime;

			if(restartTimer >= restartDelay){
				SceneManager.LoadScene (0);
			}
        }
    }
}
