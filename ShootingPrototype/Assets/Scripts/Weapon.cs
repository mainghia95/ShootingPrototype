using UnityEngine;
using System.Collections;
using Rewired;

public class Weapon : MonoBehaviour {
	public Transform playerTrans;
	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask whatToHit;
	public Vector2 moveDir;
	public Transform BulletTrailPrefab;
	public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;

	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;
	public Player player;
	public int playerId;
	public string FireButton;
	public string Horizontal;
	public string Vertical;
	float timeToFire = 0;
	public Transform firePoint;

	// Use this for initialization
	void Awake () {

		player = ReInput.players.GetPlayer (playerId);
	}

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update () {
		moveDir = new Vector2 (1,0);
		moveDir = new Vector2(player.GetAxisRaw(Horizontal),player.GetAxisRaw(Vertical));
		if (fireRate == 0) {
			if (player.GetButtonDown (FireButton)) {
				Shoot();
			}
		}
		else {
			if (player.GetButton (FireButton) && Time.time > timeToFire) {
				timeToFire = Time.time + 1/fireRate;
				Shoot();
			}
		}
	}

	void Shoot () {		
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, moveDir, 1000, whatToHit);

		Debug.DrawLine (firePointPosition, moveDir*1000, Color.cyan);
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			PlayerController otherPlayer = hit.collider.GetComponent<PlayerController>();
			if (otherPlayer != null) {
				otherPlayer.Die ();
				Debug.Log ("We hit " + hit.collider.name);
			}
		}

		if (Time.time >= timeToSpawnEffect)
		{
			Vector3 hitPos;
			Vector3 hitNormal;

			if (hit.collider == null) {
				hitPos = moveDir * 300;
				hitNormal = new Vector3(9999, 9999, 9999);
			}
			else
			{
				hitPos = hit.point;
				hitNormal = hit.normal;
			}

			Effect(hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
	}

	void Effect(Vector3 hitPos, Vector3 hitNormal)
	{
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position,firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer>();
		lr.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
		if (lr != null)
		{
			lr.SetPosition(0, firePoint.position);
			lr.SetPosition(1, hitPos);
		}

		Destroy(trail.gameObject, 0.04f);

		if (hitNormal != new Vector3(9999, 9999, 9999))
		{
			Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
			Destroy(hitParticle.gameObject, 1f);
		}

		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.05f);


	}
}
