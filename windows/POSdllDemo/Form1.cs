using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using PrinterTest;
using System.Threading;

namespace POSdllDemo
{
    public partial class Form1 : Form
    {
        private IntPtr Gp_IntPtr;
        private libUsbContorl.UsbOperation NewUsb=new libUsbContorl.UsbOperation();
        public Form1()
        {
            InitializeComponent();


            cbbCOM.Items.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {

                cbbCOM.Items.Add(s.Replace(" ", ""));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadPOSDll PosPrint = new LoadPOSDll();
            
 // POS_COM_DTR_DSR 0x00 DTR/DST
 // POS_COM_RTS_CTS 0x01 RTS/CTS
 // POS_COM_XON_XOFF 0x02 XON/OFF
 // POS_COM_NO_HANDSHAKE 0x03
 // POS_OPEN_PARALLEL_PORT 0x12
 // POS_OPEN_BYUSB_PORT 0x13 USB
 // POS_OPEN_PRINTNAME 0X14
 // POS_OPEN_NETPORT 0x15

            if (PosPrint.OpenNetPort("192.0.2.10")) // nParam POS_OPEN_NETPORT , , "192.0.2.10" IP
            {
                Gp_IntPtr = PosPrint.POS_IntPtr;
            }
            if (LoadPOSDll.POS_StartDoc())
            {
                byte[] by_SendData = System.Text.Encoding.Default.GetBytes("test print\r\n");
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, by_SendData, (uint)by_SendData.Length);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x0a }, 1);
                LoadPOSDll.POS_EndDoc();
            }
        }
        private static List<byte> esc = new List<byte>();
        //70-1
 // esc
        //20190525
        private byte[] sendesc()
        {
            return esc.ToArray();
        }
        //70-2
 // esc
        //20190525
        private void clearesc()
        {
            esc.Clear();
        }
        //70-3

        //20190525
        private void feedOneLine(byte n)
        {

            byte[] command = new byte[] { (byte)27, (byte)100, n };
            esc.AddRange(command);
        }
        //70-4

        //20190525
        private void addInitializePrinter()
        {
            byte[] command = new byte[] { (byte)27, (byte)64 };
            esc.AddRange(command);
        }
        //70-5
 // 0 ,1 ,2
        //20190525
        public void addSelectJustification(byte i)
        {
            byte[] command = new byte[] { (byte)27, (byte)97, i };
            esc.AddRange(command);
        }
        public void addByte(byte i)
        {
            byte[] command = new byte[] { i };
            esc.AddRange(command);
        }
        //70-6

        //20190528
        public void addText(String text)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(text);
            esc.AddRange(byteArray);
        }

        public enum FOOT
        {
            F2 = 0,
            F5 = 1
        };
        public enum FONT
        {
            FONTA = 0,
            FONTB = 1
        };
        public enum ENABLE
        {
            OFF = 0,
            ON = 1

        };
        public enum HRI_POSITION
        {
            NO_PRINT = 0,
            ABOVE = 1,
            BELOW = 2,
            ABOVE_AND_BELOW = 3
        };
        //70-7

        //20190528
        public void addSelectPrintModes(FONT font, ENABLE emphasized, ENABLE doubleheight, ENABLE doublewidth, ENABLE underline)
        {
            byte temp = 0;
            if (font == FONT.FONTB)
            {
                temp = 1;
            }

            if (emphasized == ENABLE.ON)
            {
                temp = (byte)(temp | 8);
            }

            if (doubleheight == ENABLE.ON)
            {
                temp = (byte)(temp | 16);
            }

            if (doublewidth == ENABLE.ON)
            {
                temp = (byte)(temp | 32);
            }

            if (underline == ENABLE.ON)
            {
                temp = (byte)(temp | 128);
            }

            byte[] command = new byte[] { (byte)27, (byte)33, temp };
            esc.AddRange(command);
        }
        //70-8
 // ( )
        //20190528
        public void addOriginRastBitImage(string bitmapPath, int nWidth, int nMode)
        {
            try
            {
                Bitmap bitmap = new Bitmap(bitmapPath, true);
                byte[] data;

                if (bitmap != null)
                {

                    data = printEscDraw(bitmap);
                    esc.AddRange(data);
                }
                else
                {
                    MessageBox.Show("Failed to send image!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //70-9

        //20190601
        public void addSetBarcodeHeight(byte height)
        {
            byte[] command = new byte[] { (byte)29, (byte)104, height };
            esc.AddRange(command);
        }
        //70-10

        //20190601
        public void addSetBarcodeWidth(byte width)
        {
            byte[] command = new byte[] { (byte)29, (byte)119, (byte)0 };
            if (width > 6)
            {
                width = 6;
            }

            if (width < 2)
            {
                width = 1;
            }

            command[2] = width;
            esc.AddRange(command);
        }

        //70-11

        //20190601
        public void addCODE128(String content)
        {
            byte[] command = new byte[] { (byte)29, (byte)107, (byte)73, (byte)content.Length };
            esc.AddRange(command);
            this.addText(content);
        }
        //70-12

        //20190601

        public void addSelectPrintingPositionForHRICharacters(HRI_POSITION position)
        {
            byte[] command = new byte[] { (byte)29, (byte)72, (byte)position };
            esc.AddRange(command);
        }
        //70-14
 // CodeB
        //20190604
        public String genCodeB(String content)
        {
            String str = "{B";
            str = str + content;
            return str; // {B: 123,66, CODEB
        }
        //70-15

        //20190618
        public void addGeneratePlus(FOOT foot, byte t1, byte t2)
        {
            byte[] command = new byte[] { (byte)27, (byte)112, (byte)foot, t1, t2 };
            esc.AddRange(command);
        }
        //70-16

        //20190702
        public void addBeep(byte num, byte t)
        {
            if (num > 9)
            {
                num = 9;
            }
            byte[] command = new byte[] { (byte)27, (byte)66, num, t };//time = t * 50ms
            esc.AddRange(command);
        }
        //70-17

        //20190702
        public void addCutImmediately()
        {

            byte[] command = new byte[] { 0x1D, 0x56, 0x01 };
            esc.AddRange(command);
        }

        //70-18

        //20190703
        public void addSelectErrorCorrectionLevelForQRCode(byte n)
        {
            byte[] command = new byte[] { (byte)29, (byte)40, (byte)107, (byte)3, (byte)0, (byte)49, (byte)69, n };
            esc.AddRange(command);
        }
        //70-19
 // qrcode
        //20190703
        public void addSelectSizeOfModuleForQRCode(byte n)
        {
            byte[] command = new byte[] { (byte)29, (byte)40, (byte)107, (byte)3, (byte)0, (byte)49, (byte)67, (byte)3 };
            command[7] = n;
            esc.AddRange(command);
        }
        //70-20
 // qrcode
        //20190703

        public void addStoreQRCodeData(String content)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(content);
            byte[] command = new byte[] { (byte)29, (byte)40, (byte)107, (byte)((byteArray.Length + 3) % 256), (byte)((byteArray.Length + 3) / 256), (byte)49, (byte)80, (byte)48 };
            esc.AddRange(command);

            if (!content.Equals(""))
            {
                esc.AddRange(byteArray);
            }

        }
        //70-21
 // qrcode
        //20190703
        public void addPrintQRCode()
        {
            byte[] command = new byte[] { (byte)29, (byte)40, (byte)107, (byte)3, (byte)0, (byte)49, (byte)81, (byte)48 };
            esc.AddRange(command);
        }


        //71

        //20190601

        public static byte[] printEscDraw(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            byte[] bitbuf = new byte[width / 8];
            byte[] imgbuf = new byte[width / 8 * height + 8];


            imgbuf[0] = 29;
            imgbuf[1] = 118;
            imgbuf[2] = 48;
            imgbuf[3] = 0;
            imgbuf[4] = (byte)(width / 8);
            imgbuf[5] = 0;
            imgbuf[6] = (byte)(height % 256);
            imgbuf[7] = (byte)(height / 256);
            int s = 7;


            for (int i = 0; i < height; ++i)
            {
                int t;
                for (t = 0; t < width / 8; ++t)
                {
                    Color c0 = bitmap.GetPixel(t * 8, i);
                    byte p0, gray0 = 0;
                    gray0 = (byte)((c0.R * 30 + c0.G * 59 + c0.B * 11) / 100);
                    if (gray0 >= 255)
                    {
                        p0 = 0;
                    }
                    else
                    {
                        p0 = 1;
                    }

                    Color c1 = bitmap.GetPixel(t * 8 + 1, i);
                    byte p1, gray1 = 0;
                    gray1 = (byte)((c1.R * 30 + c1.G * 59 + c1.B * 11) / 100);
                    if (gray1 >= 255)
                    {
                        p1 = 0;
                    }
                    else
                    {
                        p1 = 1;
                    }

                    Color c2 = bitmap.GetPixel(t * 8 + 2, i);
                    byte p2, gray2 = 0;
                    gray2 = (byte)((c2.R * 30 + c2.G * 59 + c2.B * 11) / 100);
                    if (gray2 >= 255)
                    {
                        p2 = 0;
                    }
                    else
                    {
                        p2 = 1;
                    }

                    Color c3 = bitmap.GetPixel(t * 8 + 3, i);
                    byte p3, gray3 = 0;
                    gray3 = (byte)((c3.R * 30 + c3.G * 59 + c3.B * 11) / 100);
                    if (gray3 >= 255)
                    {
                        p3 = 0;
                    }
                    else
                    {
                        p3 = 1;
                    }
                    Color c4 = bitmap.GetPixel(t * 8 + 4, i);
                    byte p4, gray4 = 0;
                    gray4 = (byte)((c4.R * 30 + c4.G * 59 + c4.B * 11) / 100);
                    if (gray4 >= 255)
                    {
                        p4 = 0;
                    }
                    else
                    {
                        p4 = 1;
                    }

                    Color c5 = bitmap.GetPixel(t * 8 + 5, i);
                    byte p5, gray5 = 0;
                    gray5 = (byte)((c5.R * 30 + c5.G * 59 + c5.B * 11) / 100);
                    if (gray5 >= 255)
                    {
                        p5 = 0;
                    }
                    else
                    {
                        p5 = 1;
                    }

                    Color c6 = bitmap.GetPixel(t * 8 + 6, i);
                    byte p6, gray6 = 0;
                    gray6 = (byte)((c6.R * 30 + c6.G * 59 + c6.B * 11) / 100);
                    if (gray6 >= 255)
                    {
                        p6 = 0;
                    }
                    else
                    {
                        p6 = 1;
                    }

                    Color c7 = bitmap.GetPixel(t * 8 + 7, i);
                    byte p7, gray7 = 0;
                    gray7 = (byte)((c7.R * 30 + c7.G * 59 + c7.B * 11) / 100);
                    if (gray7 >= 255)
                    {
                        p7 = 0;
                    }
                    else
                    {
                        p7 = 1;
                    }

                    int value = p0 * 128 + p1 * 64 + p2 * 32 + p3 * 16 + p4 * 8 + p5 * 4 + p6 * 2 + p7;
                    bitbuf[t] = (byte)value;
                }

                for (t = 0; t < width / 8; ++t)
                {
                    s++;
                    imgbuf[s] = bitbuf[t];
                }
            }

            return imgbuf;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewUsb.FindUSBPrinter();
            for (int i = 0; i < NewUsb.USBPortCount; i++)
            {
                if (NewUsb.LinkUSB(i))
                {
                    List<byte> textData = new List<byte>();
                    int ii;
                    addInitializePrinter();
                    addSelectJustification(0);
                    addByte(0x1C);
                    addByte(0x2E);
                    addText("[ASCII characters]" + "\n ");
                    for (ii = 0x20; ii < 127; ii++)
                    {
                        addByte((byte)ii);
                    }
                    addByte(0x0a);
                    addText("[Codepage characters]" + "\n  ");
                    addByte(0x1C);
                    addByte(0x2E);
                    for (ii = 128; ii < 256; ii++)
                    {
                        addByte((byte)ii);
                    }
                    addByte(0x0a);
                    addByte(0x1C);
                    addByte(0x26);


                    addByte(0x1F);
                    addByte(0x1B);
                    addByte(0x1F);
                    addByte(0xFB);
                    addByte(0x01);

                    addByte(0x1C);
                    addByte(0x26);


                        addByte(0x1C);
                        addByte(0x2E);
                        addText("[Simplified Chinese GB18030]" + "\n  ");
                        addByte(0x1F);
                        addByte(0x1B);
                        addByte(0x1F);
                        addByte(0xEF);
                        addByte(0x11);
                        addByte(0x12);
                        addByte(0x13);
                        addByte(0x11);
                        addByte(0x1C);
                        addByte(0x26);
                        for (ii = 0; ii < 23; ii++)
                        {
                            addByte(0xB0);
                            addByte((byte)(ii + 0xA1));
                        }
                        for (ii = 0; ii < 24; ii++)
                        {
                            addByte(0xB0);
                            addByte((byte)(ii + 0xA2));
                        }
                        for (ii = 0; ii < 24; ii++)
                        {
                            addByte(0xB0);
                            addByte((byte)(ii + 0xA3));
                        }
                        addByte(0x0a);
                    
                    addByte(0x1C);
                    addByte(0x2E);

                    feedOneLine(1);

                    addSelectJustification(1);
                    addText(DateTime.Now.ToString() + "\n");
                    feedOneLine(1);
                    addInitializePrinter();

                    addSelectJustification(1);
                    addSelectErrorCorrectionLevelForQRCode((byte)0x31);
 // qrcode
                    addSelectSizeOfModuleForQRCode((byte)8);
 // qrcode
                    addStoreQRCodeData("Thanks to use our printers!");
                    addPrintQRCode(); // QRCode
                    addText("\n" + "Thanks to use our printers!" + "\n\n");

                    feedOneLine(3);
                    addCutImmediately();
                    addInitializePrinter();
                    addBeep(3, 2);
                    SendData2USB(sendesc());
                    clearesc();
                    NewUsb.CloseUSBPort();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
        	NewUsb.FindUSBPrinter();
        	for(int i=0;i<NewUsb.USBPortCount;i++)
        	{
        		if(NewUsb.LinkUSB(i))
        		{
        			SendData2USB("SIZE 58 mm,60 mm\r\n");
        			SendData2USB("GAP 0 mm,0 mm\r\n");
        			SendData2USB("DENSITY 7\r\n");
        			SendData2USB("REFERENCE 0,0\r\n");
        			SendData2USB("TEXT 15,10,\"1\",0,1,1,\"asdfggDDD\"\r\n");
        			SendData2USB("TEXT 15,60,\"TSS24.BF2\",0,1,1,\"简体字\"\r\n");
        			
        			StreamReader strReadFile=new StreamReader(@"./10.bmp");
        			byte[] byteReadData=new byte[strReadFile.BaseStream.Length];
        			strReadFile.BaseStream.Read(byteReadData,0,byteReadData.Length);
        			strReadFile.Close();
        			SendData2USB("DOWNLOAD\"10.bmp\",4096,");
        			SendData2USB(byteReadData); // bmp
        			SendData2USB("PUTBMP 14,110,\"10.bmp\"\r\n");
        			SendData2USB("PRINT 1\r\n");
        			
        			NewUsb.CloseUSBPort();
        		}
        	}
        }
        private void SendData2USB(byte[] str)
        {
        	NewUsb.SendData2USB(str,str.Length);
        }
        private void SendData2USB(string str)
        {
        	byte[] by_SendData=System.Text.Encoding.Default.GetBytes(str);
        	SendData2USB(by_SendData);
        }
        private void button4_Click(object sender, EventArgs e)
        {
					if(serialPort1==null)
					{
						serialPort1=new SerialPort();
						serialPort1.RtsEnable=true;
					}
					if(serialPort1.IsOpen)
					{
						serialPort1.Close();
					}
					serialPort1.PortName="COM1";
					serialPort1.BaudRate=19200;
					serialPort1.Parity=(Parity)Enum.Parse(typeof(Parity),"None");
					serialPort1.DataBits=8;
					serialPort1.StopBits=(StopBits)Enum.Parse(typeof(StopBits),"One");
					serialPort1.ReadBufferSize=100;
					serialPort1.WriteBufferSize=2048;
					serialPort1.ReadTimeout=100;
					serialPort1.WriteTimeout=10000;
          LoadPOSDll PosPrint = new LoadPOSDll();
          if(PosPrint.OpenComPort(ref serialPort1))
          {
          	LoadPOSDll.POS_Reset();
          	IntPtr[] pathPtr=new IntPtr[4];
          	pathPtr[0]=Marshal.StringToHGlobalAnsi(@"./09.bmp");
          	pathPtr[1]=Marshal.StringToHGlobalAnsi(@"./08.bmp");
          	pathPtr[2]=Marshal.StringToHGlobalAnsi(@"./07.bmp");
          	pathPtr[3]=Marshal.StringToHGlobalAnsi(@"./06.bmp");
          	PosPrint.DownloadBmpsToFlash(pathPtr,4);
          	
          	LoadPOSDll.POS_EndDoc();
          	Marshal.FreeHGlobal(pathPtr[0]);
          	Marshal.FreeHGlobal(pathPtr[1]);
          	Marshal.FreeHGlobal(pathPtr[2]);
          	Marshal.FreeHGlobal(pathPtr[3]);
          	PosPrint.ClosePrinterPort();
          }
        }
        private void button5_Click(object sender, EventArgs e)
        {
			if(serialPort1==null)
			{
			serialPort1=new SerialPort();
			serialPort1.RtsEnable=true;
			}
			if(serialPort1.IsOpen)
			{
			serialPort1.Close();
			}
			serialPort1.PortName="COM1";
			serialPort1.BaudRate=19200;
			serialPort1.Parity=(Parity)Enum.Parse(typeof(Parity),"None");
			serialPort1.DataBits=8;
			serialPort1.StopBits=(StopBits)Enum.Parse(typeof(StopBits),"One");
			serialPort1.ReadBufferSize=100;
			serialPort1.WriteBufferSize=2048;
			serialPort1.ReadTimeout=100;
			serialPort1.WriteTimeout=10000;
          LoadPOSDll PosPrint = new LoadPOSDll();
          if(PosPrint.OpenComPort(ref serialPort1))
          {
          	LoadPOSDll.POS_Reset();
          	PosPrint.PrintBmpInFlash(1,0,0x00);
          	PosPrint.PrintBmpInFlash(2,0,0x00);
          	PosPrint.PrintBmpInFlash(3,0,0x00);
          	PosPrint.PrintBmpInFlash(4,0,0x00);
          	PosPrint.ClosePrinterPort();
          }
        }

        private void btCheckMarkGo_Click(object sender, EventArgs e)
        {
            bool printpoint = true;
            bool front = true;
            int resultLength = int.Parse(tbCheckMarkGo.Text);

            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                byte[] cmddata1 = { 0X1D, 0X54 };
                byte[] cmddata2 = { 0X1D, 0X28, 0x46, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00 };

                if (printpoint == true)
                    cmddata2[5] = 0x01;
                else
                    cmddata2[5] = 0x02;

                if (front == true)
                    cmddata2[6] = 0x00;
                else
                    cmddata2[6] = 0x01;

                cmddata2[7] = (byte)Math.Round((resultLength / 0.176) % 256);
                cmddata2[8] = (byte)((resultLength / 0.176) / 256);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, cmddata2, (uint)cmddata2.Length);

                PosPrint.ClosePrinterPort();
            }
        }

        private void btCheckMarkBack_Click(object sender, EventArgs e)
        {
            bool printpoint = true;
            bool front = false;
            int resultLength = int.Parse(tbCheckMarkBack.Text);

            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                byte[] cmddata1 = { 0X1D, 0X54 };
                byte[] cmddata2 = { 0X1D, 0X28, 0x46, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00 };

                if (printpoint == true)
                    cmddata2[5] = 0x01;
                else
                    cmddata2[5] = 0x02;

                if (front == true)
                    cmddata2[6] = 0x00;
                else
                    cmddata2[6] = 0x01;

                cmddata2[7] = (byte)Math.Round((resultLength / 0.176) % 256);
                cmddata2[8] = (byte)((resultLength / 0.176) / 256);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, cmddata2, (uint)cmddata2.Length);

                PosPrint.ClosePrinterPort();
            }
        }

        private void btCutMarkGo_Click(object sender, EventArgs e)
        {
            bool printpoint = false;
            bool front = true;
            int resultLength = int.Parse(tbCutMarkGo.Text);

            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                byte[] cmddata1 = { 0X1D, 0X54 };
                byte[] cmddata2 = { 0X1D, 0X28, 0x46, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00 };

                if (printpoint == true)
                    cmddata2[5] = 0x01;
                else
                    cmddata2[5] = 0x02;

                if (front == true)
                    cmddata2[6] = 0x00;
                else
                    cmddata2[6] = 0x01;

                cmddata2[7] = (byte)Math.Round((resultLength / 0.176) % 256);
                cmddata2[8] = (byte)((resultLength / 0.176) / 256);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, cmddata2, (uint)cmddata2.Length);

                PosPrint.ClosePrinterPort();
            }
        }

        private void btCutMarkBack_Click(object sender, EventArgs e)
        {
            bool printpoint = false;
            bool front = false;
            int resultLength = int.Parse(tbCutMarkBack.Text);

            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                byte[] cmddata1 = { 0X1D, 0X54 };
                byte[] cmddata2 = { 0X1D, 0X28, 0x46, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00 };

                if (printpoint == true)
                    cmddata2[5] = 0x01;
                else
                    cmddata2[5] = 0x02;

                if (front == true)
                    cmddata2[6] = 0x00;
                else
                    cmddata2[6] = 0x01;

                cmddata2[7] = (byte)Math.Round((resultLength / 0.176) % 256);
                cmddata2[8] = (byte)((resultLength / 0.176) / 256);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, cmddata2, (uint)cmddata2.Length);

                PosPrint.ClosePrinterPort();
            }
        }

        private void btPrintGoNext_Click(object sender, EventArgs e)
        {
            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                LoadPOSDll.POS_Reset();

                byte[] data1 = System.Text.Encoding.Default.GetBytes("开始打印");
                byte[] data2 = System.Text.Encoding.Default.GetBytes("然后走纸到下一个起始打印位置");
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, data1, (uint)data1.Length);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[]{0x0a}, 1);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, data2, (uint)data2.Length);

                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1B, 0x64, 0x05, 0x0A }, 4); // n (0x05 )
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1D, 0x54, 0x1D, 0x0C }, 4); // (1D 0C)
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x0D }, 1);

                PosPrint.ClosePrinterPort();
            }
        }

        private void btPrintGoNextCut_Click(object sender, EventArgs e)
        {
            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 100;
            serialPort1.WriteBufferSize = 2048;
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                LoadPOSDll.POS_Reset();

                byte[] data1 = System.Text.Encoding.Default.GetBytes("开始打印");
                byte[] data2 = System.Text.Encoding.Default.GetBytes("然后走纸到下一个/撕纸位置");
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, data1, (uint)data1.Length);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x0a }, 1);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, data2, (uint)data2.Length);

                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1B, 0x64, 0x05, 0x0A }, 4); // n (0x05 )
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1D, 0x54, 0x1D, 0x56, 0x30 }, 5); // (1D 56)30 31
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x0D }, 1);

                PosPrint.ClosePrinterPort();
            }
        }
 // pString=
 // module_size=
        private byte[] QRCODE_cmd_data_create(string pString, byte module_size)
        {
            byte mds = 6;
            if (module_size > 2 && module_size < 15)
                mds = module_size;
            byte[] data1 = { 0x1d, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x43, mds };

            int len = (pString.Length + 3);
            byte[] len2 = new byte[2];
            len2[0] = (byte)len;
            len2[1] = (byte)(len >> 8);
            byte[] data2 = new byte[] { 0x1d, 0x28, 0x6b, len2[0], len2[1], 0x31, 0x50, 0x30};

            byte[] data3 = System.Text.Encoding.Default.GetBytes(pString);

            byte[] data4 = { 0x1d, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x51, 0x30 };

            byte[] data = new byte[data1.Length+ data2.Length+ data3.Length+ data4.Length];

            int i = 0;
            for(int j = 0;j < data1.Length; j++)
            {
                data[i++] = data1[j];
            }
            for (int j = 0; j < data2.Length; j++)
            {
                data[i++] = data2[j];
            }
            for (int j = 0; j < data3.Length; j++)
            {
                data[i++] = data3[j];
            }
            for (int j = 0; j < data4.Length; j++)
            {
                data[i++] = data4[j];
            }
            return data;
        }

        private void btSerialPrintTest_Click(object sender, EventArgs e)
        {
            if (serialPort1 == null)
            {
                serialPort1 = new SerialPort();
                serialPort1.RtsEnable = true;
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.PortName = cbbCOM.SelectedItem.ToString();
            if(serialPort1.PortName == null)
            {
                MessageBox.Show("端口无效");
                return;
            }
                
            serialPort1.BaudRate = 19200;
            serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            serialPort1.ReadBufferSize = 512;
            serialPort1.WriteBufferSize = 128*1024;//128K
            serialPort1.ReadTimeout = 100;
            serialPort1.WriteTimeout = 10000;
 // serialPort1.Handshake = (Handshake)Enum.Parse(typeof(Handshake), "RequestToSend");// TTL/232 RequestToSend
            serialPort1.Handshake = (Handshake)Enum.Parse(typeof(Handshake), "None"); // USB None
            LoadPOSDll PosPrint = new LoadPOSDll();
            if (PosPrint.OpenComPort(ref serialPort1))
            {
                byte[] status = { 255 };
                byte[] tmp = { 0 };

                Thread.Sleep(100); // 100ms
                     

                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01}, 3); // 100401
                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 100); // 1 , 100ms
                if(status[0] == 255)
                {
                    PosPrint.ClosePrinterPort();
                    if (PosPrint.OpenComPort(ref serialPort1))
                    {
                        Thread.Sleep(100); // 100ms

                        status[0] = 255;
                        LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                        LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 100); // 1 , 100ms
                        if (status[0] == 255)
                        {
                            PosPrint.ClosePrinterPort();
                            if (PosPrint.OpenComPort(ref serialPort1))
                            {
                                Thread.Sleep(100); // 100ms

                                status[0] = 255;
                                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 100); // 1 , 100ms
                                if (status[0] == 255)
                                {
                                    PosPrint.ClosePrinterPort();
                                    MessageBox.Show("查询打印机状态超时，可能是打印机缓存已满，USB通道已阻塞");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("端口打开失败");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("端口打开失败");
                        return;
                    }
                }

 // 0x11,0x13,0x00
                if (status[0] == 0x11 || status[0] == 0x13 || status[0] == 0x00)
                {
                   while(true)
                    {
                        status[0] = 0xff;//20220715,Fred
                        LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 100); // 1 , 100ms
                        if (status[0] == 255) // 20220715,Fred
                        {
                            PosPrint.ClosePrinterPort();
                            MessageBox.Show("查询打印机状态超时，可能是打印机缓存已满，USB通道已阻塞");
                            return;
                        }
                        if (status[0] != 0x11 && status[0] != 0x13 && status[0] != 0x00)
                            break;
                    }
                }

                tmp[0] = 0x08;
                tmp[0] &= status[0];
                if (tmp[0] > 0)
                {

                    status[0] = 255;
                    LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x02 }, 3); // 100402
                    LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500); // 1 , 500ms
                  
                    if (status[0] == 255)
                    {
                        PosPrint.ClosePrinterPort();
                        MessageBox.Show("查询脱机状态超时，可能是硬件异常");
                        return;
                    }

                    tmp[0] = 1 << 2;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {                     
                        MessageBox.Show("打印机翻盖未盖好");
                    }
                    tmp[0] = 1 << 5;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {
                        MessageBox.Show("打印机缺纸");
                    }
                    tmp[0] = 1 << 6;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {
                        MessageBox.Show("打印机发生错误（切刀错误、堵纸错误或者过热报错）");
                    }
                    while (true)
                    {
                        Thread.Sleep(500); // 500ms

                        status[0] = 255;
                        LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                        LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500); // 1 , 500ms
                        if (status[0] == 255)
                        {
                            PosPrint.ClosePrinterPort();
                            MessageBox.Show("查询打印机状态超时，可能是硬件异常");
                            return;
                        }
                        tmp[0] = 0x08;
                        tmp[0] &= status[0];
                        if (tmp[0] == 0)
                            break;
                    }
                }

                LoadPOSDll.POS_Reset();

                LoadPOSDll.POS_SetMotionUnit(180, 180);

                LoadPOSDll.POS_SetRightSpacing(0);

                LoadPOSDll.POS_SetLineSpacing(100);
                LoadPOSDll.POS_S_TextOut("POS Thermal Printer", 50, 2, 3, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);

                LoadPOSDll.POS_SetLineSpacing(35);

                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_S_TextOut("热敏票据打印机", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_THICK_UNDERLINE);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_S_TextOut("热 敏 票 据 打 印 机", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_THIN_UNDERLINE);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_SetLineSpacing(24);



                LoadPOSDll.POS_SetRightSpacing(0);
                LoadPOSDll.POS_S_TextOut("80MI Printer", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_S_TextOut("POS Thermal Printer", 220, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_SetRightSpacing(2);
                LoadPOSDll.POS_S_TextOut("80MI Printer", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_S_TextOut("POS Thermal Printer", 220, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_SetRightSpacing(4);
                LoadPOSDll.POS_S_TextOut("80MI Printer", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_S_TextOut("POS Thermal Printer", 220, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();



                LoadPOSDll.POS_SetRightSpacing(2);
                LoadPOSDll.POS_S_TextOut("正常字体打印", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_S_TextOut("反显字体打印", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_REVERSE);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_S_TextOut("顺时针旋转90度字体打印", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_CLOCKWISE_90);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_S_TextOut("倒置字体打印", 20, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE,
                    LoadPOSDll.POS_FONT_STYLE_UPSIDEDOWN);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();




                LoadPOSDll.POS_SetRightSpacing(0);

                LoadPOSDll.POS_S_TextOut("----------------------------------", 50, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_S_TextOut("Barcode - Code 128", 160, 1, 1, LoadPOSDll.POS_FONT_TYPE_COMPRESSED,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_S_SetBarcode("{A*1234ABCDE*{C5678", 50, LoadPOSDll.POS_BARCODE_TYPE_CODE128,
                    2, 50, LoadPOSDll.POS_FONT_TYPE_COMPRESSED, LoadPOSDll.POS_HRI_POSITION_BOTH, 19);
                LoadPOSDll.POS_FeedLine();

                LoadPOSDll.POS_S_TextOut("----------------------------------", 50, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD,
                    LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_FeedLine();


                LoadPOSDll.POS_S_TextOut("QRCode Print Test", 160, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1B, 0x61, 0x01 }, 3);
                byte[] qrcode_cmd_data = QRCODE_cmd_data_create("www.Hello world 0123456.com", 6);
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, qrcode_cmd_data, (uint)qrcode_cmd_data.Length);
                LoadPOSDll.POS_FeedLine();
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1B, 0x61, 0x00 }, 3);


                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1D, 0x56, 0x42, 0x00 }, 4);
            

                status[0] = 255;
                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 5000); // 1 , 5
                if (status[0] == 255)
                {
                    PosPrint.ClosePrinterPort();
                    MessageBox.Show("查询打印机状态超时,可能是打印中断使USB通道阻塞，或者是硬件异常");
                    return;
                }
                tmp[0] = 0x08;
                tmp[0] &= status[0];
                if (tmp[0] > 0)
                {

                    status[0] = 255;
                    LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x02 }, 3); // 100402
                    LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500);
            
                    if (status[0] == 255)
                    {
                        PosPrint.ClosePrinterPort();
                        MessageBox.Show("查询脱机状态超时，可能是硬件异常");
                        return;
                    }

                    tmp[0] = 1 << 2;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {
                        MessageBox.Show("打印机翻盖未盖好");
                    }
                    tmp[0] = 1 << 5;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {
                        MessageBox.Show("打印机缺纸");
                    }
                    tmp[0] = 1 << 6;
                    tmp[0] &= status[0];
                    if (tmp[0] > 0)
                    {
                        MessageBox.Show("打印机发生错误（切刀错误、堵纸错误或者过热报错）");
                    }
                    while (true)
                    {
                        Thread.Sleep(500); // 500ms

                        status[0] = 255;
                        LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                        LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500); // 1 , 500ms
                        if (status[0] == 255)
                        {
                            PosPrint.ClosePrinterPort();
                            MessageBox.Show("查询打印机状态超时，可能是硬件异常");
                            return;
                        }
                        tmp[0] = 0x08;
                        tmp[0] &= status[0];
                        if (tmp[0] == 0)
                            break;
                    }
                }

                LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x1D, 0x72, 0x01}, 3);
                status[0] = 255;
                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 5000);
                if (status[0] == 255)
                {

                        status[0] = 255;
                        LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x02 }, 3); // 100402
                        LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500);
                      
                        if (status[0] == 255)
                        {
                            PosPrint.ClosePrinterPort();
                            MessageBox.Show("查询脱机状态超时，可能是硬件异常");
                            return;
                        }

                        tmp[0] = 1 << 2;
                        tmp[0] &= status[0];
                        if (tmp[0] > 0)
                        {
                            MessageBox.Show("打印机开盖导致打印中断");
                        }
                        tmp[0] = 1 << 5;
                        tmp[0] &= status[0];
                        if (tmp[0] > 0)
                        {
                            MessageBox.Show("打印机缺纸导致打印中断");
                        }
                        tmp[0] = 1 << 6;
                        tmp[0] &= status[0];
                        if (tmp[0] > 0)
                        {
                            MessageBox.Show("打印机发生错误（切刀错误、堵纸错误或者过热报错）导致打印中断");
                        }
                        while (true)
                        {
                            Thread.Sleep(500); // 500ms

                            status[0] = 255;
                            LoadPOSDll.POS_WriteFile(PosPrint.POS_IntPtr, new byte[] { 0x10, 0x04, 0x01 }, 3); // 100401
                            LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500); // 1 , 500ms
                            if (status[0] == 255)
                            {
                                PosPrint.ClosePrinterPort();
                                MessageBox.Show("查询打印机状态超时，可能是硬件异常");
                                return;
                            }
                            else if(status[0] == 0x00)
                            {
                                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 500); // 1 , 500ms
                                break;
                            }
                            tmp[0] = 0x08;
                            tmp[0] &= status[0];
                            if (tmp[0] == 0)
                            {
                                LoadPOSDll.POS_ReadFile(PosPrint.POS_IntPtr, status, 1, 5000); // 1 , 500ms
                                break;
                            }
                        }
                }
                MessageBox.Show("打印完成");

                PosPrint.ClosePrinterPort();
            }
            else
            {
                MessageBox.Show("端口打开失败");
                return;
            }
        }

        private void gb76BlackMark_Enter(object sender, EventArgs e)
        {

        }
    }
}
