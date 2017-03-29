using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.Patterns.MVC
{
    [Serializable]
    public abstract class BaseController<M> where M : BaseModel
    {
        protected M model;

        public virtual void Init(M model)
        {
            this.model = model;
            model.Init();
        }
    }
}