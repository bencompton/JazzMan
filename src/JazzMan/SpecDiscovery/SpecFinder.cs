using System;
using System.Reflection;

using JazzMan.SpecBlocks;

namespace JazzMan
{
    public class SpecFinder : ISpecFinder
    {
        private readonly JazzManSpec _specToSearch;
        private DescribeBlock _currentDescribeBlock;
        private IRuntimeContext _runtimeContext;

        public SpecFinder(JazzManSpec specToSearch, IRuntimeContext runtimeContext)
        {
            _specToSearch = specToSearch;
            _runtimeContext = runtimeContext;

            specToSearch.describe += this.HandleDescribeMethodCall;
            specToSearch.it += this.HandleItMethodCall;
            specToSearch.beforeEach += this.HandleBeforeEachMethodCall;
            specToSearch.beforeAll += this.HandleBeforeAllMethodCall;
        }

        public DescribeBlock Find(string methodName)
        {
            DescribeBlock methodDescribeBlock = new DescribeBlock(methodName.Replace("_", " "));

            foreach (MethodInfo method in _specToSearch.GetType().GetMethods())
            {
                if (method.Name == methodName)
                {
                    _currentDescribeBlock = methodDescribeBlock;
                    method.Invoke(_specToSearch, null);
                    EvaluateChildDescribeBlocks(methodDescribeBlock);
                }
            }
            
            return methodDescribeBlock;
        }

        private void EvaluateChildDescribeBlocks(DescribeBlock parentDescribeBlock)
        {
            if (!_runtimeContext.TestsExecuting)
            {
                foreach (DescribeBlock childDescribeBlock in parentDescribeBlock.ChildDescribeBlocks)
                {
                    _currentDescribeBlock = childDescribeBlock;
                    childDescribeBlock.Func();
                    EvaluateChildDescribeBlocks(childDescribeBlock);
                }
            }
        }

        public void HandleDescribeMethodCall(string description, Action func)
        {
            if (!_runtimeContext.TestsExecuting)
            {
                var newDescribeBlock = new DescribeBlock(description, func);
                newDescribeBlock.ParentDescribeBlock = _currentDescribeBlock;
                _currentDescribeBlock.ChildDescribeBlocks.Add(newDescribeBlock);
            }
        }

        public void HandleItMethodCall(string description, Action func)
        {
            if (!_runtimeContext.TestsExecuting)
            {
                var newItBlock = new ItBlock(description, func);
                newItBlock.ParentDescribeBlock = _currentDescribeBlock;
                _currentDescribeBlock.ChildItBlocks.Add(newItBlock);
            }
        }

        public void HandleBeforeEachMethodCall(Action func)
        {
            if (!_runtimeContext.TestsExecuting)
            {
                var newBeforeEachBlock = new BeforeEachBlock(func);
                newBeforeEachBlock.ParentDescribeBlock = _currentDescribeBlock;
                _currentDescribeBlock.ChildBeforeEachBlocks.Add(newBeforeEachBlock);
            }
        }

        public void HandleBeforeAllMethodCall(Action func)
        {
            if (!_runtimeContext.TestsExecuting)
            {
                var newBeforeAllBlock = new BeforeAllBlock(func);
                newBeforeAllBlock.ParentDescribeBlock = _currentDescribeBlock;
                _currentDescribeBlock.ChildBeforeAllBlocks.Add(newBeforeAllBlock);
            }
        }
    }
}