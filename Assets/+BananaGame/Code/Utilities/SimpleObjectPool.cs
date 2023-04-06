using System.Collections.Generic;
using UnityEngine;

namespace BananaSoup.Utilities
{
    public class SimpleObjectPool : MonoBehaviour
    {
        [SerializeField, Tooltip("The GameObject to pool.")]
        private GameObject objectToPool;

        [SerializeField, Tooltip("The maximum amount of objects to pool.")]
        private int amountToPool = 5;

        private List<GameObject> pooledObjects = null;

        public List<GameObject> PooledObjects
        {
            get { return pooledObjects; }
        }

        // Start is called before the first frame update
        private void Start()
        {
            SetupPool();
        }

        /// <summary>
        /// Method used in Start() to setup the object pool.
        /// </summary>
        private void SetupPool()
        {
            pooledObjects = new List<GameObject>();
            GameObject temp;

            for ( int i = 0; i < amountToPool; i++ )
            {
                temp = Instantiate(objectToPool);
                temp.SetActive(false);
                pooledObjects.Add(temp);
            }
        }

        /// <summary>
        /// Method to get a GameObject from the pool.
        /// </summary>
        /// <returns>The next inactive GameObject in hierarchy from the pool.</returns>
        private GameObject GetObjectFromPool()
        {
            for ( int i = 0; i < amountToPool; i++ )
            {
                if ( !pooledObjects[i].activeInHierarchy )
                {
                    return pooledObjects[i];
                }
            }

            return null;
        }

        /// <summary>
        /// This method can be used to spawn an object from the set pool.
        /// </summary>
        /// <param name="position">The Vector3 position where the object should be spawned.</param>
        /// <param name="rotation">The Quaternion rotation of the object when it is spawned.</param>
        public void SpawnObject(Vector3 position, Quaternion rotation)
        {
            GameObject toSpawn = GetObjectFromPool();

            if ( toSpawn != null )
            {
                toSpawn.transform.position = position;
                toSpawn.transform.rotation = rotation;
                toSpawn.SetActive(true);
            }
        }
    }
}
