using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float playerSpeed = 5f;
	public static int playerLevel = 1;

	private float jumpMaxHeight = 300f;
	private bool jumpingUp = false;
	private bool dead = false;
	private float dyingTime = 0;
	private float originalY = 0;
	private bool grounded = false;

	// Use this for initialization
	void Start () {
		originalY = transform.position.y;
		playerLevel = 1;
		Debug.Log ("Player Level 1!");
	}
	
	// Update is called once per frame
	void Update () {
		//MORTE
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

		transform.Translate (new Vector3 (playerSpeed * Time.deltaTime, 0, 0));

		//PULO
		float fireInput = Input.GetAxis ("Fire1");
		if(fireInput != 0 && grounded && !jumpingUp)
		{
			rigidbody2D.AddForce(new Vector3(0, jumpMaxHeight), ForceMode2D.Force);
			grounded = false;
			jumpingUp = true;
		}

		//VERIFICA DISTANCIA DO JOGADOR
		SetLevel ();
	}

	void FixedUpdate()
	{
		grounded = (rigidbody2D.velocity.y == 0);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		jumpingUp = false;
		if(other.gameObject.tag == "Ground")
		{
			foreach(var c in other.contacts)
			{
				//VERIFICA COLISAO FRONTAL COM GROUND
				if( c.normal.x != 0 && c.normal.y == 0 && c.collider.gameObject.transform.position.y > -0.25){
					if(transform.position.x < (c.collider.gameObject.transform.position.x - c.collider.gameObject.renderer.bounds.size.x * 0.5f))
					{
						dead = true;

						break;
					}
				}
			}
		}

		//CAIU NO BURACO OU TOCOU OBSTACULO
		if(other.gameObject.tag == "Void" || other.gameObject.tag== "Obstacle")
		{
			dead = true;
		}
	}

	void SetLevel()
	{
		if(transform.position.x > 100 && transform.position.x < 200)
		{
			playerLevel = 2;
			Debug.Log ("Player Level 2!!");
		}
		else{
			if(transform.position.x > 200 && transform.position.x < 400)
			{
				playerLevel = 3;
				Debug.Log ("Player Level 3!!!");
			}
			else
			{
				if(transform.position.x > 400)
				{
					playerLevel = 4;
					Debug.Log ("Player Level 4!!!!");
				}
			}
		}
	}
}
