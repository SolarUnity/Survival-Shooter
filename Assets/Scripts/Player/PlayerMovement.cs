using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed = 6f;
	 
	private Vector3 movement;
	private Animator animatior;
	private Rigidbody rb;
	private int floorMask;
	private float camRayLength = 100f;

	//和Start方法类似
	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		animatior = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	//渲染物理效果的Update
	void FixedUpdate ()
	{
		float height = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		Move(height,vertical);
		Turning ();
		Animating (height, vertical);
	}

	//用于控制角色移动
	void Move (float h, float v)
	{

		movement.Set (h, 0f, v);
		//deltaTime是每一次Update调用的时间间隔
		movement = movement.normalized * speed * Time.deltaTime;
		//将当前位置和移动向量相加
		rb.MovePosition (transform.position + movement);
	}

	//用于控制角色转向
	void Turning ()
	{
		//得到当前摄像机角度下到玩家人物的直线
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
	
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			//总之不能让玩家旋转的飞起来啊
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			rb.MoveRotation (newRotation);
		}	
	}

	//用于控制角色的移动动画
	void Animating(float h,float v){
		bool walking = h != 0.0f || v != 0.0f;
		//设置动画对象中的那个IsWalking标识
		animatior.SetBool ("IsWalking", walking);
	}
		
}
