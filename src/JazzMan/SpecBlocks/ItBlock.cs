using System;

namespace JazzMan.SpecBlocks
{
    public class ItBlock
    {
        public string Description { get; set; }
        public Action Func { get; set; }
        public DescribeBlock ParentDescribeBlock { get; set; }

        public ItBlock(string description, Action func)
        {
            Description = description;
            Func = func;
        }
    }
}
