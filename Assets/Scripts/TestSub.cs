using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSub : MonoBehaviour
{
    public string msg;
    Mediator.Callback onNotify;
    Mediator.Callback onNotify2;

    Mediator.Subscriptions subs = new Mediator.Subscriptions();

    private void Start()
    {
        print("Start");

        onNotify += HelloWorld;
        subs.Subscribe(msg, onNotify);

        onNotify2 += ByeWorld;
        subs.Subscribe(msg, onNotify2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            subs.Unsubscribe(msg, onNotify);
        if (Input.GetKeyDown(KeyCode.I))
            subs.Unsubscribe(msg, onNotify2);

        if (Input.GetKeyDown(KeyCode.Z))
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        subs.UnsubscribeAll();
    }

    void HelloWorld(Packet p)
    {
        transform.Translate(Vector3.up);
    }
    void ByeWorld(Packet p)
    {
        transform.Rotate(Vector3.right);
    }
}
