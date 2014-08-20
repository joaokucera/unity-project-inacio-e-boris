using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	private float holeMaxSize = 5f;
	private int minHole = 2;
	private int maxHole = 5;
	public float platformInitialY = -0.25f;
	public GameObject spawnObj;
	private float newY;
	private float xIncrement;
	private int level;

	void Start()
	{
		newY = platformInitialY;
		xIncrement = 0;
	}

	void Update () 
	{

	}

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
				float newX = chosenGround.transform.position.x + chosenGround.renderer.bounds.size.x + xIncrement;

				other.transform.position = new Vector3(newX, newY, 0);

				newY = other.transform.position.y;

				//SORTEIA SE A PLATAFORMA APARECERA ACIMA OU NO MESMO NIVEL QUE A ATUAL
				if(Random.Range(0,2) == 0)
				{
					if(newY == platformInitialY)
						newY += other.renderer.bounds.size.y;
					else
						newY = platformInitialY;
				}

				int obstacleLimit = 3, holeRange = maxHole, holeStartRange = minHole;
				switch(PlayerController.playerLevel){
				case 1://dificuldade 1: maximo 1 obstaculos por plataforma e buracos pequenos sempre
					obstacleLimit = 1;
					holeRange = minHole + 1;
					break;
					
				case 2://dificuldade 2: maximo 2 obstaculos por plataforma e buracos pequenos ou grandes (rand)
					obstacleLimit = 2;
					holeRange = maxHole;
					break;
					
				case 3:
					obstacleLimit = 3; //dificuldade 3: maximo 3 obstaculos por plataforma e buracos pequenos ou grandes (rand)
					holeRange = maxHole;
					break;

				case 4:
					obstacleLimit = 3; //dificuldade 3: maximo 3 obstaculos por plataforma e buracos grandes
					holeRange = maxHole;
					holeStartRange = minHole + 1;
					break;
				}

				//CRIANDO O BURACO ENTRE A PLATAFORMA ATUAL E A PROXIMA
				xIncrement = 0;
				int holeSize = Random.Range(0,holeRange);
				if(holeSize >= holeStartRange)
				{
					//Se a proxima plataforma sobe, o gap nao pode ser o maior senao nao consegue pular
					if(newY > platformInitialY && holeSize == maxHole-1)
						holeSize--;

					holeSize = maxHole - holeSize;
					xIncrement = holeMaxSize / holeSize;
				}


				//CRIANDO OBSTACULOS DE ACORDO COM A DIFICULDADE
				int obstacleCount = 0;
				foreach (Transform child in other.transform)
				{
					if(child.gameObject.name == "ObstaclePosition")
					{
						if(child.gameObject.tag != "LastObstacle" || xIncrement == 0)
						{
							if(Random.Range(0,2) == 0 && obstacleCount < obstacleLimit)
							{
								//PArede a frente, fica impossivel pular para esse obstaculo
								float childIncrement = 0;
								if(child.gameObject.tag == "LastObstacleGrass1" && newY != platformInitialY && xIncrement == 0)
								{
									childIncrement = 0.55f;
								}
								Instantiate(spawnObj, new Vector3(child.position.x + childIncrement, child.position.y, 0), Quaternion.identity);
								obstacleCount++;
							}
						}
					}
				}
			}
		}
		else
		{
			//DESTRUINDO OBSTACULOS OBSOLETOS
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
