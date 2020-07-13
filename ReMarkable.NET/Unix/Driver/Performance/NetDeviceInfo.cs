namespace ReMarkable.NET.Unix.Driver.Performance
{
    /// <summary>
    ///     Contains data related to the statistics of a network adapter
    /// </summary>
    internal class NetDeviceInfo
    {
        public long RxBytes { get; }
        public long RxCompressed { get; }
        public long RxDrop { get; }
        public long RxErrs { get; }
        public long RxFifo { get; }
        public long RxFrame { get; }
        public long RxMulticast { get; }
        public long RxPackets { get; }
        public long TxBytes { get; }
        public long TxCompressed { get; }
        public long TxDrop { get; }
        public long TxErrs { get; }
        public long TxFifo { get; }
        public long TxFrame { get; }
        public long TxMulticast { get; }
        public long TxPackets { get; }

        /// <summary>
        ///     Creates a new <see cref="NetDeviceInfo" />
        /// </summary>
        /// <param name="rxBytes"></param>
        /// <param name="rxPackets"></param>
        /// <param name="rxErrs"></param>
        /// <param name="rxDrop"></param>
        /// <param name="rxFifo"></param>
        /// <param name="rxFrame"></param>
        /// <param name="rxCompressed"></param>
        /// <param name="rxMulticast"></param>
        /// <param name="txBytes"></param>
        /// <param name="txPackets"></param>
        /// <param name="txErrs"></param>
        /// <param name="txDrop"></param>
        /// <param name="txFifo"></param>
        /// <param name="txFrame"></param>
        /// <param name="txCompressed"></param>
        /// <param name="txMulticast"></param>
        public NetDeviceInfo(long rxBytes, long rxPackets, long rxErrs, long rxDrop, long rxFifo, long rxFrame,
            long rxCompressed, long rxMulticast, long txBytes, long txPackets, long txErrs, long txDrop, long txFifo,
            long txFrame, long txCompressed, long txMulticast)
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