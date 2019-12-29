using System;

namespace MVC.Models
{
    public class BaseModel
    {
        protected void CheckView<T>(T road)
        {
            if (road == null)
            {
                throw new Exception("Don't find View in prefab");
            }
        }
    }
}