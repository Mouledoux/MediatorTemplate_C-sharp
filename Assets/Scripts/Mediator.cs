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
 *  non-Unity version will be commented out below Unity specific code
 */

public class Mediator : UnityEngine.MonoBehaviour
{
    #region Singleton

    private Mediator() { }

    private static Mediator instance;

    public static Mediator GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Mediator>();    // Unity Version
                //instance = new Mediator();                // Non-Unity Version
            }

            return instance;
        }
    }

    #region Unity Specific Methods
    /// !!! READ ME !!!
    /// Both of the methods below:
    /// InitializeSingleton and Awake,
    /// are only used by Unity3D,
    /// and should be removed otherwise

        /// !!! ATTENTION !!! ///
    //* <--- Remove one '/' to disable the code below

    /// <summary>
    /// Used in the Unity 'Awake' method to remove duplicate Mediators from the scene
    /// </summary>
    private void InitializeSingleton()
    {
        // Checks if the static instace is this instance
        if (GetInstance != this)
        {
            // And self-destructs if not
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        // Initializes on Awake to remove duplicates before anything else
        InitializeSingleton();
    }
    //*/// End of Unity specific methods
    #endregion Unity Specific Methods

    #endregion Singleton

    /// <summary>
    /// Callback delegate to be used by all subscribers
    /// </summary>
    /// <param name="data">Predefined data Packet to act as potential arguments for subscriptions</param>
    public delegate void Callback(Packet data);

    private System.Collections.Generic.Dictionary<string, Callback> subscriptions =
        new System.Collections.Generic.Dictionary<string, Callback>();


    public class Publisher : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects
    {
        protected void NotifySubscribers(string message, Packet data)
        {
            Callback cb;

            if (GetInstance.subscriptions.TryGetValue(message, out cb))
            {
                cb.Invoke(data);
            }
        }
    }


    public class Subscriber : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects
    {
        protected void Subscribe(string message, Callback callback)
        {
            Callback cb;

            if (!GetInstance.subscriptions.TryGetValue(message, out cb))
            {
                GetInstance.subscriptions.Add(message, cb);
            }

            cb += callback;
            GetInstance.subscriptions[message] = cb;
        }

        protected void Unsubscribe(string message, Callback callback)
        {
            Callback cb;

            if (GetInstance.subscriptions.TryGetValue(message, out cb))
            {
                cb -= callback;

                if (cb == null)
                {
                    GetInstance.subscriptions.Remove(message);
                }
                else
                {
                    GetInstance.subscriptions[message] = cb;
                }
            }
        }
    }
}

/// !!! ATTENTION !!!
/// DO NOT MODIFY ANY OF THE CLASSES, OR THEIR METHODS BELOW THIS POINT
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

#endregion Helper Classes