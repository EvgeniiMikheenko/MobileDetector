using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace LGAR
{
    public class SafeNetworkStream : Modbus.IO.IStreamResource, IDisposable
    {
        private volatile TcpClient tcp = null;
        private Uri target = null;
        private int _rt = 500;
        private int _wt = 1000;
        private volatile bool Disposed = false;

        public SafeNetworkStream(Uri target)
        {
            this.target = target;
            Connect();
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            var t = tcp;
            tcp = null;
            if (t != null)
                try
                {
                    t.Close();
                }
                catch { }
        }

        public void DiscardInBuffer()
        {
            var t = tcp;
            if (t != null)
                try
                {
                    if (!t.Connected) return;
                    var s = t.GetStream();
                    //s.Flush();
                    int a;
                    while ((a = t.Available) > 0)
                        s.Read(new byte[a], 0, a); // HACK
                }
                catch { }
        }

        public int InfiniteTimeout { get { return Timeout.Infinite; } }

        public int ReadTimeout
        {
            get { return _rt; }
            set
            {
                _rt = value;
                if (tcp != null) tcp.ReceiveTimeout = _rt;
            }
        }

        public int WriteTimeout
        {
            get { return _wt; }
            set
            {
                _wt = value;
                if (tcp != null) tcp.SendTimeout = _wt;
            }
        }

        private void Connect()
        {
            if (!Disposed && Monitor.TryEnter(target))
                try
                {
                    //var t = tcp;
                    //tcp = null;
                    ThreadPool.QueueUserWorkItem(_connect);
                    //if (t != null) t.Close();
                }
                catch { Thread.Sleep(1000); }
                finally { Monitor.Exit(target); }
        }

        object _connectLock = new object();

        private void _connect(object e)
        {
            if (!Disposed && Monitor.TryEnter(_connectLock))
                try
                {
                    var t = tcp;
                    tcp = null;
                    try
                    {
                        if (t != null)
                        {
                            var s = t.Client;
                            if (s.Connected) s.Disconnect(false);
                            //s.Dispose();
                        }

                    }
                    catch { }

                    int port = target.Port;
                    t = new TcpClient(target.Host, port <= 0 ? 502 : port)
                    {
                        ReceiveBufferSize = 10000,
                        ReceiveTimeout = _rt,
                        SendTimeout = _wt
                    };
                    if (t.Connected) tcp = t;
                }
                catch { }
                finally { Monitor.Exit(_connectLock); }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var t = tcp;
            if (t != null && !Disposed && t.Connected)
                try
                {
                    return t.GetStream().Read(buffer, offset, count);
                }
                catch (IOException)
                {
                    throw;
                }
                catch
                {
                    Connect();
                    throw;
                }
            else Connect();
            throw new TimeoutException("TCP is disconnected.");
        }

        public void Write(byte[] buf, int ofs, int cnt)
        {
            var t = tcp;
            if (t != null && !Disposed && t.Connected)
                try
                {
                    t.GetStream().Write(buf, ofs, cnt);
                    return;
                }
                catch (TimeoutException) { return; }
                catch (IOException) { return; }
                catch { }
            //Connect();
        }

        public NetworkStream Connection
        {
            get
            {
                var t = tcp;
                if (t != null) try
                    {
                        return t.GetStream();
                    }
                    catch { }
                return null;
            }
        }
    }
}
