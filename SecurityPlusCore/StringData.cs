using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SecurityPlusCore
{
    /*
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StringData
    {
        public short Length;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPWStr, SizeConst = 128)]
        public byte[] Buffer;

        public override string ToString()
        {
            unsafe
            {
                fixed (byte* data = this.Buffer)
                {
                    return Encoding.Unicode.GetString(data, this.Length);
                }
            }
        }
    }
    */
        
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StringData
    {
        public short Length;
        
        public string Buffer;

        public override string ToString()
        {
            return this.Buffer;
        }
    }
   
    /*
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  public struct StringData
  {
      public short Length;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string Buffer;

      public override string ToString()
      {
            return this.Buffer.ToString();
      }
  }*/  
}
