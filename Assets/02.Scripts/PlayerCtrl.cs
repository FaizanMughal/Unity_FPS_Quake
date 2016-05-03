using UnityEngine;
//ui항목을 사용하기 위해 추가
using UnityEngine.UI;
using System.Collections;

//클래스에 System.Serializable 이라는 어트리뷰트를 명시해야 Inspector 뷰에 노출됨
[System.Serializable]
public class Anim
{
	public AnimationClip idle;
	public AnimationClip runForward;
	public AnimationClip runBackward;
	public AnimationClip runRight;
	public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour {
	private float h = 0.0f;
	private float v = 0.0f;

	//접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
	private Transform tr;
	//이동 속도 변수
	public float moveSpeed = 10.0f;
	//회전 속도 변수
	public float rotSpeed = 100.0f;

	//인스펙터뷰에 표시할 애니메이션 클래스 변수
	public Anim anim;
	//아래에 있는 3d 모델의 애니메이션 컴포넌트에 접근하기 위한 변수
	public Animation _animation;
	//Player의 생명변수
	public int hp = 100;
	//델리게이트 및 이벤트 선언
	public delegate void PlayerDieHandler();
	public static event PlayerDieHandler OnPlayerDie;

	//Player의 생명 초기값
	private int initHp;
	//player의 health bar 이미지
	public Image imgHpbar;
	//게임 매니저에게 접근하기 위한 변수
	private GameMgr gameMgr;

	// Use this for initialization
	void Start () {
		initHp = hp;
		//스크립트 처음에 Transform 컴포넌트 할당
		tr = GetComponent<Transform> ();

		//자신의 하위에 있는 Ainmation 컴포넌트를 찾아와 변수에 할당
		_animation = GetComponentInChildren<Animation> ();

		//Animation 컴포넌트의 애니메이션 클립을 지정하고 실행
		_animation.clip = anim.idle;
		_animation.Play ();

		//GameMgr 스크립트 할당
		gameMgr = GameObject.Find ("GameManager").GetComponent<GameMgr> ();
	}
	
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		Debug.Log ("H=" + h.ToString());
		Debug.Log ("V=" + v.ToString());

		//전후좌우 이동 방향 벡터 계산
		Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

		//Translate(이동방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표)
		tr.Translate (moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

		//Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
		tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis ("Mouse X"));

		//키보드 입력값을 기준으로 동작할 애니메이션 수행
		if (v >= 0.1f) {
			//전진 애니메이션
			_animation.CrossFade(anim.runForward.name, 0.3f);
		}
		else if (v <= -0.1f) {
			//후진 애니메이션
			_animation.CrossFade(anim.runBackward.name, 0.3f);
		}
		else if (h >= 0.1f) {
			//오른쪽 애니메이션
			_animation.CrossFade(anim.runRight.name, 0.3f);
		}
		else if(h <= -0.1f) {
			//왼쪽 애니메쪽이션
			_animation.CrossFade(anim.runLeft.name, 0.3f);
		}
		else {
			//정지 애니메이션
			_animation.CrossFade(anim.idle.name, 0.3f);
		}
	}
	//충돌한 Collider 의 IsTrigger 옵션이 체크됐을 때 발생
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "PUNCH") {
			hp -= 10;
			//Image UI항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
			imgHpbar.fillAmount = (float)hp/(float)initHp;
			Debug.Log ("Player HP = " + hp.ToString());
			//Player의 생명이 0이면 사망처리
			if(hp <= 0)
			{
				PlayerDie();
			}
		}
	}
	void PlayerDie()
	{
		Debug.Log ("player Die!!");
		/*
		GameObject[] monsters = GameObject.FindGameObjectsWithTag ("MONSTER");
		foreach (GameObject monster in monsters) {
			monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
		}
		*/
		//이벤트 발생시킴
		OnPlayerDie ();

		//GameMgr의 싱글턴 인스턴스를 접근해 isGameOver 변수값을 변경
		gameMgr.isGameOver = true;
	}
}
