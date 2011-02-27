using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace UnitTesting
{
    public enum MemoryCounterType
    {
        NrOfBytesInAllHeaps,
        Gen0HeapSize,
        Gen1HeapSize,
        Gen2HeapSize,
        LargeObjectHeapSize,
        NrOfTotalCommittedBytes,
        NrOfTotalReservedBytes,
        NrOfInducedGC,
    }

    public class MemoryProfiler
    {
        public MemoryProfiler()
            : this(MemoryCounterType.NrOfBytesInAllHeaps) { }

        public MemoryProfiler(MemoryCounterType counterType)
        {
            string callingAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            performanceCounter = new PerformanceCounter(
                                        ".NET CLR Memory",
                                        counterNames[counterType],
                                        callingAssemblyName,
                                        true);

            Reset();
        }

        private PerformanceCounter performanceCounter;

        public long StartingValue { get; private set; }

        public long CurrentValue
        {
            get { return performanceCounter.RawValue; }
        }

        public long DeltaValue
        {
            get { return CurrentValue - StartingValue; }
        }

        public void Reset()
        {
            CollectGC();
            StartingValue = performanceCounter.RawValue;
        }

        public void CollectGC()
        {
            GC.Collect();
        }

        public static MemoryProfiler StartNew()
        {
            return new MemoryProfiler();
        }

        public static MemoryProfiler StartNew(MemoryCounterType counterType)
        {
            return new MemoryProfiler(counterType);
        }

        private Dictionary<MemoryCounterType, string> counterNames =
            new Dictionary<MemoryCounterType, string>()
            {
                { MemoryCounterType.NrOfBytesInAllHeaps, "# Bytes in all Heaps" },
                { MemoryCounterType.Gen0HeapSize, "Gen 0 heap size" },
                { MemoryCounterType.Gen1HeapSize, "Gen 1 heap size" },
                { MemoryCounterType.Gen2HeapSize, "Gen 2 heap size" },
                { MemoryCounterType.LargeObjectHeapSize, "Large Object Heap size" },
                { MemoryCounterType.NrOfTotalCommittedBytes, "# Total committed Bytes" },
                { MemoryCounterType.NrOfTotalReservedBytes, "# Total reserved Bytes" },
                { MemoryCounterType.NrOfInducedGC, "# Induced GC" },
            };
    }
}
