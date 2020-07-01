namespace ReMarkable.NET.Unix.Driver.Performance
{
    internal class NetDeviceInfo
    {
        public long RxBytes { get; }
        public long RxPackets { get; }
        public long RxErrs { get; }
        public long RxDrop { get; }
        public long RxFifo { get; }
        public long RxFrame { get; }
        public long RxCompressed { get; }
        public long RxMulticast { get; }
        public long TxBytes { get; }
        public long TxPackets { get; }
        public long TxErrs { get; }
        public long TxDrop { get; }
        public long TxFifo { get; }
        public long TxFrame { get; }
        public long TxCompressed { get; }
        public long TxMulticast { get; }

        public NetDeviceInfo(long rxBytes, long rxPackets, long rxErrs, long rxDrop, long rxFifo, long rxFrame, long rxCompressed, long rxMulticast, long txBytes, long txPackets, long txErrs, long txDrop, long txFifo, long txFrame, long txCompressed, long txMulticast)
        {
            RxBytes = rxBytes;
            RxPackets = rxPackets;
            RxErrs = rxErrs;
            RxDrop = rxDrop;
            RxFifo = rxFifo;
            RxFrame = rxFrame;
            RxCompressed = rxCompressed;
            RxMulticast = rxMulticast;
            TxBytes = txBytes;
            TxPackets = txPackets;
            TxErrs = txErrs;
            TxDrop = txDrop;
            TxFifo = txFifo;
            TxFrame = txFrame;
            TxCompressed = txCompressed;
            TxMulticast = txMulticast;
        }
    }
}