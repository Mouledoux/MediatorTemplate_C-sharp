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
 *  While intented to be used with Unity3D,
 *  non-Unity versions will be commented out below Unity specific code
 */

 /// <summary>
 /// Base class for all mediation.
 /// </summary>
public sealed class Mediator : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects and should be removed
{
    /// !!! READ ME !!! ///
    /// The below code is a standard singleton except for 1 line which is Unity3D specific
    /// The line is in the GetInstance getter, and MUST be changed to work outside of Unity3D
    #region Singleton

    private Mediator() { }

    private static Mediator instance;

    public static Mediator GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Mediator>();    // Unity Version, must be removed for non-Unity projects
                //instance = new Mediator();                // Non-Unity Version, must be removed for Unity projects
            }

            return instance;
        }
    }

    /// !!! READ ME !!!
    /// Both of the methods below:
    /// InitializeSingleton and Awake,
    /// are only used by Unity3D,
    /// and should be removed otherwise
    #region Unity Specific Singleton Methods

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
    #endregion Unity Specific Singleton Methods

    #endregion Singleton

    /// <summary>
    /// Callback delegate to be used by all subscribers
    /// </summary>
    /// <param name="data">Predefined data Packet to act as potential arguments for subscriptions</param>
    public delegate void Callback(Packet data);

    /// <summary>
    /// Dictionary of subscription strings and associated delegate callbacks
    /// </summary>
    private System.Collections.Generic.Dictionary<string, Callback> subscriptions =
        new System.Collections.Generic.Dictionary<string, Callback>();

    

    /// !!! READ ME !!! ///
    /// Below are the base classes for the Publishers, and Subscribers
    /// Any entity that will be broadcasting messages MUST inherit from Mediator.Publisher
    /// Any entity that will be listining for messages MUST inherit from Mediator.Subscriber
    /// 
    /// Because Publisher and Subscriber are classes that must be inherited,
    /// no single entity can be both a Publisher, AND a Subscriber
    /// 
    /// Example: A button that opens a menu, and changes color
    /// The button would be a Publisher, and the menu a Subscriber.
    /// The color change should be handeled by the button internally, OR
    /// the button would have a 'color' element that is a Subscriber
    /// the button is NOT both a Publisher AND Subscriber

    /// <summary>
    /// 
    /// </summary>
    public class Publisher : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects and should be removed
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


    public class Subscriber : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects and should be removed
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
/// Collecion of basic variables to be sent via delegates
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
    /// To be used to send empty packets
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