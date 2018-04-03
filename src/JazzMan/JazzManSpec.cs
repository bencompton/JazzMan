using System;
using System.Collections.Generic;
using JazzMan.SpecBlocks;
using JazzMan.SpecExecution;

namespace JazzMan
{
    public class JazzManSpec
    {
        private readonly IRuntimeContext _runtimeContext;
        private readonly ISpecExecutor _specExecutor;
        private readonly ISpecFinder _specFinder;
        private readonly IDescribeBlockToExecutableSpecConverter _describeBlockToExecutableSpecConverter;

        public delegate void SpecBlockMethodWithDescription(string description, Action func);
        public delegate void SpecBlockMethodWithoutDescription(Action func);

        public SpecBlockMethodWithDescription describe;
        public SpecBlockMethodWithDescription it;
        public SpecBlockMethodWithoutDescription beforeEach;
        public SpecBlockMethodWithoutDescription beforeAll;

        public JazzManSpec()
        {
            _describeBlockToExecutableSpecConverter = new DescribeBlockToExecutableSpecConverter();
            _runtimeContext = new RuntimeContext();
            _specFinder = new SpecFinder(this, _runtimeContext);
            _specExecutor = new SpecExecutor(this, _runtimeContext);
        }

        public JazzManSpec(IRuntimeContext runtimeContext, ISpecExecutor specExecutor, ISpecFinder specFinder, IDescribeBlockToExecutableSpecConverter describeBlockToExecutableSpecConverter)
        {
            _runtimeContext = runtimeContext;
            _specExecutor = specExecutor;
            _specFinder = specFinder;
            _describeBlockToExecutableSpecConverter = describeBlockToExecutableSpecConverter;
        }

        public List<ExecutableSpec> GetAndCacheSpecListForClass
        {
            get
            {
                DescribeBlock mainDescribeBlock = _specFinder.Find(_runtimeContext.CurrentlyEvaluatingMethodName);
                var executableSpecs = _describeBlockToExecutableSpecConverter.GetExecutableSpecs(mainDescribeBlock);
                _runtimeContext.AddSpecs(this.GetType().Name, _runtimeContext.CurrentlyEvaluatingMethodName, executableSpecs);

                return executableSpecs;
            }
        }
    }
}