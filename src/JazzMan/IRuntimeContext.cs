using System.Collections.Generic;
using JazzMan.SpecExecution;

namespace JazzMan
{
    public interface IRuntimeContext
    {
        bool TestsExecuting { get; }
        string CurrentlyExecutingSpecName { get; }
        string CurrentlyEvaluatingMethodName { get; set; }
        List<ExecutableSpec> GetSpecs(string className, string methodName);
        void AddSpecs(string className, string methodName, List<ExecutableSpec> executableSpecs);
    }
}