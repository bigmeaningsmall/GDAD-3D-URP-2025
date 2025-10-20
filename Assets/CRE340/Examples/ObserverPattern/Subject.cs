using UnityEngine;
using System;

public class Subject : MonoBehaviour
{
    public event Action ThingHappened ;

    void Start(){
        DoThing();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DoThing();
    }
    
    public void DoThing()
    {
        ThingHappened?.Invoke(); // Raise the event if there are any subscribers
    }
    

}

