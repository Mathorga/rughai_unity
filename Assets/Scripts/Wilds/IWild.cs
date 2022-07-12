using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWild : ILiving {
    public enum State {
        Idle,
        Wander,
        Chase,
        Dead
    };
    
    public State state {
        get;
        set;
    }
}
