using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{

    
    private int radius = 30;
 
    public float moveSpeed = 5f;


    float X;

    float Y;




    public float Yvel;
    public float Xvel;

    public int groupnumber;




    // Start is called before the first frame update

    void Start()
    {
   Xvel   = Random.Range(-1f,1f);
   Yvel = Random.Range(-1f, 1f);

       // Xvel = 1f;
      //  Yvel = 1f;
    }

    // Update is called once per frame
    void Update()
    {
       
       

    }

 
    void OnTriggerEnter2D(Collider2D collider)
    {
    
        
      

    }


        public double getDistance(GameObject neighbour)
    {

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        double xdifference = Mathf.Abs(sprite.transform.position.x - neighbour.transform.position.x);

        double ydifference = Mathf.Abs(sprite.transform.position.y - neighbour.transform.position.y);

        double Mdistance = xdifference + ydifference;

        return Mdistance;

    }
    

    public void initialise()
    {
  


    }


    public void MoveForward(double minspeed = 3, double maxspeed = 5)
    {

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Vector3 newposition = transform.position +  new Vector3(Xvel, Yvel, 0) * Time.deltaTime;

        transform.position = newposition;


        var speed = getSpeed();


        if(speed > maxspeed)
        {

            Xvel = (Xvel / speed) * (float)maxspeed;
            Yvel = (Yvel / speed) * (float)maxspeed;
        }

        else if(speed < minspeed) 
        {

            Xvel = (Xvel / speed) * (float)minspeed;
            Yvel = (Yvel / speed) * (float)minspeed;


        }
      
        // var speed = 3;

        //  Xvel = Xvel * speed;


        if (float.IsNaN(Xvel))
            Xvel = 0;

        if (float.IsNaN(Yvel))
            Yvel = 0;


        rotate();
    }


    public void WrapXY()
    {

        
        var camera = Camera.main;
        Vector3 camerarelativeposition = transform.position - camera.transform.position;

        var relativex = camerarelativeposition.x;
        var relativey = camerarelativeposition.y;

        var camerawidth = camera.orthographicSize * 2f * camera.aspect;
        var cameraheight = camera.orthographicSize * 2f;

        Vector3 newposition = transform.position; // Start with current position

        if (relativex < -camerawidth / 2f)
            newposition.x += camerawidth;
        else if (relativex > camerawidth / 2f)
            newposition.x -= camerawidth;

        if (relativey < -cameraheight / 2f)
            newposition.y += cameraheight;
        else if (relativey > cameraheight / 2f)
            newposition.y -= cameraheight;

        transform.position = newposition;
        






    }


    private float getSpeed()
    {

        var vx = Xvel;

        var vy = Yvel;

        return Mathf.Sqrt(vx * vx  + vy * vy);


  

    }



    private void rotate()
    {
       


        float angle = Mathf.Atan2(Yvel , Xvel ) * Mathf.Rad2Deg + 270;

        // Apply rotation to the object
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


    }


}
