using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boid_Controller : MonoBehaviour
{

    public GameObject[] boidinstances;

    public Camera camera;

    public GameObject Asteroid_Spawner;


    // Start is called before the first frame update
    void Start()
    {



        boidinstances = GameObject.FindGameObjectsWithTag("Boid");

       
      
  //      camera = GetComponent<Camera>();

     //   float width = camera.pixelWidth;

   //     float height = camera.pixelHeight;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    public void Move()
    {

        foreach (var boid in boidinstances)
        {
            if (boid != null)
            {

                (double cohesionXvel, double cohesionYvel) = Cohesion(boid, 5, .03);
                (double alignXvel, double alignYvel) =       Alignment(boid, 50, .01);
                (double seperateXvel, double seperateYvel) = Seperation(boid, 1, .1);
                (double avoidXvel, double avoidYvel) =       Avoid(boid, 2, .100);

                //  Boid boidvalues = boid.GetComponent<Boid>();
                double boidsXvel = boid.GetComponent<Boid>().Xvel;
                boidsXvel += cohesionXvel + alignXvel + seperateXvel + avoidXvel;

                boid.GetComponent<Boid>().Xvel = (float)boidsXvel;

                double boidsYvel = boid.GetComponent<Boid>().Yvel;
                boidsYvel += cohesionYvel + alignYvel + seperateYvel + avoidYvel;

                boid.GetComponent<Boid>().Yvel = (float)boidsYvel;


            }

        }

        foreach (var boid in boidinstances)
        {
            if (boid != null)
            {



                boid.GetComponent<Boid>().MoveForward();

                boid.GetComponent<Boid>().WrapXY();

            }

        }

    }
   


    private (double xVel , double yVel) Cohesion(GameObject boid,double distance, double power)
    {


        var localboids = getlocalBoids(boidinstances,boid, distance,true);

        double meanX = localboids.Sum(obj => obj.transform.position.x) / localboids.Count();

        double meanY = localboids.Sum(obj => obj.transform.position.y) / localboids.Count();


        double CenterX = meanX - boid.transform.position.x;

        double CenterY = meanY - boid.transform.position.y;

        return (CenterX * power, CenterY * power);
        


    }


    private (double XVel, double yVel) Alignment(GameObject boid,double distance, double power)

    {
        var boidscript = boid.GetComponent<Boid>();

        var localboids = getlocalBoids(boidinstances, boid, distance ,true);

        double meanXvel = localboids.Sum(obj => obj.GetComponent<Boid>().Xvel) / localboids.Count();

        double meanYvel = localboids.Sum(obj => obj.GetComponent<Boid>().Yvel) / localboids.Count();

        double dXvel = meanXvel - boidscript.Xvel;


        double dYvel = meanYvel - boidscript.Yvel;

        return (dXvel * power, dYvel * power);




    }


    private (double xVel , double yVel) Seperation(GameObject boid, double distance, double power)
    {

        var localboids = getlocalBoids(boidinstances, boid, distance,false);

        (double sumCLosenessX, double sumClosenessY) = (0, 0);

        foreach ( var localboid in localboids)
        {

            double closeness = distance - boid.GetComponent<Boid>().getDistance(localboid);

            sumCLosenessX += (boid.transform.position.x - localboid.transform.position.x) * closeness;
            sumClosenessY += (boid.transform.position.y - localboid.transform.position.y) * closeness;

        }

        return (sumCLosenessX * power, sumClosenessY * power);

    }


    
    private (double xVel, double yVel) Avoid(GameObject boid, double distance, double power)
    {

       var allasteroids =  GameObject.FindGameObjectsWithTag("Asteroid");

        var localasteroids = getlocalAsteroids(allasteroids, boid, distance, false);

        (double sumClosenessX, double sumClosenessY) = (0, 0);
        for (int i = 0; i <  localasteroids.Count; i++)
        {
            
            double distanceAway = boid.GetComponent<Boid>().getDistance(localasteroids[i]);
            if (distanceAway < distance)
            { 
                double closeness = distance - distanceAway;
                sumClosenessX += (boid.transform.position.x - localasteroids[i].transform.position.x) * closeness;
                sumClosenessY += (boid.transform.position.y - localasteroids[i].transform.position.y) * closeness;
            }
        }
        return (sumClosenessX * power, sumClosenessY * power);
    }


    





    public List<GameObject> getlocalBoids(GameObject[] allboids ,GameObject boid, double distance,bool samegrouponly)
    {
        List<GameObject> localboids = new List<GameObject>();
      
        foreach (GameObject Boid in allboids)
        {




            double xdifference = Mathf.Abs(boid.transform.position.x - Boid.transform.position.x);

            double ydifference = Mathf.Abs(boid.transform.position.y - Boid.transform.position.y);

            double Mdistance = xdifference + ydifference;



            if(samegrouponly == true && boid.GetComponent<Boid>().groupnumber == Boid.GetComponent<Boid>().groupnumber  && Mdistance < distance)
            {
                localboids.Add(Boid);
            }


          else  if (Mdistance < distance && samegrouponly == false )
            {



                localboids.Add(Boid);
            }

            
        }



        return localboids;


    }



    public List<GameObject> getlocalAsteroids(GameObject[] allAsteroids, GameObject boid, double distance, bool cheesecakes)
    {
        List<GameObject> localasteroids = new List<GameObject>();

        foreach (GameObject asteroid in allAsteroids)
        {


            double xdifference = Mathf.Abs(boid.transform.position.x - asteroid.transform.position.x);

            double ydifference = Mathf.Abs(boid.transform.position.y - asteroid.transform.position.y);

            double Mdistance = xdifference + ydifference;


             if (Mdistance < distance )
            {

                localasteroids.Add(asteroid);
            }

        }

        return localasteroids;

    }







}
