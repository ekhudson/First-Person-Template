using UnityEngine;
using System;
   
[System.Serializable]
public class EventBase
{
    // Time at which the event occured.
    public readonly float Time = 0f;

    // Location at which the event occurred.
    public readonly Vector3 Place = Vector3.zero;

    public readonly object Sender = null;
	
    // Create a new event.
    protected EventBase() : this(Vector3.zero, null)
    {
    }

    // Create a new event at a location in the world.
    protected EventBase(Vector3 place, object sender)
    {
       // Time = UnityEngine.Time.fixedTime; 
		Time = 0.2f;
        Place = place;
		Sender = sender;
    }
}
