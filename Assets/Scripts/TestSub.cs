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
            UnsubcribeAllFrom(msg);
    }

    void HelloWorld(Packet p)
    {
        print(name + " Hello World");
    }
    void ByeWorld(Packet p)
    {
        print(name + " Bye World");
    }

    private void OnDestroy()
    {
        UnsubscribeAll();
    }
}
