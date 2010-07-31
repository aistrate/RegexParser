using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RegexParser.Tests.Helpers
{
    public class MemoryProfiler
    {
        public MemoryProfiler()
        {
            string callingAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            bytesInAllHeapsPC = new PerformanceCounter(
                                        ".NET CLR Memory",
                                        "# Bytes in all Heaps",
                                        callingAssemblyName,
                                        true);

            Reset();
        }

        private PerformanceCounter bytesInAllHeapsPC;

        public long StartingValue { get; private set; }

        public long CurrentValue
        {
            get { return bytesInAllHeapsPC.RawValue; }
        }

        public long DeltaValue
        {
            get { return CurrentValue - StartingValue; }
        }

        public void Reset()
        {
            CollectGC();
            StartingValue = bytesInAllHeapsPC.RawValue;
        }

        public void CollectGC()
        {
            GC.Collect();
        }

        public static MemoryProfiler StartNew()
        {
            return new MemoryProfiler();
        }
    }
}
