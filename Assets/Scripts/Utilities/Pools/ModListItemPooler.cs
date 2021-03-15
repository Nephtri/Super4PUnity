// using UnityEngine;

// public class ModListItemPooler : ObjectPooler 
// {
//     public static ModListItemPooler MP;

//     override protected void Awake()
//     {
//         //Singleton pattern
//         if (MP == null) {
//             if(transform.parent.gameObject) {
//                 DontDestroyOnLoad(transform.parent.gameObject);
//             } else {
//                 DontDestroyOnLoad(gameObject);
//             }

//             MP = this;
//         }
//         else if (MP != this) {
//             Destroy(gameObject);
//         }

//         base.Awake();
//     }

//     public GameObject GetPooledObject(Transform parent = null)
//     {
//         var listItem = base.GetPooledObject("ModListItem", parent).GetComponent<ModListItem>();
//         return listItem.gameObject;
//     }
// }