using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace in3d.EL
{
    public class PropHold : ValidatedMonoBehaviour
    {
        [SerializeField] Transform holdPosition;
        [SerializeField] private Transform currentProp = null;

        [Header("Testing")]
        public GameObject propPrefab;

        public void PickUp(){
            currentProp = Instantiate(propPrefab, holdPosition.position, Quaternion.identity, holdPosition).transform;
            currentProp.localScale = Vector3.one * 0.5f;
        }

        public void Drop(){
            GameObject toDestroy = currentProp.gameObject;
            currentProp = null;
            Destroy(toDestroy);
        }
    }
}
