using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {
	//스파크 파티클 프리팹 연결할 변수
	public GameObject sparkEffect;

	//충돌을 시작할 때 발생하는 이벤트
	void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.tag == "BULLET") {
			//스파크 파티클을 동적으로 생성하고 변수에 할당
			GameObject spark =(GameObject)Instantiate (sparkEffect, coll.transform.position, Quaternion.identity);

			//PrarticleSystem 컴포넌트의 수행시간이 지난후 삭제 처리
			Destroy(spark, spark.GetComponent <ParticleSystem>().duration + 0.2f);

			//충돌한 게임오브젝트 삭제
			Destroy (coll.gameObject);
		}
	}
}
