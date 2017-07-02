using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputsObservable
{ 
    void keysChanged(Dictionary<string, KeyCode> keys);
}
