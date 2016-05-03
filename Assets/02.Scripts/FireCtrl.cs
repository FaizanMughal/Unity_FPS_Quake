using UnityEngine;
using System.Collections;

//반드시 필요한 컴포넌트를 명시해 해당 컴포넌트가 삭제되는 것을 방지한다
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour {
	//총알 프리팹
	public GameObject bullet;
	//총알 발사좌표
	public Transform firePos;
	//총알 발사 사운드
	public AudioClip fireSfx;
	//AudioSource 컴포넌트를 저장할 변수
	public AudioSource source = null;
	//MuzzleFlash의 MeshRenderer 컴포넌트 연결변수
	public MeshRenderer muzzleFlash;

	void Update () {
		//ray를 시작적으로 표시하기 위해 사용
		Debug.DrawRay (firePos.position, firePos.forward * 10.0f, Color.green);
		//마우스 왼쪽 버튼을 클릭했을 때 Fire 함수 호출
		if (Input.GetMouseButtonDown (0)) {
			Fire();
			//ray에 맞은 게임 오브젝트의 정보를 받아올 변수
			RaycastHit hit;
			//raycast 함수로 ray를 발사해 맞은 게임오브젝트가 있을 때 true를 반환
			if(Physics.Raycast(firePos.position, firePos.forward, out hit, 10.0f)){
				if(hit.collider.tag == "MONSTER"){
					object[] _params = new object[2];
					_params[0] = hit.point; //ray에 맞은 정확한 위치값
					_params[1] = 20; //몬스터에 입힐 데미지 값
					//몬스터에게 데미지 입히는 함수 호출
					hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
				}
				//ray에 맞은 게임 오브젝트가 Barrel인지 확인
				if(hit.collider.tag == "BARREL"){
					object[] _params = new object[2];
					_params[0] = firePos.position;
					_params[1] = hit.point;
					hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
	void Fire() {
		//raycast 방식으로 변경되므로 총알을 만드는 루틴은 주석으로 처리
		//동적으로 총알을 생성하는 함수
		//CreateBullet ();
		//사운드 발생 함수
		//source.PlayOneShot(fireSfx, 0.9f);
		GameMgr.instance.PlaySfx (firePos.position, fireSfx);
		//잠시 기다리는 루틴을 위해 코루틴 함수로 호출
		StartCoroutine (this.ShowMuzzleFlash ());
	}
	IEnumerator ShowMuzzleFlash(){
		float scale = Random.Range (1.0f, 2.0f);
		muzzleFlash.transform.localScale = Vector3.one * scale;

		//MuzzleFlash를 z축을 기준으로 불규칙하게 회전시킴
		Quaternion rot = Quaternion.Euler (0, 0, Random.Range (0, 360));
		muzzleFlash.transform.localRotation = rot;
		//활성화해서 보이게 함
		muzzleFlash.enabled = true;
		//불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
		yield return new WaitForSeconds (Random.Range (0.05f, 0.3f));
		//비활성화 해서 보이지 않게 함
		muzzleFlash.enabled = false;
	}
	void CreateBullet() {
		//Bullet 프리팹을 동적으로 생성
		Instantiate (bullet, firePos.position, firePos.rotation);
	}

	void Start() {
		source = GetComponent<AudioSource>();
		//최조의 MuzzleFlash Meshrenderer을 비활성화
		muzzleFlash.enabled = false;
	}
}
