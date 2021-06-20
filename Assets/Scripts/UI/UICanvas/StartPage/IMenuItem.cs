using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuItem {
    void Show();
    void Hide(System.Action afterHideCallBack);
}
