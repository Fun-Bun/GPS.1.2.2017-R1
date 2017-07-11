using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformScript : MonoBehaviour
{
	[HideInInspector]
	public float halfLength;

	public List<PlatformReceiverScript> whoStepOnMe;

	[Header("Jump Points")]
	public JumpPoint[] jumpPoints = new JumpPoint[2];
	public float distanceFromEdge;

	[Header("Platform Hierarchy")]
	public int degree;
	public bool isGround = false;
	public List<PlatformScript> top;
	public List<PlatformScript> bottom;
	public bool ClickMe;

	void Start()
	{
		halfLength = GetComponent<BoxCollider2D>().bounds.extents.x;
	}

	void OnValidate()
	{
		if(isGround)
		{
			degree = 0;
			bottom.Clear();
		}
	}
	
	public void SetDegree(int i, PlatformScript lastTop)
	{
		degree = i;
		foreach(PlatformScript p in top)
		{
			if(!p.bottom.Contains(lastTop)) p.bottom.Add(lastTop);
			p.SetDegree(i + 1, p);
		}
	}

//	public List<PlatformScript> GetListTo(PlatformScript target)
//	{
//		int degreeDiff = target.degree - this.degree;
//		Debug.Log(degreeDiff);
//		int maxCycle = Mathf.FloorToInt(Mathf.Abs(degreeDiff) / 2);
//		Debug.Log(maxCycle);
//
//		List<List<PlatformScript>> listStart = new List<List<PlatformScript>>();
//		List<List<PlatformScript>> listEnd = new List<List<PlatformScript>>();
//		
//		GetChildren(listStart, this, (maxCycle % 2 == 0 ? maxCycle : maxCycle - 1), degreeDiff >= 0);
//		GetChildren(listEnd, target, (maxCycle % 2 == 0 ? maxCycle : maxCycle - 1), degreeDiff < 0);
//
//		if(maxCycle % 2 != 0)
//		{
//			GetChildren(listStart, this, maxCycle, degreeDiff >= 0);
//			GetChildren(listStart, target, maxCycle - 1, degreeDiff < 0);
//		}
//		else
//		{
//			GetChildren(listStart, this, maxCycle, degreeDiff >= 0);
//			GetChildren(listEnd, target, maxCycle, degreeDiff < 0);
//		}
//
//		//Testing
//		Debug.Log("This");
//		foreach(PlatformScript p in listStart)
//		{
//			Debug.Log(p.gameObject.name);
//		}
//		//Testing
//		Debug.Log("Target");
//		foreach(PlatformScript p in listEnd)
//		{
//			Debug.Log(p.gameObject.name);
//		}
//
//		return null;
//	}
//
//	void GetChildren(List<List<PlatformScript>> list, PlatformScript currentPlatform, int cycle, bool goingUp)
//	{
//		if(cycle < 0) return;
//		List<PlatformScript> checkList = (goingUp ? currentPlatform.top : currentPlatform.bottom);
//		foreach(PlatformScript p in checkList)
//		{
//			if(cycle > 1)
//				GetChildren(list, p, cycle - 1, goingUp);
//			else if(!list.Contains(p))
//				list.Add(p);
//		}
//	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.GetComponent<PlatformReceiverScript>())
		{
			PlatformReceiverScript receiver = other.GetComponent<PlatformReceiverScript>();
			if(!whoStepOnMe.Contains(receiver))
			{
				whoStepOnMe.Add(receiver);
				receiver.platform = this;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.GetComponent<PlatformReceiverScript>())
		{
			PlatformReceiverScript receiver = other.GetComponent<PlatformReceiverScript>();
			if(whoStepOnMe.Contains(receiver))
			{
				whoStepOnMe.Remove(receiver);
				receiver.platform = null;
			}
		}
	}
}
