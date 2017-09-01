using UnityEngine;
using System.Collections;


public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Awake () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2;
	public Transform spawnPrefab;

	void Start()
	{
	}

}
