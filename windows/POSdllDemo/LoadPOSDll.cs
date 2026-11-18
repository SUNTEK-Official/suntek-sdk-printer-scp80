using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;

namespace PrinterTest
{
    public class LoadPOSDll
    {
        const string _DllVer = "1.4";
        /// <summary>

        /// </summary>
        public string GetDllVer
        {
            get { return _DllVer;}
        }

      
        /// <summary>

        /// </summary>
        public IntPtr POS_IntPtr;

        /// <summary>

        /// </summary>
        public const int POS_SUCCESS = 1001;
        public const int POS_FAIL = 1002;
        public const int POS_ERROR_INVALID_HANDLE = 1101;
        public const int POS_ERROR_INVALID_PARAMETER = 1102;
        public const int POS_ERROR_NOT_BITMAP = 1103;
        public const int POS_ERROR_NOT_MONO_BITMAP = 1104;
        public const int POS_ERROR_BEYONG_AREA = 1105;

        public const int POS_ERROR_INVALID_PATH = 1106;

        public const int POS_COM_DTR_DSR = 0x00; // DTR/DST
        public const int POS_COM_RTS_CTS = 0x01; // RTS/CTS
        public const int POS_COM_XON_XOFF = 0x02; // XON/OFF
        public const int POS_COM_NO_HANDSHAKE = 0x03;
        public const int POS_COM_ONESTOPBIT=0x00;
        public const int POS_COM_ONE5STOPBITS=0x01; // 1．5
        public const int POS_COM_TWOSTOPBITS=0x02;
        public const int POS_COM_NOPARITY=0x00;
        public const int POS_COM_ODDPARITY=0x01;
        public const int POS_COM_EVENPARITY=0x02;
        public const int POS_COM_MARKPARITY=0x03;
        public const int POS_COM_SPACEPARITY=0x04;
        public const int POS_OPEN_PARALLEL_PORT = 0x12;
        public const int POS_OPEN_BYUSB_PORT = 0x13; // USB
        public const int POS_OPEN_PRINTNAME = 0X14;
        public const int POS_OPEN_NETPORT = 0X15;
        public const int POS_FONT_TYPE_STANDARD = 0x00; // ASCII
        public const int POS_FONT_TYPE_COMPRESSED = 0x01; // ASCII
        public const int POS_FONT_TYPE_UDC = 0x02;
        public const int POS_FONT_TYPE_CHINESE = 0x03; // " "
        public const int POS_FONT_STYLE_NORMAL = 0x00;
        public const int POS_FONT_STYLE_BOLD = 0x08;
        public const int POS_FONT_STYLE_THIN_UNDERLINE = 0x80;
        public const int POS_FONT_STYLE_THICK_UNDERLINE = 0x100;
        public const int POS_FONT_STYLE_UPSIDEDOWN = 0x200; // ( )
        public const int POS_FONT_STYLE_REVERSE = 0x400; // ( )
        public const int POS_FONT_STYLE_SMOOTH = 0x800; // ( )
        public const int POS_FONT_STYLE_CLOCKWISE_90 = 0x1000;
        public const int POS_PRINT_MODE_STANDARD = 0x00; // ( )
        public const int POS_PRINT_MODE_PAGE = 0x01;
        public const int POS_PRINT_MODE_BLACK_MARK_LABEL = 0x02;
        public const int POS_BARCODE_TYPE_UPC_A = 0x02;// UPC-A
        public const int POS_BARCODE_TYPE_UPC_E = 0x41;// UPC-C
        public const int POS_BARCODE_TYPE_JAN13 = 0x43;// JAN13(EAN13)
        public const int POS_BARCODE_TYPE_JAN8 = 0x44;// JAN8(EAN8)
        public const int POS_BARCODE_TYPE_CODE39 = 0x45 ;// CODE39
        public const int POS_BARCODE_TYPE_ITF = 0x46;// INTERLEAVED 2 OF 5
        public const int POS_BARCODE_TYPE_CODEBAR = 0x47;// ICODEBAR
        public const int POS_BARCODE_TYPE_CODE93 = 0x48;// INTERLEAVED 2 OF 5
        public const int POS_BARCODE_TYPE_CODE128 = 0x49;// CODE 128


 // public const int POS_FONT_TYPE_STANDARD = 0x00;// ASCII
 // public const int POS_FONT_TYPE_COMPRESSED = 0x01;// ASCII
        public const int POS_HRI_POSITION_NONE = 0x00;
        public const int POS_HRI_POSITION_ABOVE = 0x01;
        public const int POS_HRI_POSITION_BELOW = 0x02;
        public const int POS_HRI_POSITION_BOTH = 0x03;
        public const int POS_BITMAP_PRINT_NORMAL = 0x00;
        public const int POS_BITMAP_PRINT_DOUBLE_WIDTH = 0x01;
        public const int POS_BITMAP_PRINT_DOUBLE_HEIGHT = 0x02;
        public const int POS_BITMAP_PRINT_QUADRUPLE = 0x03;
        public const int POS_CUT_MODE_FULL = 0x00;
        public const int POS_CUT_MODE_PARTIAL = 0x01;
        public const int POS_AREA_LEFT_TO_RIGHT = 0x0;
        public const int POS_AREA_BOTTOM_TO_TOP = 0x1;
        public const int POS_AREA_RIGHT_TO_LEFT = 0x2;
        public const int POS_AREA_TOP_TO_BOTTOM = 0x3;

        /// <summary>
 // / POS
        /// </summary>
        /// <param name="lpName">
 // / null
 // / nParam POS_COM_DTR_DSR, POS_COM_RTS_CTS, POS_COM_XON_XOFF POS_COM_NO_HANDSHAKE , "COM1", "COM2", "COM3", "COM4" ;
 // / nParam POS_OPEN_PARALLEL_PORT , "LPT1", "LPT2" ;
 // / nParam POS_OPEN_BYUSB_PORT , "BYUSB-0", "BYUSB-1", "BYUSB-2", "BYUSB-3" USB
 // / nParam POS_OPEN_PRINTNAME
 // / nParam POS_OPEN_NETPORT , , "192.0.2.10" IP </param>
 // / <param name="nComBaudrate"> </param>
 // / <param name="nComDataBits"> </param>
 // / <param name="nComStopBits"> </param>
 // / <param name="nComParity"> </param>
 // / <param name="nParam"> null
 // / nParam POS_COM_DTR_DSR, POS_COM_RTS_CTS, POS_COM_XON_XOFF POS_COM_NO_HANDSHAKE
 // / "COM1", "COM2", "COM3", "COM4" ;
 // / nParam POS_OPEN_PARALLEL_PORT , "LPT1", "LPT2" ;
 // / nParam POS_OPEN_BYUSB_PORT , "BYUSB-0", "BYUSB-1", "BYUSB-2", "BYUSB-3" USB
 // / nParam POS_OPEN_PRINTNAME , . </param>
 // / <returns> , . , INVALID_HANDLE_VALUE (-1). </returns> [DllImport("POSDLL.dll", CharSet = CharSet.Ansi)]
        [DllImport("POSDLL.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr POS_Open([MarshalAs(UnmanagedType.LPStr)]string lpName,
                                             uint nComBaudrate,
                                             uint nComDataBits,
                                             uint nComStopBits,
                                             uint nComParity,
                                             uint nParam);

        /// <summary>
 // / , USB
        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_Close();

        /// <summary>

        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_Reset();

        /// <summary>

        /// </summary>
 // / <param name="nHorizontalMU"> 25.4 / nHorizontalMU . 0 255. </param>
 // / <param name="nVerticalMU"> 25.4 / nVerticalMU . 0 255. </param>
        /// <returns>
 // / , POS_SUCCESS
 // / , : POS_FAIL POS_ERROR_INVALID_HANDLE POS_ERROR_INVALID_PARAMETER </returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetMotionUnit(uint nHorizontalMU, uint nVerticalMU);
        /// <summary>

        /// </summary>
        /// <param name="nCharSet">
 // / . 0x23 0x7E ASCII

        /// 0x00 U.S.A  0x01 France  0x02 Germany  0x03 U.K. 0x04 Denmark I 0x05 Sweden 
        /// 0x06 Italy 0x07 Spain I  0x08 Japan 0x09 Nonway 0x0A Denmark II 0x0B Spain II 
        /// 0x0C Latin America 0x0D Korea </param>
        /// <param name="nCodePage">
 // / . 0x80 0xFF ASCII
        /// 0x00 PC437 [U.S.A. Standard Europe 0x01 Reserved 0x02 PC850 [Multilingual] 
        /// 0x03 PC860 [Portuguese] 0x04 PC863 [Canadian-French] 0x05 PC865 [Nordic] 
        /// 0x12 PC852 0x13 PC858 
        /// </param>
        /// <returns>
 // / , POS_SUCCESS
 // / , : POS_FAIL POS_ERROR_INVALID_HANDLE POS_ERROR_INVALID_PARAMETER </returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetCharSetAndCodePage(uint nCharSet, uint nCodePage);

        /// <summary>
 // / POS
        /// </summary>
        /// 
 // public uint POS_FONT_TYPE_STANDARD = 0x00;// ASCII
 // public uint POS_FONT_TYPE_COMPRESSED = 0x01;// ASCII
 // public uint POS_FONT_TYPE_UDC = 0x02; //
 // public uint POS_FONT_TYPE_CHINESE = 0x03; // " "
 // public uint POS_FONT_STYLE_NORMAL = 0x00; //
 // public uint POS_FONT_STYLE_BOLD = 0x08; //
 // public uint POS_FONT_STYLE_THIN_UNDERLINE = 0x80; // 1
 // public uint POS_FONT_STYLE_THICK_UNDERLINE = 0x100; // 2
 // public uint POS_FONT_STYLE_UPSIDEDOWN = 0x200; // ( )
 // public uint POS_FONT_STYLE_REVERSE = 0x400; // ( )
 // public uint POS_FONT_STYLE_SMOOTH = 0x800; // ( )
 // public uint POS_FONT_STYLE_CLOCKWISE_90 = 0x1000; // 90

        /// <summary>
 // / , X ( )

        /// </summary>
 // / <param name="pszString"> null </param>
 // / <param name="nOrgx"> X ( ) . </param>
 // / <param name="nWidthTimes"> . 1 6. </param>
 // / <param name="nHeightTimes"> . 1 6. </param>
 // / <param name="nFontType"> . </param>
 // / <param name="nFontStyle"> . </param>
        /// <returns></returns>
        
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_S_TextOut([MarshalAs(UnmanagedType.LPStr)]string pszString, 
                                                   uint nOrgx, uint nWidthTimes, uint nHeightTimes, 
                                                   uint nFontType, uint nFontStyle);

        /// <summary>
 // / POS ( )
        /// </summary>
        /// <param name="nPrintMode">
 // / POS_PRINT_MODE_STANDARD 0x00 ( )
 // / POS_PRINT_MODE_PAGE 0x01
 // / POS_PRINT_MODE_BLACK_MARK_LABEL 0x02
 // / POS_PRINT_MODE_WHITE_MARK_LABEL 0x03 </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetMode(uint nPrintMode);
        /// <summary>

        /// </summary>
 // / <param name="nDistance"> . 0 255. . </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetLineSpacing(uint nDistance);
        /// <summary>
 // / ( )
        /// </summary>
 // / <param name="nDistance"> . 0 255. . </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetRightSpacing(int nDistance);

        /// <summary>

 // / 1． ( )
 // / 2． ( )
 // / 3．
 // / , POS_PL_Print
        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_FeedLine();
        /// <summary>
 // / n
        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_FeedLines(uint nLines);
        
        /// <summary>

        /// </summary>
 // / <param name="nMode"> </param>
 // / <param name="nDistance"> </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_CutPaper(uint nMode, uint nDistance);

        /// <summary>

        /// </summary>
 // / <param name="nDistance"> </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_SetRightSpacing(uint nDistance);
        /// <summary>
 // / RAM , ID
        /// </summary>
 // / <param name="pszPath"> null . </param>
 // / <param name="nID"> ID . 0 7. </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PreDownloadBmpToRAM([MarshalAs(UnmanagedType.LPStr)]string pszPath, uint nID);
        /// <summary>
 // / Flash
        /// </summary>
 // / <param name="pszPath">[in] . </param>
 // / <param name="nID">[in] . 1 255. </param>
 // / <returns> , POS_SUCCESS</returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PreDownloadBmpsToFlash(IntPtr[] pszPath, uint nCount);
        /// <summary>

        /// </summary>
 // / <param name="pszPath"> null . </param>
 // / <param name="nOrgx"> . 0 65535 . </param>
 // / <param name="nMode"> . </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_S_DownloadAndPrintBmp([MarshalAs(UnmanagedType.LPStr)]string pszPath, uint nOrgx, uint nMode);

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_S_PrintBmpInRAM(uint nID, uint nOrgx, uint nMode);

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_S_PrintBmpInFlash(uint nID, uint nOrgx, uint nMode);

        /// <summary>

        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_RTQueryStatus(byte[] address);

        /// <summary>

        /// </summary>
        /// <param name="pszStatus">
 // / , 1
 // / 0, 1 0/1 / 2, 3 0/1 /
 // / 4, 5 0/1 3 / ( )
 // / 6, 7 0 ( 0)
        /// </param>
 // / <param name="nTimeouts"> ( ). </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_QueryStatus(byte[] pszStatus, int nTimeouts);
        /// <summary>

        /// </summary>
 // / <param name="ipAddress"> IP . "192.0.2.10". </param>
        /// <param name="pszStatus">
 // / , 1
 // / 0 0/1 3 / ( )
 // / 1 0/1 /
 // / 2 0/1 /
 // / 3 0/1 / Feed
 // / 4 0/1 /
 // / 5 0/1 /
 // / 6 0/1 / ( )
 // / 7 0/1 / ( )
        /// </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern int POS_NETQueryStatus([MarshalAs(UnmanagedType.LPStr)]string ipAddress, out Byte pszStatus);

        
        /// <summary>

        /// </summary>
 // / <param name="pszInfo"> null . . </param>
 // / <param name="nOrgx"> . 0 65535. </param>
        /// <param name="nType">

        /// POS_BARCODE_TYPE_UPC_A 0x41 UPC-A POS_BARCODE_TYPE_UPC_E 0x42 UPC-C 
        /// POS_BARCODE_TYPE_JAN13 0x43 JAN13(EAN13) POS_BARCODE_TYPE_JAN8 0x44 JAN8(EAN8) 
        /// POS_BARCODE_TYPE_CODE39 0x45 CODE39 POS_BARCODE_TYPE_ITF 0x46 INTERLEAVED 2 OF 5 
        /// POS_BARCODE_TYPE_CODEBAR 0x47 CODEBAR POS_BARCODE_TYPE_CODE93 0x48 25 
        /// POS_BARCODE_TYPE_CODE128 0x49 CODE 128 </param>
        /// <param name="nWidthX">

        /// 2 0．25mm 0．25mm 0．625mm 3 0．375mm 0．375mm 1．0mm 4 0．5mm 0．5mm 1．25mm 
        /// 5 0．625mm 0．625mm 1．625mm 6 0．75mm 0．75mm 1.875mm 
        /// </param>
 // / <param name="nheight"> . 1 255 . 162 . </param>
        /// <param name="nHriFontType">
 // / HRI(Human Readable Interpretation)
 // / POS_FONT_TYPE_STANDARD 0x00 ASCII POS_FONT_TYPE_COMPRESSED 0x01 ASCII
        /// </param>
        /// <param name="nHriFontPosition">
 // / HRI(Human Readable Interpretation)
 // / POS_HRI_POSITION_NONE 0x00 POS_HRI_POSITION_ABOVE 0x01
 // / POS_HRI_POSITION_BELOW 0x02 POS_HRI_POSITION_BOTH 0x03
        /// </param>
 // / <param name="nBytesOfInfo"> pszInfoBuffer , . . </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_S_SetBarcode([MarshalAs(UnmanagedType.LPStr)]string pszInfo,
                                                      uint nOrgx,uint nType,uint nWidthX,uint nheight,
                                                      uint nHriFontType,uint nHriFontPosition,uint nBytesOfInfo);


        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_SetArea(uint nOrgx , uint nOrgY , uint nWidth , uint nheight , uint nDirection );

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_TextOut([MarshalAs(UnmanagedType.LPStr)]string pszString,uint nOrgx,uint nOrgY, 
                                                   uint nWidthTimes, uint nHeightTimes, uint nFontType, uint nFontStyle);

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_PrintBmpInRAM(uint nID , uint nOrgx , uint nOrgY , uint nMode );

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_PrintBmpInFlash(uint nID, uint nOrgx, uint nOrgY, uint nMode);

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_SetBarcode([MarshalAs(UnmanagedType.LPStr)]string pszInfo, 
                                                       uint nOrgx , uint nOrgY , uint nType , uint nWidthX,uint nheight,
                                                       uint nHriFontType,uint nHriFontPosition,uint nBytesOfInfo);

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_Clear();

        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_PL_Print();
        /// <summary>

        /// </summary>
 // / <param name="nID"> . 0x00 2 0x01 5 </param>
 // / <param name="nOnTimes"> , nOnTimes × 2 . 1 255. </param>
 // / <param name="nOffTimes"> , nOffTimes × 2 . 1 255. </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_KickOutDrawer(uint nID, uint nOnTimes, uint nOffTimes);
        /// <summary>

        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern bool POS_StartDoc();
        /// <summary>

        /// </summary>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern bool POS_EndDoc();
        /// <summary>

        /// </summary>
 // / <param name="hPort"> . POS_Open </param>
 // / <param name="pszData"> . </param>
 // / <param name="nBytesToWrite"> . </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_WriteFile(IntPtr hPort, byte[] pszData, uint nBytesToWrite);
        /// <summary>
 // / , USB
        /// </summary>
 // / <param name="hPort"> . POS_Open </param>
 // / <param name="pszData"> . </param>
 // / <param name="nBytesToRead"> </param>
 // / <param name="nTimeouts"> </param>
        /// <returns></returns>
        [DllImport("POSDLL.dll", SetLastError = true)]
        public static extern IntPtr POS_ReadFile(IntPtr hPort, byte[] pszData, uint nBytesToRead, uint nTimeouts);

				public IntPtr DownloadBmpsToFlash(IntPtr[] pszPath, uint nCount)
				{
					  return POS_PreDownloadBmpsToFlash(pszPath,nCount);
				}
				public IntPtr PrintBmpInFlash(uint nID, uint nOrgx, uint nMode)
				{
					  return POS_S_PrintBmpInFlash(nID, nOrgx, nMode);
				}
        /// <summary>

        /// </summary>
 // / <param name="PrintSerialPort"> ( , )</param>
 // / <returns> </returns>
        public bool OpenComPort(ref SerialPort PrintSerialPort)
        { 
            uint i_stopbits=0;
            if (PrintSerialPort.StopBits== StopBits.One)
                i_stopbits=POS_COM_ONESTOPBIT;
            if (PrintSerialPort.StopBits== StopBits.OnePointFive)
                i_stopbits=POS_COM_ONE5STOPBITS;
            if (PrintSerialPort.StopBits== StopBits.Two)
                i_stopbits=POS_COM_TWOSTOPBITS;

            uint i_nComParity=0;
            if (PrintSerialPort.Parity== Parity.None)
                i_nComParity=POS_COM_NOPARITY;
            if (PrintSerialPort.Parity== Parity.Even)
                i_nComParity=POS_COM_EVENPARITY;
            if (PrintSerialPort.Parity== Parity.Odd)
                i_nComParity=POS_COM_ODDPARITY;
            if (PrintSerialPort.Parity== Parity.Space)
                i_nComParity=POS_COM_SPACEPARITY;
            if (PrintSerialPort.Parity== Parity.Mark)
                i_nComParity=POS_COM_MARKPARITY;

            uint i_para=0;
            if (PrintSerialPort.Handshake== Handshake.None)
                i_para=POS_COM_NO_HANDSHAKE;
            if (PrintSerialPort.Handshake== Handshake.RequestToSend)
                i_para=POS_COM_DTR_DSR;
            if (PrintSerialPort.Handshake== Handshake.RequestToSendXOnXOff)
                i_para=POS_COM_RTS_CTS;
            if (PrintSerialPort.Handshake== Handshake.XOnXOff)
                i_para=POS_COM_XON_XOFF;

            POS_IntPtr = POS_Open(PrintSerialPort.PortName, 
                                 (uint)PrintSerialPort.BaudRate,
                                 (uint)PrintSerialPort.DataBits,
                                 i_stopbits, i_nComParity, i_para);

            if ((int)POS_IntPtr != -1)
                return true;
            else
                return false;
        }
        /// <summary>

        /// </summary>
 // / <param name="LPTPortName"> </param>
 // / <returns> </returns>
        public bool OpenLPTPort(string LPTPortName)
        {
            POS_IntPtr = POS_Open(LPTPortName, 0, 0, 0, 0, POS_OPEN_PARALLEL_PORT);
            if ((int)POS_IntPtr != -1)
                return true;
            else
                return false;
        }
        /// <summary>

        /// </summary>
 // / <param name="IPAddress"> IP </param>
 // / <returns> </returns>
        public bool OpenNetPort(string IPAddress)
        {
            POS_IntPtr = POS_Open(IPAddress, 0, 0, 0, 0, POS_OPEN_NETPORT);
            if ((int)POS_IntPtr != -1)
                return true;
            else
                return false;
        }
        /// <summary>
 // / USB
        /// </summary>
        /// <param name="USBPortName">“BYUSB-0”、“BYUSB-1”、“BYUSB-2”、“BYUSB-3”</param>
 // / <returns> </returns>
        public bool OpenUSBPort(string USBPortName)
        {
            POS_IntPtr = POS_Open(USBPortName, 0, 0, 0, 0, POS_OPEN_BYUSB_PORT);
            if ((int)POS_IntPtr != -1)
                return true;
            else
                return false;
        }
        /// <summary>
 // / windows OPOS
        /// </summary>
 // / <param name="PrintName"> </param>
 // / <returns> </returns>
        public bool OpenPrinter(string PrintName)
        {
            POS_IntPtr = POS_Open(PrintName, 0, 0, 0, 0, POS_OPEN_PRINTNAME);
            if ((int)POS_IntPtr != -1)
                return true;
            else
                return false;
        }
        /// <summary>

        /// </summary>
 // / <returns> </returns>
        public bool ClosePrinterPort()
        {
            IntPtr tmpIntPtr = POS_Close();
            return ((uint)tmpIntPtr == POS_SUCCESS);
        }
    }
}