package com.gpsdk.demo;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.hardware.usb.UsbDevice;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

import com.printer.sdk.io.BluetoothPort;
import com.printer.sdk.io.EthernetPort;
import com.printer.sdk.io.PortManager;
import com.printer.sdk.io.SerialPort;
import com.printer.sdk.io.UsbPort;

import java.io.IOException;
import java.util.Vector;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_DETACHED;


public class DeviceConnFactoryManager {

    static private PortManager mPort;

    private static final String TAG = DeviceConnFactoryManager.class.getSimpleName();

    private CONN_METHOD connMethod;

    private String ip;

    private int port;

    private String macAddress;

    private UsbDevice mUsbDevice;

    private Context mContext;

    private String serialPortPath;

    private int baudrate;

    private int id;

    private static DeviceConnFactoryManager[] deviceConnFactoryManagers = new DeviceConnFactoryManager[4];

    static private boolean isOpenPort = false;

    private byte[] esc = {0x10, 0x04, 0x02};


    private byte[] ASB = {0x1D, 0x61, 0x0C};

    private byte[] ASB_OFF = {0x1D, 0x61, 0x00};

    private static final int ESC_STATE_PAPER_ERR = 0x20;

    private static final int ESC_STATE_COVER_OPEN = 0x04;

    private static final int ESC_STATE_ERR_OCCURS = 0x40;

    private static final int ESC_STATE_NORMAL = 0x60;


    private static final int ESC_ASB_STATE_NORMAL = 0x00;

    private static final int ESC_ASB_2_STATE_PAPER_ERR = 0x0C;

    private static final int ESC_ASB_1_STATE_UNRECOVERABLE_ERR = 0x20;

    private static final int ESC_ASB_1_STATE_RECOVERABLE_ERR = 0x40;


    private byte[] tsc = {0x1b, '!', '?'};


    private static final int TSC_STATE_PAPER_ERR = 0x04;


    private static final int TSC_STATE_COVER_OPEN = 0x01;


    private static final int TSC_STATE_ERR_OCCURS = 0x80;
    private byte[] sendCommand;

    private PrinterCommand currentPrinterCommand;
    public static final byte FLAG = 0x10;
    public static final byte FLAG_ASB = 0x02;
    private static final int READ_DATA = 10000;
    private static final String READ_DATA_CNT = "read_data_cnt";
    private static final String READ_BUFFER_ARRAY = "read_buffer_array";
    public static final String ACTION_CONN_STATE = "action_connect_state";
    public static final String ACTION_QUERY_PRINTER_STATE = "action_query_printer_state";
    public static final String STATE = "state";
    public static final String DEVICE_ID = "id";
    public static final int CONN_STATE_DISCONNECT = 0x90;
    public static final int CONN_STATE_CONNECTING = CONN_STATE_DISCONNECT << 1;
    public static final int CONN_STATE_FAILED = CONN_STATE_DISCONNECT << 2;
    public static final int CONN_STATE_CONNECTED = CONN_STATE_DISCONNECT << 3;
    public static final int ESC_PRINTER_STATE_PAPER_ERR = 0x0A;
    public static final int ESC_PRINTER_STATE_ERR_OCCURS = 0x0B;
    private PrinterReader reader;

    public enum CONN_METHOD {

        BLUETOOTH("BLUETOOTH"),

        USB("USB"),

        WIFI("WIFI"),

        SERIAL_PORT("SERIAL_PORT");

        private String name;

        private CONN_METHOD(String name) {
            this.name = name;
        }

        @Override
        public String toString() {
            return this.name;
        }
    }

    public static DeviceConnFactoryManager[] getDeviceConnFactoryManagers() {
        return deviceConnFactoryManagers;
    }

    /**
     * 打开端口
     *
     * @return
     */
    public void openPort() {
        sendStateBroadcast(CONN_STATE_CONNECTING);
        switch (deviceConnFactoryManagers[id].connMethod) {
            case BLUETOOTH:
                System.out.println("id -> " + id);
                mPort = new BluetoothPort(macAddress);
                isOpenPort = deviceConnFactoryManagers[id].mPort.openPort();

                break;
            case USB:
                mPort = new UsbPort(mContext, mUsbDevice);
                isOpenPort = mPort.openPort();

                if (isOpenPort) {
                    IntentFilter filter = new IntentFilter(ACTION_USB_DEVICE_DETACHED);
                    mContext.registerReceiver(usbStateReceiver, filter);
                }
                break;
            case WIFI:
                mPort = new EthernetPort(ip, port);
                isOpenPort = mPort.openPort();
                break;
            case SERIAL_PORT:
                mPort = new SerialPort(serialPortPath, baudrate, 0);
                isOpenPort = mPort.openPort();
                break;
            default:
                break;
        }

        if (isOpenPort) {
            queryCommand();
            sendStateBroadcast(CONN_STATE_CONNECTED);
        } else {
            sendStateBroadcast(CONN_STATE_FAILED);
        }
    }


    private void queryCommand() {

        if(reader == null) {
            reader = new PrinterReader();
            reader.start();
        }
        else {
            reader.begin();
            reader.start();
        }
        currentPrinterCommand = PrinterCommand.ESC;
        sendStateBroadcast(CONN_STATE_CONNECTED);
//        queryPrinterCommand();
    }


    public CONN_METHOD getConnMethod() {
        return connMethod;
    }


    static public boolean getConnState() {

        return isOpenPort;
    }

    public void queryPrinterState() {
        queryPrinterCommand();
    }

    public void turnOnASB() {
        if(!this.isOpenPort) {
            sendStateBroadcast(CONN_STATE_DISCONNECT);
            return;
        }
        Vector<Byte> data = new Vector<>(ASB.length);
        for (int i = 0; i < ASB.length; i++) {
            data.add(ASB[i]);
        }
        sendDataImmediately(data);
    }

    public void turnOffASB() {
        if(!this.isOpenPort) {
            sendStateBroadcast(CONN_STATE_DISCONNECT);
            return;
        }
        Vector<Byte> data = new Vector<>(ASB_OFF.length);
        for (int i = 0; i < ASB_OFF.length; i++) {
            data.add(ASB_OFF[i]);
        }
        sendDataImmediately(data);
    }

    public String getMacAddress() {
        return macAddress;
    }


    public int getPort() {
        return port;
    }


    public String getIp() {
        return ip;
    }


    public UsbDevice usbDevice() {
        return mUsbDevice;
    }


    public void closePort(int id) {
        if (this.mPort != null) {
            System.out.println("id -> " + id);
            this.mPort.closePort();
            if(reader != null) {reader.cancel();}
            isOpenPort = false;
            currentPrinterCommand = null;
        }
        sendStateBroadcast(CONN_STATE_DISCONNECT);
    }


    public String getSerialPortPath() {
        return serialPortPath;
    }


    public int getBaudrate() {
        return baudrate;
    }

    public static void closeAllPort() {
        for (DeviceConnFactoryManager deviceConnFactoryManager : deviceConnFactoryManagers) {
            if (deviceConnFactoryManager != null) {
                Log.e(TAG, "cloaseAllPort() id -> " + deviceConnFactoryManager.id);
                deviceConnFactoryManager.closePort(deviceConnFactoryManager.id);
                deviceConnFactoryManagers[deviceConnFactoryManager.id] = null;
            }
        }
    }

    private DeviceConnFactoryManager(Build build) {
        this.connMethod = build.connMethod;
        this.macAddress = build.macAddress;
        this.port = build.port;
        this.ip = build.ip;
        this.mUsbDevice = build.usbDevice;
        this.mContext = build.context;
        this.serialPortPath = build.serialPortPath;
        this.baudrate = build.baudrate;
        this.id = build.id;
        deviceConnFactoryManagers[id] = this;
    }


    public PrinterCommand getCurrentPrinterCommand() {
        return deviceConnFactoryManagers[id].currentPrinterCommand;
    }

    public static final class Build {
        private String ip;
        private String macAddress;
        private UsbDevice usbDevice;
        private int port;
        private CONN_METHOD connMethod;
        private Context context;
        private String serialPortPath;
        private int baudrate;
        private int id;

        public DeviceConnFactoryManager.Build setIp(String ip) {
            this.ip = ip;
            return this;
        }

        public DeviceConnFactoryManager.Build setMacAddress(String macAddress) {
            this.macAddress = macAddress;
            return this;
        }

        public DeviceConnFactoryManager.Build setUsbDevice(UsbDevice usbDevice) {
            this.usbDevice = usbDevice;
            return this;
        }

        public DeviceConnFactoryManager.Build setPort(int port) {
            this.port = port;
            return this;
        }

        public DeviceConnFactoryManager.Build setConnMethod(CONN_METHOD connMethod) {
            this.connMethod = connMethod;
            return this;
        }

        public DeviceConnFactoryManager.Build setContext(Context context) {
            this.context = context;
            return this;
        }

        public DeviceConnFactoryManager.Build setId(int id) {
            this.id = id;
            return this;
        }

        public DeviceConnFactoryManager.Build setSerialPort(String serialPortPath) {
            this.serialPortPath = serialPortPath;
            return this;
        }

        public DeviceConnFactoryManager.Build setBaudrate(int baudrate) {
            this.baudrate = baudrate;
            return this;
        }

        public DeviceConnFactoryManager build() {
            return new DeviceConnFactoryManager(this);
        }
    }

    public void sendDataImmediately(final Vector<Byte> data) {
        if (this.mPort == null) {
            return;
        }
        try {
            Log.e(TAG, "data -> " + new String(com.printer.sdk.utils.Utils.convertVectorByteTobytes(data), "gb2312"));
            this.mPort.writeDataImmediately(data, 0, data.size());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public int readDataImmediately(byte[] buffer) throws IOException {
        return this.mPort.readData(buffer);
    }


    private void queryPrinterCommand() {
        ThreadPool.getInstantiation().addTask(new Runnable() {
            @Override
            public void run() {
                sendCommand = esc;
                Vector<Byte> data = new Vector<>(esc.length);
                for (int i = 0; i < esc.length; i++) {
                    data.add(esc[i]);
                }
                sendDataImmediately(data);

                final ThreadFactoryBuilder threadFactoryBuilder = new ThreadFactoryBuilder("Timer");
                final ScheduledExecutorService scheduledExecutorService = new ScheduledThreadPoolExecutor(1, threadFactoryBuilder);
                scheduledExecutorService.schedule(threadFactoryBuilder.newThread(new Runnable() {
                    @Override
                    public void run() {
                        if (currentPrinterCommand == null) {
                            if (reader != null) {
                                reader.cancel();
                                mPort.closePort();
                                isOpenPort = false;
                                sendStateBroadcast(CONN_STATE_FAILED);
                            }
                        }
                    }
                }), 2000, TimeUnit.MILLISECONDS);

            }
        });
    }

    class PrinterReader extends Thread {
       private boolean isRun = false;

        private byte[] buffer = new byte[100];

        public PrinterReader() {
            isRun = true;
        }
        public void begin(){ isRun = true;}
        @Override
        public void run() {
            try {
                while (isRun) {

                    int len = readDataImmediately(buffer);
                    if (len > 0) {
                        Message message = Message.obtain();
                        message.what = READ_DATA;
                        Bundle bundle = new Bundle();
                        bundle.putInt(READ_DATA_CNT, len);
                        bundle.putByteArray(READ_BUFFER_ARRAY, buffer);
                        message.setData(bundle);
                        mHandler.sendMessage(message);
                        try {
                            Thread.sleep(200);
                        } catch (InterruptedException e) {

                        }
                    }
                }
            } catch (Exception e) {
                if (deviceConnFactoryManagers[id] != null) {
                    closePort(id);
                }
            }
        }

        public void cancel() {
            isRun = false;
        }
    }

    private Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case READ_DATA:
                    int cnt = msg.getData().getInt(READ_DATA_CNT);
                    byte[] buffer = msg.getData().getByteArray(READ_BUFFER_ARRAY);

                    if (buffer == null) {
                        return;
                    }
                    int result = judgeResponseType(buffer[0]);
                    //Utils.toast(App.getContext(), String.valueOf(result));
                    String status = App.getContext().getString(R.string.str_printer_conn_normal);
 // ESC
                    if (result == 0) {
                        Intent intent = new Intent(ACTION_QUERY_PRINTER_STATE);
                        intent.putExtra(DEVICE_ID, id);
                        App.getContext().sendBroadcast(intent);
                    } else if (result == 1) {
                        result = judgeResponseType_ASB(buffer[0]);
                        if (result == 1){
                            if ((buffer[0] & ESC_STATE_PAPER_ERR) > 0) {
                                status += " " + App.getContext().getString(R.string.str_printer_out_of_paper);

                                sendStateBroadcast(ESC_PRINTER_STATE_PAPER_ERR);
                            }
                            if ((buffer[0] & ESC_STATE_COVER_OPEN) > 0) {
                                status += " " + App.getContext().getString(R.string.str_printer_open_cover);

                                sendStateBroadcast(ESC_PRINTER_STATE_ERR_OCCURS);
                            }
                            if ((buffer[0] & ESC_STATE_ERR_OCCURS) > 0) {
                                status += " " + App.getContext().getString(R.string.str_printer_error);

                                sendStateBroadcast(ESC_PRINTER_STATE_ERR_OCCURS);
                            }
                            if ((buffer[0] & ESC_STATE_NORMAL) == 0) {

                                sendStateBroadcast(CONN_STATE_CONNECTED);
                            }
                            System.out.println(App.getContext().getString(R.string.str_state) + status);
                            Utils.toast(App.getContext(), "status:" + status);
                        }else if (result == 0){//ASB
                            if ((buffer[1] & ESC_ASB_STATE_NORMAL) == 0 && (buffer[2] & ESC_ASB_STATE_NORMAL) == 0) {
                                sendStateBroadcast(CONN_STATE_CONNECTED);
                            }
                            if ((buffer[1] & ESC_ASB_1_STATE_UNRECOVERABLE_ERR) > 0 || (buffer[1] & ESC_ASB_1_STATE_RECOVERABLE_ERR) > 0) {
                                sendStateBroadcast(ESC_PRINTER_STATE_ERR_OCCURS);
                            }
                            if ((buffer[2] & ESC_ASB_2_STATE_PAPER_ERR) > 0 ) {
                                sendStateBroadcast(ESC_PRINTER_STATE_PAPER_ERR);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    };

    private void sendStateBroadcast(int state) {
        Intent intent = new Intent(ACTION_CONN_STATE);
        intent.putExtra(STATE, state);
        intent.putExtra(DEVICE_ID, id);
        App.getContext().sendBroadcast(intent);
    }


    private int judgeResponseType(byte r) {
        return (byte) ((r & FLAG) >> 4);
    }

    private int judgeResponseType_ASB(byte r) {
        return (byte) ((r & FLAG_ASB) >> 1);
    }

    BroadcastReceiver usbStateReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            switch (action) {
                case ACTION_USB_DEVICE_DETACHED:
                    sendStateBroadcast(CONN_STATE_DISCONNECT);
                    break;
                default:
                    break;
            }
        }
    };

}