using System;

namespace Content
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class IncompatibleWithAttribute : Attribute
    {
        public Type[] Types { get; }

        public IncompatibleWithAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}