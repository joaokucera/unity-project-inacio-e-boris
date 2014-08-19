using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Ground")
		{
			var otherGround = GameObject.FindGameObjectsWithTag ("Ground");
			GameObject chosenGround = null;
			float greatestX = other.transform.position.x;

			foreach(var g in otherGround)
			{
				if(g.transform.position.x > greatestX)
				{
					greatestX = g.transform.position.x;
					chosenGround = g;
				}
			}

			if(chosenGround != null)
			{
				float newY = other.transform.position.y;
				/*if(Random.Range(0,2) == 0)
				{
					if(newY == -0.25f)
						newY += chosenGround.renderer.bounds.size.y;
					else
						newY = -0.25f;
				}*/

				other.transform.position = new Vector3(chosenGround.transform.position.x + chosenGround.renderer.bounds.size.x, newY, 0);
				/*var obstacleSpawner = GameObject.Find("ObstaclePosition");
				obstacleSpawner.transform.position = new Vector3(obstacleSpawner.transform.position.x, other.transform.position.y + chosenGround.renderer.bounds.size.y *0.5f, obstacleSpawner.transform.position.z);
				*/
			}
		}
		else
		{
			if(other.gameObject.tag == "Obstacle")
			{
				if(other.gameObject.transform.parent)
				{
					Destroy(other.gameObject.transform.parent.gameObject);
				}
				else
				{
					Destroy(other.gameObject);
				}
			}
		}
	}
}
