using System.Collections.Generic;
using JazzMan.SpecBlocks;

namespace JazzMan.SpecExecution
{
    public class DescribeBlockToExecutableSpecConverter : IDescribeBlockToExecutableSpecConverter
    {
        public List<ExecutableSpec> GetExecutableSpecs(DescribeBlock describeBlock)
        {
            var executableTests = new List<ExecutableSpec>();

            foreach (var itBlock in describeBlock.ChildItBlocks)
            {
                executableTests.Add(new ExecutableSpec(itBlock));
            }

            foreach (var childDescribeBlock in describeBlock.ChildDescribeBlocks)
            {
                executableTests.AddRange(GetExecutableSpecs(childDescribeBlock));
            }

            return executableTests;
        }
    }
}
