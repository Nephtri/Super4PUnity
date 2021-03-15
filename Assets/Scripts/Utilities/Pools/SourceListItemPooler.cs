// using UnityEngine;

// public class SourceListItemPooler : ObjectPooler 
// {
//     public static SourceListItemPooler SP;

//     override protected void Awake()
//     {
//         //Singleton pattern
//         if (SP == null) {
//             if(transform.parent.gameObject) {
//                 DontDestroyOnLoad(transform.parent.gameObject);
//             } else {
//                 DontDestroyOnLoad(gameObject);
//             }

//             SP = this;
//         }
//         else if (SP != this) {
//             Destroy(gameObject);
//         }

//         base.Awake();
//     }

//     public GameObject GetPooledObject(Transform parent = null)
//     {
//         var listItem = base.GetPooledObject("SourceListItem", parent).GetComponent<SourceListItem>();
//         return listItem.gameObject;
//     }
// }