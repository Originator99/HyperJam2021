using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateFactory {
    private readonly CameraStateFollowing.Factory _followingStateFactory;

    public CameraStateFactory(CameraStateFollowing.Factory followingStateFactory) {
        _followingStateFactory = followingStateFactory;
    }
    
    public CameraState CreateFactory(CameraState.State state) {
        switch(state) {
            case CameraState.State.Following:
               return _followingStateFactory.Create();
        }

        throw Assert.CreateException();
    }
}
