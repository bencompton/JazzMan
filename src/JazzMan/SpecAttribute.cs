using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace JazzMan
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SpecAttribute : TestCaseSourceAttribute
    {
        private IRuntimeContext _runtimeContext;

        public SpecAttribute([CallerMemberName] string methodName = null) : this(new RuntimeContext(), methodName) { }

        public SpecAttribute(IRuntimeContext runtimeContext, [CallerMemberName] string methodName = null)
            : base("GetAndCacheSpecListForClass")
        {
            _runtimeContext = runtimeContext;
            _runtimeContext.CurrentlyEvaluatingMethodName = methodName;
        }
    }
}