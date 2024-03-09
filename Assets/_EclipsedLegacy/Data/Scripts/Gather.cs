using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace in3d.EL.GameLogic
{
    public interface IGather
    {
        void Collect();
    }

    public abstract class Gather : MonoBehaviour, IGather
    {
        public bool hasCollectionTime => collectionTime > 0f;
        protected float collectionTime = 0f;
        public Gather(){}

        public virtual void Collect(){}
    }
}
