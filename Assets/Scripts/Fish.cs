using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    LineRenderer lineRenderer;
    SpriteRenderer wall;
    FishCreator fishCreator;
    
    public Transform fishEye;
    
    public List<Vector3> fishEdges = new List<Vector3>();
    private List<Vector3> prevEdges = new List<Vector3>();
    private List<Vector3> initEdges = new List<Vector3>();
  
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        
        lineRenderer = GetComponent<LineRenderer>(); //get line renderer component !! -- All lines of fish
        
        
        //my child is MY EYE
        fishEye = this.gameObject.transform.GetChild(0);

        CreateFish();
        //instantiate Fish
        
        //accY = gravity; //gravity
        //accY = 0f;
        //accX = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
        for (int i = 0; i < fishEdges.Count; i++){
            prevEdges[i] = fishEdges[i];
            
        }
        

        setFishLines();
    }

    public void CreateFish(){
        
        if(fishEye.position.x <= 0){
            
            fishEdges.Add(new Vector3(0f, -0.25f,0f));
            fishEdges.Add(new Vector3(0.25f, 0.12f,0f));
            fishEdges.Add(new Vector3(1f, -0.25f,0f));
            fishEdges.Add(new Vector3(2f, 0f,0f));
            fishEdges.Add(new Vector3(1.75f, 0.25f,0f));
            fishEdges.Add(new Vector3(2f, 0.5f,0f));
            fishEdges.Add(new Vector3(1f, 0.75f,0f));
            fishEdges.Add(new Vector3(0.25f, 0.25f,0f));
            fishEdges.Add(new Vector3(0f, 0.5f,0f));
            fishEdges.Add(new Vector3(0f, -0.25f,0f));
            
        }
        else{
            
            fishEdges.Add(new Vector3(0f, -0.25f,0f));
            fishEdges.Add(new Vector3(-0.25f, 0.12f,0f));
            fishEdges.Add(new Vector3(-1f, -0.25f,0f));
            fishEdges.Add(new Vector3(-2f, 0f,0f));
            fishEdges.Add(new Vector3(-1.75f, 0.25f,0f));
            fishEdges.Add(new Vector3(-2f, 0.5f,0f));
            fishEdges.Add(new Vector3(-1f, 0.75f,0f));
            fishEdges.Add(new Vector3(-0.25f, 0.25f,0f));
            fishEdges.Add(new Vector3(0f, 0.5f,0f));
            fishEdges.Add(new Vector3(0f, -0.25f,0f));
        }
        
        for(int i = 0; i < fishEdges.Count; i++)
        {
            prevEdges.Add(fishEdges[i]);
            initEdges.Add(fishEdges[i]);
            //initial edges!! For detection afterwards
        }
        //gameObject.transform.GetComponentInChildren<FishEye>().createFishEye();
    }

    public void setFishLines(){
        lineRenderer.SetWidth(0.1f,0.1f);
        lineRenderer.useWorldSpace = false;
        lineRenderer.SetVertexCount(fishEdges.Count);
        for (int i = 0; i < fishEdges.Count; i++)
        {
            lineRenderer.SetPosition(i, fishEdges[i]);
        }
    }

    void Move(){
        
 
        /*Vector3 vel = gameObject.transform.GetComponentInChildren<FishEye>().getVel();
        for (int i = 0; i < fishEdges.Count; i++)
        {
            Vector3 before_v = prevEdges[i];
            Vector3 current_v = fishEdges[i] + fishEdges[i] + vel;

            fishEdges[i] = current_v - before_v;
        }*/
    }

    public void BodyMoveWithConstraints(){

        Vector3 eyeVel = gameObject.transform.GetComponentInChildren<FishEye>().getVel();
        //compute based on eye position and velocity
        Vector3 eyePos = gameObject.transform.GetComponentInChildren<FishEye>().transform.position;
        Vector3 original_eyePos = gameObject.transform.GetComponentInChildren<FishEye>().getOriginalEyePos();
        for (int i = 0; i < fishEdges.Count; i++)
        {
            
            float randY = Random.Range(1.0003f,1.001f);
            float randX = Random.Range(1.0006f,1.002f);
            //positive or negative for x & y
            float randXPN = Random.Range(0f,1f);
            float randYPN = Random.Range(0f,1f);
            randX = (randXPN < 0.5f)?2-randX: randX;
            randY = (randYPN < 0.5f)?2-randY:randY;
            Vector3 original_pos = initEdges[i];
            
            //gives original distance of x & y
            float original_x_distance = original_eyePos.x + eyeVel.x - original_pos.x;
            float original_y_distance = original_eyePos.y + eyeVel.y - original_pos.y;
            float newX = eyePos.x + eyeVel.x - original_x_distance * randX;
            float newY = eyePos.y + eyeVel.y - original_y_distance * randY;
            
            if(eyePos.y <= -13.3f && newY < 0.1f){
                newY = 0.1f;
            }
            if(eyePos.y <= -3.68f){
                if(eyePos.x >= 0f && newX <= -19f - 1 + transform.GetComponentInChildren<FishEye>().getMountainXFromYRight(eyePos.y) && eyePos.x - 0.3f <= 1){
                    newX = -19f - 1 + transform.GetComponentInChildren<FishEye>().getMountainXFromYRight(eyePos.y);
                }
                if(eyePos.x <= 0f && newX >= 19f + 1 - transform.GetComponentInChildren<FishEye>().getMountainXFromYRight(eyePos.y) && eyePos.x +  0.3f >= -1){
                    newX = 19f + 1 - transform.GetComponentInChildren<FishEye>().getMountainXFromYRight(eyePos.y);
                }
            }
                
            
            fishEdges[i] = new Vector3(newX, newY, 0f);
        }   
    }

    public void DeleteFish(){
        Destroy(gameObject);
    }
}
