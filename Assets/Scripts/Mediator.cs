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
public sealed class Mediator
{
    /// !!! READ ME !!! ///
    /// The below code is a standard singleton except for 1 line which is Unity3D specific
    /// The line is in the GetInstance getter, and MUST be changed to work outside of Unity3D
    #region Singleton

    private Mediator() { }

    private static Mediator _instance;

    public static Mediator instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Mediator();                // Non-Unity Version, must be removed for Unity projects
            }

            return _instance;
        }
    }
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
    /// Base class for all entities that will be broadcasting
    /// </summary>
    public class Publisher : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects and should be removed
    {
        /// <summary>
        /// Checks to see if their are any Subscribers to the broadcasted message
        /// and invokes ALL callbacks associated with it
        /// </summary>
        /// <param name="message">The message to be broadcasted (case sensitive)</param>
        /// <param name="data">Packet of information to be used by ALL recieving parties</param>
        protected void NotifySubscribers(string message, Packet data)
        {
            // Temporary delegate container for modifying subscription delegates 
            Callback cb;

            // Check to see if the message has any valid subscriptions
            if (instance.subscriptions.TryGetValue(message, out cb))
            {
                // Invokes ALL associated delegates with the data Packet as the argument
                cb.Invoke(data);
            }
        }
    }

    /// <summary>
    /// Base class for all entities that will be listing for broadcasts
    /// </summary>
    public class Subscriber : UnityEngine.MonoBehaviour // <--- No inheritance is necessary for non-Unity projects and should be removed
    {
        /// <summary>
        /// Personal, internal record of all active subscriptions
        /// </summary>
       private System.Collections.Generic.Dictionary<string, Callback> totalSubscriptions =
            new System.Collections.Generic.Dictionary<string, Callback>();


        /// <summary>
        /// Links a custom delegate to a message in a SPECIFIC subscription dictionary
        /// </summary>
        /// <param name="container">Refrence to the dictionary of subscriptions we want to modify</param>
        /// <param name="message">The message to subscribe to (case sensitive)</param>
        /// <param name="callback">The delegate to be linked to the broadcast message</param>
        private void Subscribe(ref System.Collections.Generic.Dictionary<string, Callback> container, string message, Callback callback)
        {
            // Temporary delegate container for modifying subscription delegates 
            Callback cb;
            
            // Check to see if there is not already a subscription to this message
            if (!container.TryGetValue(message, out cb))
            {
                // If there is not, then make one with the message and currently empty callback delegate
                container.Add(message, cb);
            }

            /// If the subscription does already exist,
            /// then cb is populated with all associated delegates,
            /// if it does not, cb is empty.

            // Add the delegate to cb (new or populated)
            cb += callback;
            // Set the delegate linked to the message to cb
            container[message] = cb;
        }

        /// <summary>
        /// Links a custom delegate to a message that may be breadcasted via a Publisher
        /// </summary>
        /// <param name="message">The message to subscribe to (case sensitive)</param>
        /// <param name="callback">The delegate to be linked to the broadcast message</param>
        protected void Subscribe(string message, Callback callback)
        {
            // First, adds the subscription to the internaal records
            Subscribe(ref totalSubscriptions, message, callback);
            // Then, adds the subcription to the public records
            Subscribe(ref instance.subscriptions, message, callback);
        }


        /// <summary>
        /// Unlinks a custom delegate from a message in a SPECIFIC subscription dictionary
        /// </summary>
        /// <param name="container">Refrence to the dictionary of subscriptions we want to modify</param>
        /// <param name="message">The message to unsubscribe from (case sensitive)</param>
        /// <param name="callback">The delegate to be removed from the broadcast message</param>
        private void Unsubscribe(ref System.Collections.Generic.Dictionary<string, Callback> container, string message, Callback callback)
        {
            // Temporary delegate container for modifying subscription delegates 
            Callback cb;

            // Check to see if there is a subscription to this message
            if (container.TryGetValue(message, out cb))
            {
                /// If the subscription does already exist,
                /// then cb is populated with all associated delegates.
                /// Otherwise nothing will happen
                
                // Remove the selected delegate from the callback
                cb -= callback;

                // Check the modified cb to see if there are any delegates left
                if (cb == null)
                {   
                    // If tere is not, then remove the subscription completely
                    container.Remove(message);
                }
                else
                {
                    // If there are some left, reset the callback to the now lesser cb
                    container[message] = cb;
                }
            }
        }

        /// <summary>
        /// Unlinks a custom delegate from a message that may be breadcasted via a Publisher
        /// </summary>
        /// <param name="message">The message to unsubscribe from (case sensitive)</param>
        /// <param name="callback">The delegate to be unlinked from the broadcast message</param>
        protected void Unsubscribe(string message, Callback callback)
        {
            // First, remove the subscription from the internal records
            Unsubscribe(ref totalSubscriptions, message, callback);
            // Then, remove the subcription from the public records
            Unsubscribe(ref instance.subscriptions, message, callback);
        }

        /// <summary>
        /// Unlinks all (local) delegates from given broadcast message
        /// </summary>
        /// <param name="message">The message to unsubscribe from (case sensitive)</param>
        protected void UnsubcribeAllFrom(string message)
        {
            Unsubscribe(message, totalSubscriptions[message]);
        }
        
        /// <summary>
        /// Unlinks all (local) delegates from every (local) broadcast message
        /// </summary>
        protected void UnsubscribeAll()
        {
            foreach(string message in totalSubscriptions.Keys)
            {
                Unsubscribe(ref instance.subscriptions, message, totalSubscriptions[message]);
            }
            totalSubscriptions.Clear();
        }
    }
}

/// !!! ATTENTION !!!
///
#region Packet Class

/// <summary>
/// Collecion of basic variables to be sent via delegates
/// </summary>
public class Packet
{
    /// <summary>
    /// All of the intigers to be used
    /// </summary>
    public int[] ints;
    /// <summary>
    /// All of the boolens to be used
    /// </summary>
    public bool[] bools;
    /// <summary>
    /// All of the floating point numbers to be used
    /// </summary>
    public float[] floats;
    /// <summary>
    /// All of the text strings to be used
    /// </summary>
    public string[] strings;

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

#endregion Packet Class