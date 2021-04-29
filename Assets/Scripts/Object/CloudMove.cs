using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{   
    public Sprite[] cloudSprite;
    public float MaxSpeed = 0.25f;
    public float MinSpeed = 0.1f;

    Vector3 randomMove;
    Transform cloudPos;
    float randomDir,randomPos,randomSpeed,speed;
    int randomSprite;
    bool Once = false;
    
    void Start()
    {   
        cloudPos = gameObject.transform;

        randomSprite = Random.Range(0,cloudSprite.Length);
        GetComponent<SpriteRenderer>().sprite = cloudSprite[randomSprite];
        
        randomDir = Random.Range(-1f,1f);
        randomPos = Random.Range(-15f,15f);
        randomSpeed = Random.Range(MinSpeed,MaxSpeed);
        speed = randomSpeed;
        cloudPos.position = cloudPos.position + new Vector3(randomPos,0,0);

    }
    private void Update() {
        goLeftRight();
    }

    private void goLeftRight()
    {   
        randomMove.x = randomDir * speed * Time.deltaTime;
        cloudPos.Translate(randomMove.x,0,0);  
    }
    private void OnBecameVisible() {
        speed = randomSpeed;
    }
    private void OnBecameInvisible() {
        speed = 0;
    }

}
