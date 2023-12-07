using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnOrder : MonoBehaviour {
    public List<CarController> carList = new List<CarController>();
    public List<GameObject> checkPointNumber = new List<GameObject>();        // The checkpoint use by each car
    public List<bool> canMove = new List<bool>();        // The checkpoint use by each car


    public List<int> listNumber = new List<int>();        // list of check use actually


    public List<float> _timer = new List<float>();                                // timer between two spawn
    public float refTimeBetweenTwoRespawn = 1;
    public int _counter = 0;





    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach(GameObject obj in gos){
            _timer.Add(refTimeBetweenTwoRespawn);
        }
    }

    public void MCR_AddCarToRespawnList(CarController newCar,GameObject checkpoint){
        if(carList.Count == 0){
            carList.Insert(0, newCar);  }
        else{
            carList.Insert(carList.Count, newCar);}

        if (checkPointNumber.Count == 0){
            checkPointNumber.Insert(0, checkpoint);}
        else{
            checkPointNumber.Insert(checkPointNumber.Count, checkpoint);}

        if (canMove.Count == 0){
            canMove.Insert(0, true);}
        else{
            canMove.Insert(canMove.Count, true);}

        returnCarCanMove(newCar, checkpoint, canMove.Count - 1);

        int tmpNum = int.Parse(checkpoint.name);
        bool b_AddValue = true;
        if (listNumber.Count == 0)
        {
            foreach(int num in listNumber){
                if (num == tmpNum)
                    b_AddValue = false; 
            }
            if(b_AddValue)
                listNumber.Insert(0,int.Parse(checkpoint.name) );
        }
        else
        {
           foreach (int num in listNumber)
            {
                if (num == tmpNum)
                    b_AddValue = false;
            }
            if (b_AddValue)
                listNumber.Insert(listNumber.Count, int.Parse(checkpoint.name));
        }

       
       
    }


    public bool returnIfTheCarCanRespawn(CarController newCar, GameObject checkpoint){
        bool result = false;
        for (var i = 0; i < carList.Count; i++)
        {
            if (checkPointNumber[i] == checkpoint && carList[i] != newCar)
            {
                result = false;
                break;
            }
            else if (checkPointNumber[i] == checkpoint && 
                     carList[i] == newCar /*&& 
                     _timer[int.Parse(checkpoint.name)-1] == 0*/)
            {
                result = true;
                carList.RemoveAt(i);
                checkPointNumber.RemoveAt(i);
                canMove.RemoveAt(i);
                _timer[int.Parse(checkpoint.name) - 1] = refTimeBetweenTwoRespawn;
                break;
            }
                
        }
        return result;
    }


    public bool carCanRespawn(CarController newCar){
            bool collisionCheck = true;

        int cmpt = 0;
        Collider[] col = Physics.OverlapBox(newCar.transform.position, new Vector3(.1F, .5f, .5F), newCar.transform.rotation);

        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag == "Car" && col[i].gameObject != newCar.gameObject){
                //Debug.Log("collisionCheck : " + col[i].gameObject.name);
                cmpt++;         // A car is near.  
            }
               
        }
        if (cmpt == 0)
            collisionCheck = false;

        //Debug.Log("carCanRespawn");
        //Debug.Log("collisionCheck : " + collisionCheck );
        return collisionCheck;
    }

    void Update()
    {
        for (int i = 0; i < listNumber.Count; i++)
        {

        }
    }


    void returnCarCanMove(CarController newCar,GameObject checkpoint,int posNumber)
    {
        bool b_allowToMove = true;
        if(listNumber.Count == 0){
            canMove[posNumber] = true;  
        }

        for (int i = 0; i < listNumber.Count; i++)
        {
            //Debug.Log("here : " + listNumber[i] + " : " + int.Parse(checkpoint.name));
            if (listNumber[i] == int.Parse(checkpoint.name))
            {
                //Debug.Log("here 2");
                b_allowToMove = false;
                canMove[posNumber] = b_allowToMove;
                break;
            }
        }
    }


    public bool carCanGoToRespawnPosition(CarController newCar)
    {

        for (int i = 0; i < carList.Count; i++)
        {
            if(carList[i] == newCar && canMove[i] == true){
                return true;
            }
        }
        return false;
    }

    public bool UpdateCarToRespawn(CarController newCar){
        int currentCheckpoint = 0;
        //Debug.Log("Update");
        for (int i = 0; i < carList.Count; i++)
        {
            if (carList[i] == newCar)
            {
                for (int j = 0; j < listNumber.Count; j++)
                {
                    if (listNumber[j] == int.Parse(checkPointNumber[i].name)){
                        currentCheckpoint = listNumber[j];
                        listNumber.RemoveAt(j);
                    }
                        
                }
                carList.RemoveAt(i);
                checkPointNumber.RemoveAt(i);
                canMove.RemoveAt(i);
            }
        }

        //for (int j = 0; j < listNumber.Count; j++)
        //{
            for (int i = 0; i < carList.Count; i++)
            {
                if (currentCheckpoint == int.Parse(checkPointNumber[i].name))
                {
                    canMove[i] = true;
                listNumber.Add(currentCheckpoint);
                    break;
                }
            }
        //}

        return true;
    }

}
