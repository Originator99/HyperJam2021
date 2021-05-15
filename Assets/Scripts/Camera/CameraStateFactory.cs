using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateFactory {
    private readonly CameraStateFollowing.Factory _followingStateFactory;
    private readonly CameraStateZoom.Factory _zoomStateFactory;

    public CameraStateFactory(CameraStateFollowing.Factory followingStateFactory, CameraStateZoom.Factory zoomStateFactory) {
        _followingStateFactory = followingStateFactory;
        _zoomStateFactory = zoomStateFactory;
    }
    
    public CameraState CreateFactory(CameraState.State state) {
        switch(state) {
            case CameraState.State.Following:
               return _followingStateFactory.Create();
            case CameraState.State.Zoom:
                return _zoomStateFactory.Create();
        }

        throw Assert.CreateException();
    }
}
