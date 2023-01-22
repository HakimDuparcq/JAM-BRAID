using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PointInTime
{
    public Vector3 pos;
    public Quaternion rot;
	public Vector2 velocity;
    public Sprite sprite;

	public SpwanerState spwanerState;
	public BulletState bulletState;

	public PointInTime()
	{

	}

	public PointInTime(Vector3 _pos, Quaternion _rot, Vector2 _velocity , Sprite _spr)
    {
        pos = _pos;
        rot = _rot;
		velocity = _velocity;
        sprite = _spr;
    }


	public PointInTime(SpwanerState _spwanerState)
	{
		spwanerState = _spwanerState;
	}

	public PointInTime(Vector3 _pos, Quaternion _rot, BulletState _bulletState)
	{
		pos = _pos;
		rot = _rot;
		bulletState = _bulletState;
	}


}

public class TimeManager : MonoBehaviour
{
	public static bool isRewinding = false;

	public static float recordTime = 5f;

	List<PointInTime> pointsInTime = new List<PointInTime>();

	public static UnityEvent EventStartRewind = new UnityEvent();
	public static UnityEvent EventStopRewind= new UnityEvent();

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			StartRewind();
		if (Input.GetKeyUp(KeyCode.Return))
			StopRewind();
	}

	void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			
			pointsInTime.RemoveAt(0);
		}
		else
		{

			StopRewind();
		}

	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime());

		
	}

	public void StartRewind()
	{
		isRewinding = true;
		EventStartRewind.Invoke();
		AudioManager.instance.ChangePitch("Theme",-1f);
	}

	public void StopRewind()
	{
		isRewinding = false;
		EventStopRewind.Invoke();
		AudioManager.instance.ChangePitch("Theme", 1f);

	}
}
