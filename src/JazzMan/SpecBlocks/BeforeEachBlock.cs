using System;

namespace JazzMan.SpecBlocks
{
    public class BeforeEachBlock
    {
        public Action Func { get; set; }
        public DescribeBlock ParentDescribeBlock { get; set; }

        public BeforeEachBlock(Action func)
        {
            Func = func;
        }
    }
}
