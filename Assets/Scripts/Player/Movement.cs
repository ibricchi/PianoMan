﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
	public Rigidbody2D rb;
	public Animator anim;
	public float speed = 200f;

	private Vector2 dir = Vector2.zero;
	private bool lastRight = true;

	private bool isSitting = true;
	private bool isIdleStanding = false;
	private float idleStandingTime = 0f;


	public void FixedUpdate()
	{
		dir = Vector2.zero;

		if (Input.GetKey(KeyCode.W)) dir.y += 1;
		if (Input.GetKey(KeyCode.A)) dir.x -= 1;
		if (Input.GetKey(KeyCode.S)) dir.y -= 1;
		if (Input.GetKey(KeyCode.D)) dir.x += 1;

		dir = Vector2.ClampMagnitude(dir, 1);

		rb.velocity = dir * speed * Time.fixedDeltaTime;

		anim.SetFloat("speed", rb.velocity.sqrMagnitude);

		if (dir.x != 0) lastRight = dir.x > 0;
		anim.SetBool("right", lastRight);

		if(anim.GetFloat("speed") < 0.1 && !isSitting && !isIdleStanding)
		{
			isIdleStanding = true;
			idleStandingTime = 0f;
		}

		if(isIdleStanding)
		{
			idleStandingTime += Time.fixedDeltaTime;
			if(idleStandingTime > 1f)
			{
				anim.SetTrigger("sit");
				isIdleStanding = false;
				isSitting = true;
			}
		}
		
		if((isSitting || isIdleStanding) && anim.GetFloat("speed") > 0.1)
		{
			isIdleStanding = false;
			isSitting = false;
		}

	}
}
