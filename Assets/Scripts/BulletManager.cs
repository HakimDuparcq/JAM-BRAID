using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
	public float bulletSpeed=0.08f;
	public List<PointInTime> pointsInTime = new List<PointInTime>();


    private void Start()
    {
		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, BulletState.Spawn));
	}



	void FixedUpdate()
	{
		if (TimeManager.isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
            if (pointsInTime[0].bulletState == BulletState.Spawn)
            {
				Destroy(gameObject);
            }
            else if (pointsInTime[0].bulletState == BulletState.DeadFrame)
            {
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
				gameObject.GetComponent<BoxCollider2D>().enabled = true;
				transform.position = pointsInTime[0].pos;
				transform.rotation = pointsInTime[0].rot;
			}
            else if (pointsInTime[0].bulletState == BulletState.Dead)
            {

            }
            else if (pointsInTime[0].bulletState == BulletState.Moving)
            {
				transform.position = pointsInTime[0].pos;
				transform.rotation = pointsInTime[0].rot;
			}
            else
            {

            }

			
			pointsInTime.RemoveAt(0);
		}


	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(TimeManager.recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}


		//is dead stay here
		if (pointsInTime.Count > 0 && pointsInTime[0].bulletState == BulletState.DeadFrame || pointsInTime.Count > 0 && pointsInTime[0].bulletState == BulletState.Dead)
		{
			pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, BulletState.Dead));
		}
		else if(gameObject.GetComponent<SpriteRenderer>().enabled)
		{
			pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, BulletState.Moving));
		}
        else
        {
			pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, BulletState.Dead));
		}


        if (pointsInTime.Count > 0 && pointsInTime[0].bulletState == BulletState.Moving || pointsInTime.Count > 0 && pointsInTime[0].bulletState == BulletState.Spawn)
        {
			transform.position += new Vector3(bulletSpeed, 0, 0);
		}



        if (pointsInTime[pointsInTime.Count-1].bulletState==BulletState.Dead)
        {
			Destroy(gameObject);
        }


	}




    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.GetComponent<PlayerMovement>())  //Touch player
        {
			//Debug.Log("player Touch");
		}
        else if (collision.gameObject.GetComponent<BulletManager>()) //Self
        {
			//Debug.Log("Self Touch");
        }
        else  //Touch map
        {
			//Debug.Log("Touch" + name);
			pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, BulletState.DeadFrame));
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
    }

}
