using UnityEngine;
using System.Collections;

//处理摄像机跟随玩家
public class CameraFollow : MonoBehaviour {

	//跟随的目标
	public Transform target;
	//移动平滑度
	public float smoothing;

	private Vector3 offset;


	void Start(){
		//计算初始时候的摄像机到目标的距离
		offset = transform.position - target.position;
	}

	void FixedUpdate(){
		//一旦目标移动，那么摄像机移动的坐标就是玩家坐标+之前计算的偏移量
		Vector3 targetCamPos = target.position + offset;
		//将当前摄像机的坐标平滑移动到新坐标
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing);
	}
}
