using System.Collections.Generic;
using JazzMan.SpecBlocks;

namespace JazzMan.SpecExecution
{
    public interface IDescribeBlockToExecutableSpecConverter
    {
        List<ExecutableSpec> GetExecutableSpecs(DescribeBlock describeBlock);
    }
}