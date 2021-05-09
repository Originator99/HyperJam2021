using UnityEngine;
using Zenject;

public class PrefabFactory : PlaceholderFactory<GameObject, Transform> {

}

//public class DIPrefabFactory :IFactory<GameObject, Transform> {
//    private readonly DiContainer _container;

//    public DIPrefabFactory(DiContainer container) {
//        _container = container;
//    }

//    public Transform Create(GameObject prefab, Transform parent) {
//        return _container.InstantiatePrefab(prefab, parent).transform;
//    }

//    public Transform Create(GameObject param) {
//        throw new System.NotImplementedException();
//    }
//}
