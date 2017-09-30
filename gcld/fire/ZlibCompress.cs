using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using zlib;

namespace TestApp.fire
{
    /**//// <summary>
    /// Zlibѹ���㷨ѹ��Ӧ����
  /// </summary>
    public class ZlibCompress
    {
        /**//// <summary>
        /// �����������ֽ�
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2048];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        /**//// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="FileName">��ѹ���ļ����������������·����</param>
        /// <param name="CompressedFileName">ѹ���󱣴���ļ����������������·����</param>
        /// <returns></returns>
        public static bool CompressFile(string FileName, string CompressedFileName)
        {
            bool bResult = false;

            FileStream outFileStream = new FileStream(CompressedFileName, FileMode.Create);
            ZOutputStream outZStream = new ZOutputStream(outFileStream, zlibConst.Z_DEFAULT_COMPRESSION);
            FileStream inFileStream = new FileStream(FileName, FileMode.Open);
            try
            {
                CopyStream(inFileStream, outZStream);
                bResult = true;
            }
            catch
            {
                bResult = false;
            }
            finally
            {
                outZStream.Close();
                outFileStream.Close();
                inFileStream.Close();
            }
            return bResult;
        }

        /**//// <summary>
        /// ��ѹ�ļ�
        /// </summary>
        /// <param name="CompressedFileName">����ѹ�ļ����������������·����</param>
        /// <param name="DecompressFileName">��ѹ�󱣴���ļ����������������·����</param>
        /// <returns></returns>
        public static bool DecompressFile(string CompressedFileName, string DecompressFileName)
        {
            bool bResult = false;
            FileStream outFileStream = new FileStream(DecompressFileName, FileMode.Create);
            ZOutputStream outZStream = new ZOutputStream(outFileStream);
            FileStream inFileStream = new FileStream(CompressedFileName, FileMode.Open);
            try
            {
                CopyStream(inFileStream, outZStream);
                bResult = true;
            }
            catch
            {
                bResult = false;
            }
            finally
            {
                outZStream.Close();
                outFileStream.Close();
                inFileStream.Close();
            }
            return bResult;
        }

        /**//// <summary>
        /// ѹ��byte��������
        /// </summary>
        /// <param name="SourceByte">��Ҫ��ѹ����Byte��������</param>
        /// <returns></returns>
        public static byte[] CompressBytes(byte[] SourceByte)
        {
            try
            {
                MemoryStream stmInput = new MemoryStream(SourceByte);
                Stream stmOutPut = ZlibCompress.CompressStream(stmInput);
                byte[] bytOutPut = new byte[stmOutPut.Length];
                stmOutPut.Position = 0;
                stmOutPut.Read(bytOutPut, 0, bytOutPut.Length);
                return bytOutPut;
            }
            catch
            {
                return null;
            }
        }

        /**//// <summary>
        /// ��ѹbyte��������
        /// </summary>
        /// <param name="SourceByte">��Ҫ����ѹ��byte��������</param>
        /// <returns></returns>
        public static byte[] DecompressBytes(byte[] SourceByte)
        {
            try
            {
                MemoryStream stmInput = new MemoryStream(SourceByte);
                Stream stmOutPut = ZlibCompress.DecompressStream(stmInput);
                byte[] bytOutPut = new byte[stmOutPut.Length];
                stmOutPut.Position = 0;
                stmOutPut.Read(bytOutPut, 0, bytOutPut.Length);
                return bytOutPut;
            }
            catch
            {
                return null;
            }
        }

        /**//// <summary>
        /// ѹ����
        /// </summary>
        /// <param name="SourceStream">��Ҫ��ѹ����������</param>
        /// <returns></returns>
        public static Stream CompressStream(Stream SourceStream)
        {
            try
            {
                MemoryStream stmOutTemp = new MemoryStream();
                ZOutputStream outZStream = new ZOutputStream(stmOutTemp, zlibConst.Z_DEFAULT_COMPRESSION);
                CopyStream(SourceStream, outZStream);
                outZStream.finish();
                return stmOutTemp;
            }
            catch
            {
                return null;
            }
        }

        /**//// <summary>
        /// ��ѹ��
        /// </summary>
        /// <param name="SourceStream">��Ҫ��������������</param>
        /// <returns></returns>
        public static Stream DecompressStream(Stream SourceStream)
        {
            try
            {
                MemoryStream stmOutput = new MemoryStream();
                ZOutputStream outZStream = new ZOutputStream(stmOutput);
                CopyStream(SourceStream, outZStream);
                outZStream.finish();
                return stmOutput;
            }
            catch
            {
                return null;
            }
        }

        /**//// <summary>
        /// ѹ���ַ���
        /// </summary>
        /// <param name="SourceString">��Ҫ��ѹ�����ַ���</param>
        /// <returns></returns>
        public static string CompressString(string SourceString)
        {
            byte[] byteSource = System.Text.Encoding.UTF8.GetBytes(SourceString);
            byte[] byteCompress = ZlibCompress.CompressBytes(byteSource);
            if (byteCompress != null)
            {
                return Convert.ToBase64String(byteCompress);
            }
            else
            {
                return null;
            }
        }

        /**//// <summary>
        /// ��ѹ�ַ���
        /// </summary>
        /// <param name="SourceString">��Ҫ����ѹ���ַ���</param>
        /// <returns></returns>
        public static string DecompressString(string SourceString)
        {
            byte[] byteSource = Convert.FromBase64String(SourceString);
            byte[] byteDecompress = ZlibCompress.DecompressBytes(byteSource);
            if (byteDecompress != null)
            {
                return System.Text.Encoding.UTF8.GetString(byteDecompress);
            }
            else
            {
                return null;
            }
        }

    }
}