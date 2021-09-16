using System;

namespace AAModEXAI.Loaders
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnloadThis : Attribute
    {
        public bool clientOnly;

        public UnloadThis(bool clientOnly = false)
        {
            this.clientOnly = clientOnly;
        }
    }
}
