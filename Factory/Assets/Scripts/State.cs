using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode 
{
    None,
    Factory,
    Foundry,
    Delete,
} 

public static class State 
{
    public static Mode mode = Mode.None;
}
