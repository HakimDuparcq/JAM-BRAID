using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerTime : MonoBehaviour
{
    List<PointInTime> pointsInTime;

    public Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public Animator animator;


	void Start()
    {
        pointsInTime = new List<PointInTime>();

		TimeManager.EventStartRewind.AddListener(StartRewind);
		TimeManager.EventStartRewind.AddListener(StopRewind);
	}

    private void OnDisable()
    {
		TimeManager.EventStartRewind.RemoveListener(StartRewind);
		TimeManager.EventStartRewind.RemoveListener(StopRewind);
	}

    void Update()
	{

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
			animator.enabled = false;
			transform.position = pointsInTime[0].pos;
			transform.rotation = pointsInTime[0].rot;
			rb.velocity = pointsInTime[0].velocity;
			spriteRenderer.sprite = pointsInTime[0].sprite;
			pointsInTime.RemoveAt(0);
		}
		else
		{
			
			StopRewind();
		}

	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(TimeManager.recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.velocity, spriteRenderer.sprite));

		animator.enabled = true;
	}

	public void StartRewind()
	{
		rb.bodyType = RigidbodyType2D.Static;
	}

	public void StopRewind()
	{

		rb.bodyType = RigidbodyType2D.Dynamic;
	}
}
