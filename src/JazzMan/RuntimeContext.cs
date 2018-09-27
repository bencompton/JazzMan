using System;
using System.Collections.Generic;
using JazzMan.SpecExecution;

namespace JazzMan
{
    public class RuntimeContext : IRuntimeContext
    {
        public bool TestsExecuting
        {
            get
            {
                try
                {
                    var dummy = NUnit.Framework.TestContext.CurrentContext.Test.Name;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }

        public string CurrentlyExecutingSpecName => NUnit.Framework.TestContext.CurrentContext.Test.Name;

        public string CurrentlyEvaluatingMethodName
        {
            get => (string)AppDomain.CurrentDomain.GetData("CurrentlyEvaluatingMethodName");
            set => AppDomain.CurrentDomain.SetData("CurrentlyEvaluatingMethodName", value);
        }

        public List<ExecutableSpec> GetSpecs(string className, string methodName)
        {
            return AppDomain.CurrentDomain.GetData(
                string.Format(
                    "Specs_For_{0}_{1}",
                    className,
                    methodName
                )
            ) as List<ExecutableSpec>;            
        }

        public void AddSpecs(string className, string methodName, List<ExecutableSpec> executableSpecs)
        {
            AppDomain.CurrentDomain.SetData(
                String.Format(
                    "Specs_For_{0}_{1}",
                    className,
                    methodName
                ),
                executableSpecs
            );
        }

    }
}
