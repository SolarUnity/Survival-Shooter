using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
	//初始设置的生命值
    public int startingHealth = 100;
    //当前生命值
	public int currentHealth;
    
	//UI的生命条
	public Slider healthSlider;
    //收到伤害后闪过的图片
	public Image damageImage;
	//死亡音效
    public AudioClip deathClip;
    //伤害图片闪过的速率
	public float flashSpeed = 5f;
	//伤害图片的颜色
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

	//动画组件引用
    private Animator anim;
	//声音动画引用
    private AudioSource playerAudio;
	//角色移动引用
	private PlayerMovement playerMovement;
    //角色
	private PlayerShooting playerShooting;

	//是否死亡
    bool isDead;
	//是否收到伤害
    bool damaged;


    void Awake ()
    {
		//通过自身引用得到组件
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();

		//初始化
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
			//如果收到伤害，颜色改变（主要是透明度）
            damageImage.color = flashColour;
        }
        else
        {
			//如果完成了伤害图片闪烁，那么将其渐变为透明(x,y,z,a) -> (0,0,0,0)
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
		//无论怎样，最后都设置为false
        damaged = false;
    }

	//收到伤害，伤害值作为参数传入
    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

		//设置生命条长度
        healthSlider.value = currentHealth;

        playerAudio.Play ();

		//如果生命值小于0同时未触发死亡标记，死亡
        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }

	//死亡触发
    void Death ()
    {
		//标记
        isDead = true;

        playerShooting.DisableEffects ();

		//动画效果触发器触发
        anim.SetTrigger ("Die");

		//更改当前的声音，触发死亡音效
        playerAudio.clip = deathClip;
        playerAudio.Play ();

		//取消移动能力
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

	//这个默认是在Player模型的Event中设置的，我将它搬到了GameOverManager里
	//重新载入关卡,通过外部组件触发
    /*
    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
    */
}
