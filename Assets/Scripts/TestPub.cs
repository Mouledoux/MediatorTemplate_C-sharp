using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPub : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Mediator.instance.NotifySubscribers(collision.gameObject.GetInstanceID().ToString(), new Packet());
    }
}
