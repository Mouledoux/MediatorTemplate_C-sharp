/*  Created by: Eric Mouleoux
 *  Contact: EricMouledoux@gmail.com
 *  
 *  Summary:
 *  
 *  
 *  Usage:
 *  
 *  
 *  Notes:
 *  While intented to be used with Unity3D(5.5.0f3),
 *  a non-Unity version will be commented out below.
 *  
 *  !!!NOTICE!!!: Make sure that ONLY Unity, OR non-Unity is active at once.
 */


#region Unity

public class Mediator : UnityEngine.MonoBehaviour
{
    #region Singleton

    private static Mediator instance;

    public static Mediator GetInstance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Mediator>();

            return instance;
        }
    }

    private void InitializeSingleton()
    {
        if (GetInstance != this)
            Destroy(gameObject);
    }

    #endregion

    #region Callback Delegate

    /// <summary>
    /// Callback delegate to be used by all subscribers
    /// </summary>
    /// <param name="data">Predefined data Packet to act as potential arguments for subscription methods</param>
    public delegate void Callback(Packet data);

    #endregion

    private Mediator() { }

    private void Awake()
    {
        InitializeSingleton();
    }
        
    public System.Collections.Generic.Dictionary<string, Callback> subscriptions =
        new System.Collections.Generic.Dictionary<string, Callback>();
}



/// <summary>
/// 
/// </summary>
public class Publisher : UnityEngine.MonoBehaviour
{
    protected static void NotifySubscribers(string message, Packet data)
    {
        Mediator.Callback cb;

        if (Mediator.GetInstance.subscriptions.TryGetValue(message, out cb))
        {
            cb.Invoke(data);
        }
    }
}


/// <summary>
/// 
/// </summary>
public class Subscriber : UnityEngine.MonoBehaviour
{
    protected static void Subscribe(string message, Mediator.Callback callback)
    {
        Mediator.Callback cb;

        if (Mediator.GetInstance.subscriptions.TryGetValue(message, out cb))
        { 
            Mediator.GetInstance.subscriptions[message] += callback;
        }

        else
        {
            Mediator.GetInstance.subscriptions.Add(message, callback);
        }
    }

    protected static void Unsubscribe(string message, Mediator.Callback callback)
    {
        Mediator.Callback cb;

        if (Mediator.GetInstance.subscriptions.TryGetValue(message, out cb))
        {
            Mediator.GetInstance.subscriptions[message] -= callback;

            if (Mediator.GetInstance.subscriptions.TryGetValue(message, out cb))
            {
                Mediator.GetInstance.subscriptions.Remove(message);
            }
        }
    }
}

#endregion




#region non-Unity
#endregion




/// !!! ATTENTION !!!
/// Everything below this point is used by both Unity, and non-Unity
/// DO NOT MODIFY ANY OF THE HELPER CLASSES, OR THEIR METHODS
#region Helper Classes

/// <summary>
/// Collecion of basic variable to be sent via Callbacks
/// </summary>
public class Packet
{
    /// <summary>
    /// All of the intigers to be used
    /// </summary>
    private int[] ints;
    /// <summary>
    /// All of the boolens to be used
    /// </summary>
    private bool[] bools;
    /// <summary>
    /// All of the floating point numbers to be used
    /// </summary>
    private float[] floats;
    /// <summary>
    /// All of the text strings to be used
    /// </summary>
    private string[] strings;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Packet()
    {
        this.ints = new int[0];
        this.bools = new bool[0];
        this.floats = new float[0];
        this.strings = new string[0];
    }

    /// <summary>
    /// Constructor to ensure all arrays are set
    /// </summary>
    /// <param name="ints">Predefined array of ints</param>
    /// <param name="bools">Predefined array of bools</param>
    /// <param name="floats">Predefined array of floats</param>
    /// <param name="strings">Predefined array of strings</param>
    public Packet(int[] ints, bool[] bools, float[] floats, string[] strings)
    {
        this.ints = ints;
        this.bools = bools;
        this.floats = floats;
        this.strings = strings;
    }
}

#endregion