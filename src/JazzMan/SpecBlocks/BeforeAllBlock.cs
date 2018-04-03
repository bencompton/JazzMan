using System;

namespace JazzMan.SpecBlocks
{
    public class BeforeAllBlock
    {
        public Action Func { get; set; }
        public DescribeBlock ParentDescribeBlock { get; set; }
        public bool HasExecuted { get; set; }

        public BeforeAllBlock(Action func)
        {
            Func = func;
        }
    }
}
