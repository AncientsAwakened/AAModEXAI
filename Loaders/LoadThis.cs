using System;

namespace AAModEXAI.Loaders
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LoadThis : Attribute
    {
        public bool clientOnly;

        public LoadThis(bool clientOnly = false)
        {
            this.clientOnly = clientOnly;
        }
    }
}
