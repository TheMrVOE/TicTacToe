using System;
using UnityEngine;

namespace Tools.Patterns.MVC
{
    public class BaseView<M, C> : MonoBehaviour
    where M : BaseModel, new()
    where C : BaseController<M>, new()
    {
        public M model;
        //was protected 
        public C Controller;

        public virtual void Awake()
        {
            model = new M();
            Controller = new C();
            Controller.Init(model);
        }
    }
}