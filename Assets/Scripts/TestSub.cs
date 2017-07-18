using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSub : MonoBehaviour
{
    Mediator.Callback onNotify;
    Mediator.Subscriptions subs = new Mediator.Subscriptions();

    private void Start()
    {
        onNotify += HelloWorld;
        subs.Subscribe(gameObject.GetInstanceID().ToString(), onNotify);
    }

    private void OnDestroy()
    {
        subs.UnsubscribeAll();
    }

    void HelloWorld(Packet p)
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
}
