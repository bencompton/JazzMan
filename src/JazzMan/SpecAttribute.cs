using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace JazzMan
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SpecAttribute : TestCaseSourceAttribute
    {
        public SpecAttribute([CallerMemberName] string methodName = null) : this(new RuntimeContext(), methodName) { }

        public SpecAttribute(IRuntimeContext runtimeContext, [CallerMemberName] string methodName = null)
            : base("GetAndCacheSpecListForClass")
        {
            runtimeContext.CurrentlyEvaluatingMethodName = methodName;
        }
    }
}