using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MenuOnRobot
{
    public class CircularBuffer
    {
        MemoryStream m_msBuf;
        int m_nBufSize;
        int m_nPosRead;
        int m_nPosWrite;
        bool m_bIsFull; // 循环缓冲区写满标志(写满时只可读不可写)

        public int MaxWriteLength
        {
            get 
            {
                int n = m_nBufSize - (m_nPosWrite - m_nPosRead);
                if (n == m_nBufSize && !m_bIsFull) return n;
                return n % m_nBufSize;
            }
        }

        public int MaxReadLength
        {
            get
            {
                int n = m_nPosWrite - m_nPosRead;
                if (n == 0 && m_bIsFull) return m_nBufSize;
                return (n + m_nBufSize) % m_nBufSize;
            }
        }

        public CircularBuffer()
        {
            _CircularBuffer(10000);
        }
        public CircularBuffer(int n)
        {
            if (n < 0) n = 10000;
            _CircularBuffer(n);
        }
        private void _CircularBuffer(int n)
        {
            m_nBufSize = n;
            m_msBuf = new MemoryStream(m_nBufSize);
            m_nPosRead = 0;
            m_nPosWrite = 0;
            m_bIsFull = false;
        }

        public int Write(byte[] buf, int offset, int length)
        {
            if (offset < 0) return 0;
            int nCount = 0;
            // 可写入的的数据长度为 length、buf.Length - offset、MaxWriteLength 三者中的最小值
            int nDataLen = length < buf.Length - offset ? length : buf.Length - offset;
            nCount = nDataLen < MaxWriteLength ? nDataLen : MaxWriteLength;
            if (nCount > m_nBufSize - m_nPosWrite)
            {
                m_msBuf.Seek((long)m_nPosWrite, SeekOrigin.Begin);
                m_msBuf.Write(buf, offset, m_nBufSize - m_nPosWrite);
                m_msBuf.Seek(0, SeekOrigin.Begin);
                m_msBuf.Write(buf, offset + m_nBufSize - m_nPosWrite, (nCount + m_nPosWrite) % m_nBufSize);
            }
            else
            {
                m_msBuf.Seek((long)m_nPosWrite, SeekOrigin.Begin);
                m_msBuf.Write(buf, offset, nCount);
            }
            m_nPosWrite = (m_nPosWrite + nCount) % m_nBufSize;
            if (m_nPosRead == m_nPosWrite) m_bIsFull = true;
            return nCount;
        }

        public int Read(byte[] outBuf)
        {
            int nCount = outBuf.Length < MaxReadLength ? outBuf.Length : MaxReadLength;
            if (nCount > m_nBufSize - m_nPosRead)
            {
                MemoryStream ms = new MemoryStream();
                m_msBuf.Seek((long)m_nPosRead, SeekOrigin.Begin);
                m_msBuf.Read(outBuf, 0, m_nBufSize - m_nPosRead);
                ms.Write(outBuf, 0, m_nBufSize - m_nPosRead);
                m_msBuf.Seek(0, SeekOrigin.Begin);
                m_msBuf.Read(outBuf, 0, (nCount + m_nPosRead) % m_nBufSize);
                ms.Write(outBuf, 0, (nCount + m_nPosRead) % m_nBufSize);
                ms.Read(outBuf, 0, nCount);
            }
            else
            {
                m_msBuf.Seek(m_nPosRead, SeekOrigin.Begin);
                nCount = m_msBuf.Read(outBuf, 0, nCount);
            }
            m_nPosRead = (m_nPosRead + nCount) % m_nBufSize;
            if (m_bIsFull) m_bIsFull = false;
            return nCount;
        }

        public void Clear()
        {

            if (null != m_msBuf)
            {
                byte[] by = new byte[m_msBuf.Capacity];
                m_msBuf.Write(by, 0, m_msBuf.Capacity);
            }
        }
    }
}
