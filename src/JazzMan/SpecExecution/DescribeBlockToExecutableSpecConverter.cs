using System.Collections.Generic;
using JazzMan.SpecBlocks;
using NUnit.Framework;

namespace JazzMan.SpecExecution
{
    public class DescribeBlockToExecutableSpecConverter : IDescribeBlockToExecutableSpecConverter
    {
        public List<ExecutableSpec> GetExecutableSpecs(DescribeBlock describeBlock)
        {
            var executableTests = new List<ExecutableSpec>();

            foreach (ItBlock itBlock in describeBlock.ChildItBlocks)
            {
                executableTests.Add(new ExecutableSpec(itBlock));
            }

            foreach (DescribeBlock childDescribeBlock in describeBlock.ChildDescribeBlocks)
            {
                executableTests.AddRange(GetExecutableSpecs(childDescribeBlock));
            }

            return executableTests;
        }
    }
}
