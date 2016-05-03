using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {
	//총알 파괴력
	public int damage = 20;
	//총알 발사 속도
	public float speed = 1000.0f;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed);
	}
}
