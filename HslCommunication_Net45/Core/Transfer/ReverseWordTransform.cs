﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HslCommunication.Core
{
    /// <summary>
    /// 按照字节错位的数据转换类
    /// </summary>
    public class ReverseWordTransform : ByteTransformBase
    {

        #region Private Method

        /// <summary>
        /// 按照字节错位的方法
        /// </summary>
        /// <param name="buffer">实际的字节数据</param>
        /// <param name="index">起始字节位置</param>
        /// <param name="length">数据长度</param>
        /// <param name="isReverse">是否按照字来反转</param>
        /// <returns></returns>
        private byte[] ReverseBytesByWord( byte[] buffer, int index, int length, bool isReverse = false )
        {
            byte[] tmp = new byte[length];

            for (int i = 0; i < length; i++)
            {
                tmp[i] = buffer[index + i];
            }

            for (int i = 0; i < length / 2; i++)
            {
                byte b = tmp[i * 2 + 0];
                tmp[i * 2 + 0] = tmp[i * 2 + 1];
                tmp[i * 2 + 1] = b;
            }

            // 两两高地位互换
            if(isReverse)
            {
                if(tmp.Length == 4)
                {
                    byte a = tmp[0];
                    byte b = tmp[1];

                    tmp[0] = tmp[2];
                    tmp[1] = tmp[3];

                    tmp[2] = a;
                    tmp[3] = b;
                }
                else if(tmp.Length == 8)
                {
                    byte a = tmp[0];
                    byte b = tmp[1];

                    tmp[0] = tmp[6];
                    tmp[1] = tmp[7];

                    tmp[6] = a;
                    tmp[7] = b;

                    a = tmp[2];
                    b = tmp[3];

                    tmp[2] = tmp[4];
                    tmp[3] = tmp[5];

                    tmp[4] = a;
                    tmp[5] = b;
                }
            }

            return tmp;
        }

        private byte[] ReverseBytesByWord( byte[] buffer , bool isReverse = false )
        {
            return ReverseBytesByWord( buffer, 0, buffer.Length , isReverse );
        }


        #endregion

        #region Public Properties
        
        /// <summary>
        /// 多字节的数据是否高低位反转，常用于Int32,UInt32,float,double,Int64,UInt64类型读写
        /// </summary>
        public bool IsMultiWordReverse { get; set; }

        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        public bool IsStringReverse { get; set; }


        #endregion

        #region Get Value From Bytes


        /// <summary>
        /// 从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public override short TransInt16( byte[] buffer, int index )
        {
            return BitConverter.ToInt16( ReverseBytesByWord( buffer, index, 2 ), 0 );
        }



        /// <summary>
        /// 从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public override ushort TransUInt16( byte[] buffer, int index )
        {
            return BitConverter.ToUInt16( ReverseBytesByWord( buffer, index, 2 ), 0 );
        }



        /// <summary>
        /// 从缓存中提取int结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>int对象</returns>
        public override int TransInt32( byte[] buffer, int index )
        {
            return BitConverter.ToInt32( ReverseBytesByWord( buffer, index, 4, IsMultiWordReverse ), 0 );
        }


        /// <summary>
        /// 从缓存中提取uint结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>uint对象</returns>
        public override uint TransUInt32( byte[] buffer, int index )
        {
            return BitConverter.ToUInt32( ReverseBytesByWord( buffer, index, 4, IsMultiWordReverse ), 0 );
        }


        /// <summary>
        /// 从缓存中提取long结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>long对象</returns>
        public override long TransInt64( byte[] buffer, int index )
        {
            return BitConverter.ToInt64( ReverseBytesByWord( buffer, index, 8, IsMultiWordReverse ), 0 );
        }



        /// <summary>
        /// 从缓存中提取ulong结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ulong对象</returns>
        public override ulong TransUInt64( byte[] buffer, int index )
        {
            return BitConverter.ToUInt64( ReverseBytesByWord( buffer, index, 8, IsMultiWordReverse ), 0 );
        }




        /// <summary>
        /// 从缓存中提取float结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>float对象</returns>
        public override float TransSingle( byte[] buffer, int index )
        {
            return BitConverter.ToSingle( ReverseBytesByWord( buffer, index, 4, IsMultiWordReverse ), 0 );
        }



        /// <summary>
        /// 从缓存中提取double结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>double对象</returns>
        public override double TransDouble( byte[] buffer, int index )
        {
            return BitConverter.ToDouble( ReverseBytesByWord( buffer, index, 8, IsMultiWordReverse ), 0 );
        }
        

        

        /// <summary>
        /// 从缓存中提取string结果，使用指定的编码
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        public override string TransString( byte[] buffer, int index, int length, Encoding encoding )
        {
            byte[] tmp = TransByte( buffer, index, length );

            if(IsStringReverse)
            {
                return encoding.GetString( ReverseBytesByWord( tmp, false ) );
            }
            else
            {
                return encoding.GetString( tmp );
            }
        }

        #endregion

        #region Get Bytes From Value
        
        /// <summary>
        /// bool数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( bool[] values )
        {
            return BasicFramework.SoftBasic.BoolArrayToByte( values );
        }



        /// <summary>
        /// short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( short[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes( values[i] );
                tmp.CopyTo( buffer, 2 * i );
            }

            return ReverseBytesByWord( buffer );
        }


        /// <summary>
        /// ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( ushort[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tmp = BitConverter.GetBytes( values[i] );
                tmp.CopyTo( buffer, 2 * i );
            }

            return ReverseBytesByWord( buffer );
        }



        /// <summary>
        /// int数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( int[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 4 * i );
            }

            return buffer;
        }


        /// <summary>
        /// uint数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( uint[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 4 * i );
            }

            return buffer;
        }


        /// <summary>
        /// long数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( long[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 8 * i );
            }

            return buffer;
        }

        /// <summary>
        /// ulong数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( ulong[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 8 * i );
            }

            return buffer;
        }


        /// <summary>
        /// float数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( float[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 4 * i );
            }

            return buffer;
        }

        /// <summary>
        /// double数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( double[] values )
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                ReverseBytesByWord( BitConverter.GetBytes( values[i] ), IsMultiWordReverse ).CopyTo( buffer, 8 * i );
            }

            return buffer;
        }
        

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( string value, Encoding encoding )
        {
            if (value == null) return null;
            byte[] buffer = encoding.GetBytes( value );
            buffer = BasicFramework.SoftBasic.ArrayExpandToLengthEven( buffer );
            if (IsStringReverse)
            {
                return ReverseBytesByWord( buffer, false );
            }
            else
            {
                return buffer;
            }
        }

        #endregion

    }
}
