using System;
using JazzMan.SpecBlocks;

namespace JazzMan
{
    public class SpecFinder : ISpecFinder
    {
        private readonly JazzManSpec _specToSearch;
        private DescribeBlock _currentDescribeBlock;
        private readonly IRuntimeContext _runtimeContext;

        public SpecFinder(JazzManSpec specToSearch, IRuntimeContext runtimeContext)
        {
            _specToSearch = specToSearch;
            _runtimeContext = runtimeContext;

            specToSearch.describe += HandleDescribeMethodCall;
            specToSearch.it += HandleItMethodCall;
            specToSearch.beforeEach += HandleBeforeEachMethodCall;
            specToSearch.beforeAll += HandleBeforeAllMethodCall;
        }

        public DescribeBlock Find(string methodName)
        {
            var methodDescribeBlock = new DescribeBlock(methodName.Replace("_", " "));

            foreach (var method in _specToSearch.GetType().GetMethods())
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
                foreach (var childDescribeBlock in parentDescribeBlock.ChildDescribeBlocks)
                {
                    _currentDescribeBlock = childDescribeBlock;
                    childDescribeBlock.Func();
                    EvaluateChildDescribeBlocks(childDescribeBlock);
                }
            }
        }

        public void HandleDescribeMethodCall(string description, Action func)
        {
            if (_runtimeContext.TestsExecuting) return;
            var newDescribeBlock =
                new DescribeBlock(description, func) {ParentDescribeBlock = _currentDescribeBlock};
            _currentDescribeBlock.ChildDescribeBlocks.Add(newDescribeBlock);
        }

        public void HandleItMethodCall(string description, Action func)
        {
            if (_runtimeContext.TestsExecuting) return;
            var newItBlock = new ItBlock(description, func) {ParentDescribeBlock = _currentDescribeBlock};
            _currentDescribeBlock.ChildItBlocks.Add(newItBlock);
        }

        public void HandleBeforeEachMethodCall(Action func)
        {
            if (_runtimeContext.TestsExecuting) return;
            var newBeforeEachBlock = new BeforeEachBlock(func) {ParentDescribeBlock = _currentDescribeBlock};
            _currentDescribeBlock.ChildBeforeEachBlocks.Add(newBeforeEachBlock);
        }

        public void HandleBeforeAllMethodCall(Action func)
        {
            if (_runtimeContext.TestsExecuting) return;
            var newBeforeAllBlock = new BeforeAllBlock(func) {ParentDescribeBlock = _currentDescribeBlock};
            _currentDescribeBlock.ChildBeforeAllBlocks.Add(newBeforeAllBlock);
        }
    }
}