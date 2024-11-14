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
public enum FactoryType
{
    Add,
    Minus,
    Divide,
    Multiply,
}

public static class State 
{
    public static Mode mode = Mode.None;
    public static FactoryType factoryType = FactoryType.Add;
}
