using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSub : Mediator.Subscriber
{
    public string msg;
    Mediator.Callback onNotify;
    Mediator.Callback onNotify2;

    private void Start()
    {
        onNotify += HelloWorld;
        Subscribe(msg, onNotify);

        onNotify2 += ByeWorld;
        Subscribe(msg, onNotify2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            Unsubscribe(msg, onNotify);
        if (Input.GetKeyDown(KeyCode.I))
            Unsubscribe(msg, onNotify2);

        if (Input.GetKeyDown(KeyCode.Z))
            UnsubscribeAll();
    }

    void HelloWorld(Packet p)
    {
        transform.Translate(Vector3.up * name.Length);
    }
    void ByeWorld(Packet p)
    {
        transform.Rotate(Vector3.right);
    }
}
