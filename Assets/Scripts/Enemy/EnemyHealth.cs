using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
 	//沉入地板的速度   
	public float sinkSpeed = 2.5f;
    //一个多少分
	public int scoreValue = 10;
    //死亡音效
	public AudioClip deathClip;

	//自身的组件引用
    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;

	//是否死亡
	private bool isDead;
    //是否沉入完成
	private bool isSinking;

	//初始化
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
		//如果开始下沉
        if(isSinking)
        {
			//那么Y轴向下走你
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


	//收到伤害
    public void TakeDamage (int amount, Vector3 hitPoint)
    {
		if (isDead) {
			return;
		}

        enemyAudio.Play ();

        currentHealth -= amount;
        
		//根据击中位置，计算后退位置
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

		//解除物理碰撞器,可以让玩家穿过
        capsuleCollider.isTrigger = true;
		//死亡动画
        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
		//开始下沉,干掉他的路径渲染对象，让地板可以让他被穿过
		//要注意我们不取消整个NavMeshAgent，而是这个NavMeshAgent的关于这个敌人的组件的(一部分)，所以要用.enabled = false
        GetComponent <NavMeshAgent> ().enabled = false;
        //让自己的碰撞器可以穿过地板
		GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;

		//死了之后要计数啊
       	ScoreManager.score += scoreValue;

		//2秒钟之后销毁对象
		Destroy (gameObject, 2f);
    }
}
