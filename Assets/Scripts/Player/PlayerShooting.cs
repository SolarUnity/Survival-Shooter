using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	//子弹伤害
    public int damagePerShot = 20;
    //射速
	public float timeBetweenBullets = 0.15f;
    //射程
	public float range = 100f;

	//计时器
    float timer;
    //子弹路线
	Ray shootRay;
    //命中区域
	RaycastHit shootHit;
    //射击有效的目标
	int shootableMask;
    //枪口闪光效果
	ParticleSystem gunParticles;
    //射击弹道线
	LineRenderer gunLine;
   	//枪声
	AudioSource gunAudio;
    //射击闪光
	Light gunLight;
    //效果显示时间
	float effectsDisplayTime = 0.2f;


    void Awake ()
    {
		//获取可以射击的对象
        shootableMask = LayerMask.GetMask ("Shootable");
        //获取组件
		gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
		//计算射击间隔
        timer += Time.deltaTime;

		//计算间隔，射击
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

		//如果射速太快，则直接取消后续子弹的效果
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }

	//关闭当前某个子弹的效果,不是全局
    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
		//重设计时器
        timer = 0f;
		//播放枪声
        gunAudio.Play ();
		//显示枪的闪光
        gunLight.enabled = true;
		//如果上一个射击的枪口火花还没有完成，停止，并播放本次的
        gunParticles.Stop ();
        gunParticles.Play ();
		//显示弹道
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);
		//射击起始地点
        shootRay.origin = transform.position;
        //射击方向（即不断的加Z轴的数字）
		shootRay.direction = transform.forward;

		//如果射击路线(shootRay)最终碰撞到的目标(shootHit)是射击作用目标(shootableMask),并且在射程范围内(range)
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
			//获取敌人血量
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
				//如果获取成功，减血
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
			//并且弹道效果终止
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
			//否则弹道的显示两点就是初始位置,距离*z轴走过的位置
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
