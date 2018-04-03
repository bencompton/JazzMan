using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JazzMan.SpecExecution
{
    public class SpecExecutor : ISpecExecutor
    {
        private JazzManSpec _specClass;
        private IRuntimeContext _runtimeContext;

        public SpecExecutor(JazzManSpec specClass, IRuntimeContext runtimeContext)
        {
            _specClass = specClass;
            _runtimeContext = runtimeContext;
            specClass.it += SpecExecutionTriggered;
            specClass.describe += SpecExecutionTriggered;
            specClass.beforeAll += SpecExecutionTriggered;
            specClass.beforeEach += SpecExecutionTriggered;
        }

        private void SpecExecutionTriggered(Action dummy)
        {
            ExecuteCurrentlyRunningSpec(dummy.Method.Name);
        }

        private void SpecExecutionTriggered(string dummy, Action dummy1 = null)
        {
            ExecuteCurrentlyRunningSpec(dummy1.Method.Name);
        }

        private void ExecuteCurrentlyRunningSpec(string lambdaFunctionName)
        {
            string methodName = System.Text.RegularExpressions.Regex.Match(lambdaFunctionName, @"\<(.+)\>").Groups[1].Value;

            if (_runtimeContext.TestsExecuting)
            {
                List<ExecutableSpec> executableSpecs = _runtimeContext.GetSpecs(_specClass.GetType().Name, methodName);

                foreach (ExecutableSpec spec in executableSpecs)
                {
                    if (!spec.HasExecuted && spec.TestName == _runtimeContext.CurrentlyExecutingSpecName)
                    {
                        spec.Execute();
                    }
                }
            }
        }
    }
}