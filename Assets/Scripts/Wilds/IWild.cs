using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWild : ILiving {
    public enum Mode {
        Idle,
        Wander,
        Chase,
        Dead
    };

    public enum State {
        Idle,
        Wander,
        Chase,
        Dead
    };

    public Mode mode {
        get;
        set;
    }

    public State state {
        get;
        set;
    }
}
