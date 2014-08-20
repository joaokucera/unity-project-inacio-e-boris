using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player;
	public float speed = 2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Segue o jogador
	void Update () {
		Vector3 pos = player.transform.position;
		pos.x += 6;
		pos.y = transform.position.y;
		pos.z = transform.position.z;
		transform.position = Vector3.Lerp (transform.position, pos, speed * Time.deltaTime);
	}
}
