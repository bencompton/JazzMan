using System;
using JazzMan.SpecBlocks;

namespace JazzMan
{
    public interface ISpecFinder
    {
        DescribeBlock Find(string methodName);
        void HandleDescribeMethodCall(string description, Action func);
        void HandleItMethodCall(string description, Action func);
        void HandleBeforeEachMethodCall(Action func);
        void HandleBeforeAllMethodCall(Action func);
    }
}