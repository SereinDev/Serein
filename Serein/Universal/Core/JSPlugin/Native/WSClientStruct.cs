namespace Serein.Core.JSPlugin.Native
{
    internal struct WSClientStruct
    {
        public bool disposed;

        public int state;

        public WSClientStruct(WSClient wsclient)
        {
            disposed = wsclient.Disposed;
            state = wsclient.state;
        }
    }
}
