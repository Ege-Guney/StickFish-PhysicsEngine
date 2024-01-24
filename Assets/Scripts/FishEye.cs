using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEye : MonoBehaviour
{
    public GameObject fishEyePrefab;
    public float FloorY = -14f; //X value where floor is
    public float mtUpY = -4f;
    public float mtLeftX = -1f;
    public float mtRightX = 1f;
    public float preference = 0.05f;

    private int location;
    public float xCoordinate = 5.78f;
    public float yCoordinate = -5.59f;

    public float angleUpLimit = 75f;
    public float angleDownLimit = 25f;
    private float resultAngle;

    public float speed = 0.000001f;
    public float initVelLimit = 0.00003f;
    public float gravity = -0.00098f;
    public float initVelocity = -0.00002f; //check location
    
    private float velY;
    private float velX;
    private float accY;
    private float accX;
    private bool isRight;//if on the right pond 

    private float aliveTime;
    public Vector3 originalEyePos;
    // Start is called before the first frame update
    void Start()
    {
       
        if(transform.position.x <= 0){
            isRight = false;
        }
        else{
            isRight = true;
        }

        if(!isRight){
            initVelocity = Random.Range(0.01f, initVelLimit);
        }
        else{
            initVelocity = Random.Range(-initVelLimit, -0.01f);
        }

        resultAngle = Random.Range(angleUpLimit, angleDownLimit);
        if(isRight){
            
            velY = -1 * initVelocity * Mathf.Sin(resultAngle * Mathf.Deg2Rad);
            velX = initVelocity * Mathf.Cos(resultAngle * Mathf.Deg2Rad);
        }
        else{
            
            velY = initVelocity * Mathf.Sin(resultAngle * Mathf.Deg2Rad);
            velX = initVelocity * Mathf.Cos(resultAngle * Mathf.Deg2Rad);
            
        }

        originalEyePos = transform.position;
        accX = 0f;
        accY = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime += Time.deltaTime;
        velX += accX;
        velY += accY;
        Move(velX * speed, velY * speed);
        if(CollisionWithFloor()){
            //check if collided with pond
            if((transform.position.x <= 20 && transform.position.x >= 12) || (transform.position.x >= -20 && transform.position.x <= -12)){
                gameObject.transform.GetComponentInParent<Fish>().DeleteFish();
                Destroy(gameObject);
            }
            else{
                ResolveCollisionWithFloor();
            }
        }

        if(CollisionWithMountain()){
            Debug.Log("Collided with Mountain.");
            gameObject.transform.GetComponentInParent<Fish>().BodyMoveWithConstraints();
            ResolveCollisionWithMountain();
        }

        if(CollisionWithFish()){
            Debug.Log("Fish HIT EACH OTHER!!");
        }
        CheckDeath();
    }

    public Vector3 getOriginalEyePos(){
        return originalEyePos;
    }
    //call from fish
    public void Move(float accx, float accy){
        transform.position = new Vector3(transform.position.x + accx, transform.position.y + accy,0f);
        gameObject.transform.GetComponentInParent<Fish>().BodyMoveWithConstraints();
    }

    public Vector3 getVel(){
        return new Vector3(velX * speed,velY * speed,0f);
    }
    public Vector3 getVelWithoutSpeed(){
        return new Vector3(velX ,velY,0f);
    }

    public void SetVelX(float x){
        velX = x;
    }
    public void SetVelY(float y){
        velY = y;
    }
    bool CollisionWithFish(){
        GameObject[] fishList = GameObject.FindGameObjectsWithTag("Fish");
         for (int i = 0; i < fishList.Length; i++)
        {
            
            FishEye otherEye = fishList[i].GetComponent<Fish>().transform.GetComponentInChildren<FishEye>();
            if(otherEye.transform.position.x == gameObject.transform.position.x && gameObject.transform.position.y == otherEye.transform.position.y){
                continue;
            }

            float Eucdistance = Mathf.Sqrt(Mathf.Pow(otherEye.transform.position.x - transform.position.x, 2) + Mathf.Pow(otherEye.transform.position.y - transform.position.y, 2));
            if(Eucdistance <= gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2){
                Vector3 othPos = otherEye.transform.position;
                
                
                otherEye.ResolveCollisionWithFish(transform.position);
                ResolveCollisionWithFish(othPos);
                
                return true;
            }   
            
        }
        return false;
    }
    public bool CollisionWithMountain(){
        float bottom_eyeBox = transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float right_eyeBox = transform.position.x + gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float left_eyeBox = transform.position.x - gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        if(bottom_eyeBox <= mtUpY){
            
            //right side
            if(transform.position.x >= 0){
                if(left_eyeBox <= getMountainXFromYRight(transform.position.y)){
                    return true;
                }
            }//left side
            else{
                if(right_eyeBox >= (-1 * getMountainXFromYRight(transform.position.y))){
                    return true;
                }
            }
            
        }
        return false;
    }
    public float getMountainXFromYRight(float y){
        
        return (y + 4f)/-10f;
    }
    public float getRightEyeBox(){
        return transform.position.x + gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }
    public float getLeftEyeBox(){
        return transform.position.x - gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }
    bool CollisionWithFloor(){
        float bottom_eyeBox = transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        //bottomside of eyebox
        

        if(FloorY >= bottom_eyeBox){
            Debug.Log("Fish HIT FLOOR!!!");
            return true;
        }
        return false;
    }

    void ResolveCollisionWithFloor(){
        float rand = Random.Range(0.75f,0.92f);
        velX = velX * rand;
        velY = -velY * rand;
    }
    void ResolveCollisionWithMountain(){
        float rand = Random.Range(1.2f,1.48f);
        if(transform.position.y <= -3.8f){
            velX = -velX;
        }
        else if(velY <= 0){
            velY = -velY;
        }
        velY = velY * rand;
    }
    
    
    void ResolveCollisionWithFish(Vector3 othPos){
        float randIncreaser = Random.Range(1.15f,1.3f);
        float randDecreaser = Random.Range(0.70f,0.85f);
        int relativePos = 0; 
        
        //computations for velocity on x plane based on position of objects
        if(transform.position.x >= othPos.x){
            if(velX < 0){
                velX = -velX * randDecreaser;
            }
            else{
                velX = velX * randIncreaser;
            }     
        }
        else{
            if(velX > 0){
                velX = -velX * randDecreaser;
            }
            else{
                velX = velX * randIncreaser;
            } 
        }
        //computations for velocity on y plane based on position of objects
        if(transform.position.y >= othPos.y){
            if(velY > 0){
                velY = velY * randIncreaser;
            }
            else{
                velY = -velY * randDecreaser;
            }
        }
        else{
            if(velY < 0){
                velY = velY * randIncreaser;
            }
            else{
                velY = -velY * randDecreaser;
            }
        }
    }

    void CheckDeath(){
        if(aliveTime >= 15 || transform.position.x > 26 || transform.position.x < -26){
            gameObject.transform.GetComponentInParent<Fish>().DeleteFish();
            Destroy(gameObject);
        }
    }

}
