using UnityEngine;

namespace Yd.Gameplay.Behavior
{
    public class PlayerCharacterSpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject playerCharacterPrefab;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }

        public GameObject Spawn()
        {
            var go = Instantiate(playerCharacterPrefab, transform);
            go.SetActive(true);
            return go;
        }
    }
}