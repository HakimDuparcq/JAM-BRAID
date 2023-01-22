using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletState
{
    BeforeSpawn,
	Spawn,
    Moving,
	DeadFrame,
    Dead,
    Destroy
}

public enum SpwanerState
{
	Spawn,
	None
}

[System.Serializable]
public class Spawners
{
    public GameObject spawner;
    public GameObject prefabBullet;

}


public class SpawnerManager : MonoBehaviour
{
    public Spawners spawners;

	List<PointInTime> pointsInTime = new List<PointInTime>();

	public float spawningTime = 2f;
	public float compteur;

    private void Start()
    {
		TimeManager.EventStopRewind.AddListener(StopRewinding);

    }

    private void OnDisable()
    {
		TimeManager.EventStopRewind.RemoveListener(StopRewinding);

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
			compteur= (compteur-Time.fixedDeltaTime)%spawningTime;
			pointsInTime.RemoveAt(0);
		}
		

	}

	void Record()
	{
		
		if (pointsInTime.Count > Mathf.Round(TimeManager.recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

        if (compteur>= spawningTime)
        {
			compteur = 0;
			Spawning();
			pointsInTime.Insert(0, new PointInTime(SpwanerState.Spawn));
		}
        else
        {
			compteur += Time.fixedDeltaTime;
			pointsInTime.Insert(0, new PointInTime(SpwanerState.None));
		}
		
		//animator.enabled = true;
	}

	void Spawning()
    {
		GameObject go = Instantiate(spawners.prefabBullet, spawners.spawner.gameObject.transform);
		go.transform.localScale = new Vector3(3, 3, 3);
    }

	void StopRewinding()
    {
		//compteur = (compteur + (int)(Mathf.Abs(compteur)+spawningTime)) % spawningTime;
		compteur = (compteur + 100) % spawningTime;
	}

}
