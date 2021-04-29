﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
 
 
public class StarField : MonoBehaviour
{   
    //----- From Guido Henkel -----
	public int MaxStars = 100;
	public float StarSize = 0.1f;
	public float StarSizeRange = 0.5f;
	public float FieldWidth = 20f;
	public float FieldHeight = 25f;
	public bool	Colorize = false;
	
    public Transform Camera;
	float xOffset;
	float yOffset;
 
	ParticleSystem Particles;
	ParticleSystem.Particle[] Stars;
	
 
	void Awake ()
	{
		Stars = new ParticleSystem.Particle[ MaxStars ];
		Particles = GetComponent<ParticleSystem>();
 
		// Assert.IsNotNull( Particles, "Particle system missing from object!" );
 
		xOffset = FieldWidth * 0.5f;																									
		yOffset = FieldHeight * 0.5f;																									
	
		for ( int i=0; i<MaxStars; i++ )
		{
			float randSize = Random.Range( StarSizeRange, StarSizeRange + 1f );					
			float scaledColor = ( true == Colorize ) ? randSize - StarSizeRange : 1f;			
 
			Stars[ i ].position = GetRandomInRectangle( FieldWidth, FieldHeight ) + transform.position;
			Stars[ i ].startSize = StarSize * randSize;
			Stars[ i ].startColor = new Color( 1f, scaledColor, scaledColor, 1f );
		}
		Particles.SetParticles( Stars, Stars.Length );
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

    void StarExpand()
    {
        for ( int i=0; i<MaxStars; i++ )
		{
			Vector3 pos = Stars[ i ].position + transform.position;
 
			if ( pos.x < ( Camera.position.x - xOffset ) )
			{
				pos.x += FieldWidth;
			}
			else if ( pos.x > ( Camera.position.x + xOffset ) )
			{
				pos.x -= FieldWidth;
			}
 
			if ( pos.y < ( Camera.position.y - yOffset ) )
			{
				pos.y += FieldHeight;
			}
			else if ( pos.y > ( Camera.position.y + yOffset ) )
			{
				pos.y -= FieldHeight;
			}
 
			Stars[ i ].position = pos - transform.position;
		}
		Particles.SetParticles( Stars, Stars.Length );
	}
    void Update ()
	{
        StarExpand();
    }
}