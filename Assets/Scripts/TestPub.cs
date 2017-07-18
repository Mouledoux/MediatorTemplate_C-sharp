using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPub : MonoBehaviour
{
    public string msg;

	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("broadcasting");
            Mediator.instance.NotifySubscribers(msg, new Packet());
        }
	}
}
