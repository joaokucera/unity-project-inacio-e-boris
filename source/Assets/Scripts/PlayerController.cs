using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float playerSpeed = 5f;
	private float jumpAccell = 20f;
	private float jumpSpeed = 0;
	private float jumpMaxSpeed = 6f;
	private float jumpMaxHeight = 3f;
	private bool jumpingUp = false;
	private bool jumpingDown = false;
	private bool dead = false;
	private float dyingTime = 0;
	private float originalY = 0;

	// Use this for initialization
	void Start () {
		originalY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		//restarting level after death
		if(dead)
		{
			renderer.material.color = Color.red;
			dyingTime += Time.deltaTime;
			if(dyingTime > 2)
			{
				Application.LoadLevel (0);
			}
			return;
		}

		float y = 0;
		float fireInput = Input.GetAxis ("Fire1");

		if(fireInput != 0 && !jumpingUp && !jumpingDown)
		{
			jumpingUp = true;
		}

		jumpSpeed = jumpSpeed + (jumpAccell * Time.deltaTime);
		if(jumpSpeed > jumpMaxSpeed)
		{
			jumpSpeed = jumpMaxSpeed;
		}

		if (jumpingUp) 
		{
			y = (jumpSpeed * Time.deltaTime);
			if (transform.position.y >= jumpMaxHeight) {
				jumpingDown = true;
				jumpingUp = false;
				jumpSpeed = 0;
			}
		} else {
			if (jumpingDown) {
				y = - (jumpSpeed * Time.deltaTime);
				if((transform.position.y + y) <= originalY){
					jumpingDown = false;
					jumpSpeed = 0;
					y = originalY - transform.position.y;
				}
			}
		}

		transform.Translate (new Vector3 (playerSpeed * Time.deltaTime, y, 0));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag== "Obstacle") {
			Debug.Log ("Collided!!!");
			dead = true;

		}
	}
}
