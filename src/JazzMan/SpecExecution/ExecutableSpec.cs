using System;
using System.Collections.Generic;
using System.Text;
using JazzMan.SpecBlocks;
using NUnit.Framework.Interfaces;

namespace JazzMan.SpecExecution
{
    public class ExecutableSpec : ITestCaseData
    {
        private readonly ItBlock _itBlock;

        public string TestName
        {
            get
            {
                var sb = new StringBuilder();
                var whitespace = new StringBuilder();

                foreach (var describeBlock in ParentDescribeBlocks)
                {
                    //Skip the top level describe block (method-level) so the method name won't get repeated in the spec name
                    if (describeBlock.ParentDescribeBlock == null) continue;
                    sb.Append(whitespace);
                    sb.Append(describeBlock.Description);
                    if (whitespace.Length == 0) whitespace.Append("\n");
                    whitespace.Append("   ");
                }

                sb.Append(whitespace);
                sb.Append(_itBlock.Description);

                return sb.ToString();
            }
        }

        public RunState RunState { get; }

        public List<DescribeBlock> ParentDescribeBlocks
        {
            get
            {
                var parentDescribeBlocks = new List<DescribeBlock>();
                var parentDescribeBlock = _itBlock.ParentDescribeBlock;

                while (parentDescribeBlock != null)
                {
                    parentDescribeBlocks.Insert(0, parentDescribeBlock);
                    parentDescribeBlock = parentDescribeBlock.ParentDescribeBlock;
                }

                return parentDescribeBlocks;
            }
        }

        public bool HasExecuted { get; private set; }

        public ExecutableSpec(ItBlock itBlock)
        {
            _itBlock = itBlock;
        }

        public void Execute()
        {
            foreach (var parentDescribeBlock in ParentDescribeBlocks)
            {
                foreach (var beforeEach in parentDescribeBlock.ChildBeforeEachBlocks)
                {
                    beforeEach.Func();
                }

                foreach (var beforeAll in parentDescribeBlock.ChildBeforeAllBlocks)
                {
                    if (!beforeAll.HasExecuted)
                    {
                        beforeAll.Func();
                        beforeAll.HasExecuted = true;
                    }
                }
            }

            HasExecuted = true;
            _itBlock.Func();
        }

#region "Unused properties required to implement ITestCaseData"

        public object[] Arguments => new object[0];

        public IPropertyBag Properties { get; }

        public string Description => null;

        public Type ExpectedException => null;

        public string ExpectedExceptionName => null;

        public bool Explicit => false;

        public object ExpectedResult { get; }

        public bool HasExpectedResult => false;

        public string IgnoreReason => null;

        public bool Ignored => false;

        public object Result => null;

        #endregion

    }
}