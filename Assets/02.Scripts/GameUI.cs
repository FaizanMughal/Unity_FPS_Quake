using UnityEngine;
//ui 컴포넌트에 접근하기 위해 추가한 네임스페이스
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {
	public Text txtScore;
	private int totScore = 0;
	// Use this for initialization
	void Start () {
		//처음 실행 후 저장된 스코어 정보 로드
		//otScore = PlayerPrefs.GetInt ("TOT_SCORE", 0);
		DispScore (0);
	}
	
	//스코어 저장 안함
	public void DispScore (int score) {
		totScore += score;
		txtScore.text = "score <color=#ff0000>" + totScore.ToString () + "</color>";
		//스코어 저장
		//PlayerPrefs.SetInt ("TOT_SCORE", totScore);
	}
}
