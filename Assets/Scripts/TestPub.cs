using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPub : Mediator.Publisher
{
    public string msg;

	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("broadcasting");
            NotifySubscribers(msg, new Packet());
        }
	}
}
