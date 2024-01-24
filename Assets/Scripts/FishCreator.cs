using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCreator : MonoBehaviour
{
    public GameObject fish;
    public GameObject fishEye;
    public float num = 1.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1) && transform.position.x < 0){
            Debug.Log("Fish shooting from left pond.");
            
            Shoot();
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.position.x > 0){
            Debug.Log("Fish shooting from right pond.");
            if(num > 0){
                num = -1 * num;
            }
            
            Shoot();
        }
    }

    public void InstantiateFish(){
        GameObject c = Instantiate(fish, transform.position, transform.rotation);
        Instantiate(fishEye, new Vector3(transform.position.x + num, transform.position.y + 0.35f,0f), transform.rotation,c.transform);
    }

    public void Shoot(){
        InstantiateFish();
        
    }
}
