using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int MaxObject = 10;
	public float FieldWidth = 20f;
	public float FieldHeight = 25f;
	public bool	Colorize = false;
	
    // public Transform Camera;
	float xOffset;
	float yOffset;
 
	// [SerializeField] private GameObject[] asteroidPrefab;
     private GameObject[] asteroids;
    [SerializeField] private GameObject[] asteroidsPrefab;

	void Awake ()
	{
		asteroids = new GameObject[MaxObject];

		xOffset = FieldWidth * 0.5f;																									
		yOffset = FieldHeight * 0.5f;																									
	
		for ( int i=0; i<asteroids.Length; i++ )
		{				
            Instantiate(asteroidsPrefab[Random.Range(0,asteroidsPrefab.Length)],GetRandomInRectangle( FieldWidth, FieldHeight )+ transform.position,Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
			// asteroids[ i ].transform.position = GetRandomInRectangle( FieldWidth, FieldHeight ) + transform.position;
		
		}

	}
 
 
	// GetRandomInRectangle
	//----------------------------------------------------------
	// Get a random value within a certain rectangle area
	//
	Vector3 GetRandomInRectangle ( float width, float height )
	{
		float x = Random.Range( 0, width );
		float y = Random.Range( 0, height );
		return new Vector3 ( x - xOffset , y - yOffset, 0 );
	}

    // void StarExpand()
    // {
    //     for ( int i=0; i<MaxStars; i++ )
	// 	{
	// 		Vector3 pos = Stars[ i ].position + transform.position;
 
	// 		if ( pos.x < ( Camera.position.x - xOffset ) )
	// 		{
	// 			pos.x += FieldWidth;
	// 		}
	// 		else if ( pos.x > ( Camera.position.x + xOffset ) )
	// 		{
	// 			pos.x -= FieldWidth;
	// 		}
 
	// 		if ( pos.y < ( Camera.position.y - yOffset ) )
	// 		{
	// 			pos.y += FieldHeight;
	// 		}
	// 		else if ( pos.y > ( Camera.position.y + yOffset ) )
	// 		{
	// 			pos.y -= FieldHeight;
	// 		}
 
	// 		Stars[ i ].position = pos - transform.position;
	// 	}
	// 	Particles.SetParticles( Stars, Stars.Length );
	// }
    // void Update ()
	// {
    //     StarExpand();
    // }
}