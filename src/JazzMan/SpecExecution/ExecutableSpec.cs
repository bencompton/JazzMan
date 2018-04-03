using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JazzMan.SpecBlocks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace JazzMan.SpecExecution
{
    public class ExecutableSpec : ITestCaseData
    {
        private ItBlock _itBlock;

        public string TestName
        {
            get
            {
                var sb = new StringBuilder();
                var whitespace = new StringBuilder();

                foreach (DescribeBlock describeBlock in ParentDescribeBlocks)
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
            foreach (DescribeBlock parentDescribeBlock in ParentDescribeBlocks)
            {
                foreach (BeforeEachBlock beforeEach in parentDescribeBlock.ChildBeforeEachBlocks)
                {
                    beforeEach.Func();
                }

                foreach (BeforeAllBlock beforeAll in parentDescribeBlock.ChildBeforeAllBlocks)
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

        public object[] Arguments
        {
            get { return new object[0]; }
        }

        public IPropertyBag Properties { get; }

        public string Description
        {
            get { return null; }
        }

        public Type ExpectedException
        {
            get { return null; }
        }

        public string ExpectedExceptionName
        {
            get { return null; }
        }

        public bool Explicit
        {
            get { return false; }
        }

        public object ExpectedResult { get; }

        public bool HasExpectedResult
        {
            get { return false; }
        }

        public string IgnoreReason
        {
            get { return null; }
        }

        public bool Ignored
        {
            get { return false; }
        }

        public object Result
        {
            get { return null; }
        }

#endregion

    }
}