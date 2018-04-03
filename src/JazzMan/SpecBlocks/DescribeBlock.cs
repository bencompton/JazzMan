using System;
using System.Collections.Generic;

namespace JazzMan.SpecBlocks
{
    public class DescribeBlock
    {
        public DescribeBlock(string description)
        {
            Description = description;
            ChildDescribeBlocks = new List<DescribeBlock>();
            ChildItBlocks = new List<ItBlock>();
            ChildBeforeEachBlocks = new List<BeforeEachBlock>();
            ChildBeforeAllBlocks = new List<BeforeAllBlock>();
        }

        public DescribeBlock(string description, Action func)
            : this(description)
        {
            Func = func;
        }

        public DescribeBlock ParentDescribeBlock { get; set; }
        public string Description { get; set; }
        public Action Func { get; set; }
        public List<DescribeBlock> ChildDescribeBlocks { get; set; }
        public List<ItBlock> ChildItBlocks { get; set; }
        public List<BeforeEachBlock> ChildBeforeEachBlocks { get; set; }
        public List<BeforeAllBlock> ChildBeforeAllBlocks { get; set; }
    }
}
