// using UnityEngine;

// public class PoseListItemPooler : ObjectPooler 
// {
//     // override protected void Awake()
//     // {
//     //     foreach(var x in pool)
//     //     {
//     //         x.GetComponent<PoseListItem>
//     //     }
//     //     //Singleton pattern
//     //     if (MP == null) {
//     //         if(transform.parent.gameObject) {
//     //             DontDestroyOnLoad(transform.parent.gameObject);
//     //         } else {
//     //             DontDestroyOnLoad(gameObject);
//     //         }

//     //         MP = this;
//     //     }
//     //     else if (MP != this) {
//     //         Destroy(gameObject);
//     //     }

//     //     base.Awake();
//     // }

//     public GameObject GetPooledObject(Transform parent = null)
//     {
//         var listItem = base.GetPooledObject("ModListItem", parent).GetComponent<ModListItem>();
//         return listItem.gameObject;
//     }


//     override public virtual PoseListItem GetPooledObject(string objName, Transform parent = null)
//     {
//         GameObject obj = null;

//         foreach(var tempObj in pool)
//         {
//             if(!tempObj.activeSelf) { 
//                 obj = tempObj;
//                 break;
//             }
//         }

//         if(obj == null && expandable) 
//         {
//             obj = Instantiate(pooledObj);
//             pool.Add(obj);
//         }

//         if(obj != null) {
//             obj.SetActive(true);

//             if(parent != null) { 
//                 obj.transform.SetParent(parent);
//                 obj.transform.localPosition = Vector2.zero;
//                 obj.transform.localScale = Vector3.one;
//             }
//         }
        
//         obj.name = objName;

//         return obj;
//     }
// }