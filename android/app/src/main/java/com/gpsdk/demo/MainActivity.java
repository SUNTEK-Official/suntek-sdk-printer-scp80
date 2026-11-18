package com.gpsdk.demo;

import android.Manifest;
import android.app.AlertDialog;
import android.app.PendingIntent;
import android.bluetooth.BluetoothDevice;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import com.printer.sdk.command.EscCommand;
import com.printer.sdk.command.LabelCommand;
import java.util.ArrayList;
import java.util.Vector;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import static android.hardware.usb.UsbManager.ACTION_USB_DEVICE_DETACHED;
import static com.gpsdk.demo.Constant.ACTION_USB_PERMISSION;
import static com.gpsdk.demo.Constant.MESSAGE_UPDATE_PARAMETER;
import static com.gpsdk.demo.Constant.USB_REQUEST_CODE;
import static com.gpsdk.demo.DeviceConnFactoryManager.ACTION_QUERY_PRINTER_STATE;
import static com.gpsdk.demo.DeviceConnFactoryManager.CONN_STATE_FAILED;


public class MainActivity extends AppCompatActivity {
    private static final String TAG = "MainActivity";
    ArrayList<String> per = new ArrayList<>();
    private UsbManager usbManager;
    private int counts;
    private static final int REQUEST_CODE = 0x004;

    private static final int CONN_STATE_DISCONN = 0x007;

    private static final int PRINTER_COMMAND_ERROR = 0x008;


    private static  int USBCONNECT_ENABLE = 1;

    private byte[] esc = {0x10, 0x04, 0x02};//1004



    private byte[] tsc = {0x1b, '!', '?'};

    private static final int CONN_MOST_DEVICES = 0x11;
    private static final int CONN_PRINTER = 0x12;
    private PendingIntent mPermissionIntent;
    private String[] permissions = {
            Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_COARSE_LOCATION,
            Manifest.permission.BLUETOOTH
    };
    private String usbName;
    private TextView tvConnState;
    private ThreadPool threadPool;
    private int selectedDensity;
    static private int id = 0;
    private EditText etPrintCounts;
    private Spinner spDensity;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.e(TAG,"onCreate()");
        setContentView(R.layout.activity_main);
        usbManager = (UsbManager) getSystemService(Context.USB_SERVICE);
        checkPermission();
        requestPermission();

        tvConnState = (TextView) findViewById(R.id.tv_connState);
        etPrintCounts = (EditText) findViewById(R.id.et_print_counts);

        spDensity = (Spinner) findViewById(R.id.spdensity);
        ArrayAdapter portAdapter2 = new ArrayAdapter(this, R.layout.sp_tv_item, this.getResources().getStringArray(R.array.str_density_type));
        spDensity.setAdapter(portAdapter2);
        spDensity.setSelection(2);
        initListener();
    }

    @Override
    protected void onStart() {
        super.onStart();
        IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
        filter.addAction(ACTION_USB_DEVICE_DETACHED);
        filter.addAction(ACTION_QUERY_PRINTER_STATE);
        filter.addAction(DeviceConnFactoryManager.ACTION_CONN_STATE);
        registerReceiver(receiver, filter);
    }

    private void checkPermission() {
        for (String permission : permissions) {
            if (PackageManager.PERMISSION_GRANTED != ContextCompat.checkSelfPermission(this, permission)) {
                per.add(permission);
            }
        }
    }

    private void requestPermission() {
        if (per.size() > 0) {
            String[] p = new String[per.size()];
            ActivityCompat.requestPermissions(this, per.toArray(p), REQUEST_CODE);
        }
    }


    public void btnBluetoothConn(View view) {
        startActivityForResult(new Intent(this, BluetoothDeviceList.class), Constant.BLUETOOTH_REQUEST_CODE);
    }


    public void btnMoreDevices(View view) {
        startActivityForResult(new Intent(this, ConnMoreDevicesActivity.class), CONN_MOST_DEVICES);
    }


    public void btnSerialPortConn(View view) {
        startActivityForResult(new Intent(this, SerialPortList.class), Constant.SERIALPORT_REQUEST_CODE);
    }


    public void btnUsbConn(View view) {
        if(USBCONNECT_ENABLE == 1) {
            usbManager = (UsbManager) getSystemService(Context.USB_SERVICE);
            startActivityForResult(new Intent(this, UsbDeviceList.class), Constant.USB_REQUEST_CODE);
            USBCONNECT_ENABLE = 0;
        }
        else
        {
            mHandler.obtainMessage(CONN_STATE_DISCONN).sendToTarget();
            USBCONNECT_ENABLE = 1;
        }
    }


    public void btnWifiConn(View view) {
        WifiParameterConfigDialog wifiParameterConfigDialog = new WifiParameterConfigDialog(this, mHandler);
        wifiParameterConfigDialog.show();
    }
    public void btnSerialTest(View view) {
        Intent startIntent = new Intent(getApplicationContext(),
                SerialTest.class);
        startIntent.putExtra(SerialTest.ID, id);
        startActivity(new Intent(startIntent));
    }
    public void btnReceiptPrint(View view) {
       // sendReceiptWithResponse();

        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(new Runnable() {
            @Override
            public void run() {
                //if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getCurrentPrinterCommand() == PrinterCommand.ESC) {
                //DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].turnOnASB();
                sendReceiptWithResponse();
                //} else {
                //    mHandler.obtainMessage(PRINTER_COMMAND_ERROR).sendToTarget();
                //}
            }
        });
    }

    public void btnLabelPrint(View view) {
        threadPool = ThreadPool.getInstantiation();
        threadPool.addTask(new Runnable() {
            @Override
            public void run() {
                if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                        !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
                    mHandler.obtainMessage(CONN_PRINTER).sendToTarget();
                    return;
                }
                if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getCurrentPrinterCommand() == PrinterCommand.TSC) {
                    //sendLabel();
                } else {
                    mHandler.obtainMessage(PRINTER_COMMAND_ERROR).sendToTarget();
                }
            }
        });
    }


    public void btnDisConn(View view) {
        mHandler.obtainMessage(CONN_STATE_DISCONN).sendToTarget();
        USBCONNECT_ENABLE = 1;
    }
    public void btnCash(View view) {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        EscCommand esc = new EscCommand();
        esc.addGeneratePlus(LabelCommand.FOOT.F5, (byte) 255, (byte) 255);
        esc.addGeneratePlus(LabelCommand.FOOT.F2, (byte) 255, (byte) 255);
        Vector<Byte> datas = esc.getCommand();

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);
    }

    static int cutmodeflag = 0;
    public void btnCutMode(View view) {

        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        byte n = 0;
        String cutmode = getString(R.string.str_cutmode_partial);
        if (this.cutmodeflag == 0){
            n = (byte) 0xaa; // halfcut;
            cutmode = getString(R.string.str_cutmode_partial);
            this.cutmodeflag = 1;
        }
        else
        {
            n = (byte) 0x88; // fullcut;
            cutmode = getString(R.string.str_cutmode_full);
            this.cutmodeflag = 0;
        }
        byte[] command= new byte[]{0x1f, 0x1b, 0x1f, 0x40, 0x04, 0x06, 0x08, n,0x1D,0x56 ,0x42 ,0x00 ,0x0A ,0x0A ,0x0A ,0x0A ,0x0A ,0x0A};
        Vector<Byte> datas = new Vector<Byte> (18);
        for(int i =0;i<18;i++){
            datas.add(command[i]);
        }

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);
        Utils.toast(this, cutmode);
    }

    public void btnCuttest(View view) {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        byte[] command= new byte[]{0x1D,0x56 ,0x42 ,0x00 ,0x0A ,0x0A ,0x0A ,0x0A ,0x0A ,0x0A};
        Vector<Byte> datas = new Vector<Byte> (10);
        for(int i =0;i<10;i++){
            datas.add(command[i]);
        }

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);
    }

    public void btnFlashLogodownload(View view) {
        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        Bitmap b = BitmapFactory.decodeResource(getResources(),R.mipmap.logo1);
        Bitmap[] bm = new Bitmap[1];
        bm[0] = b;
        EscCommand esc = new EscCommand();
        esc.addDownloadNvBitImage(bm);
        esc.addPrintNvBitmap((byte)1, (byte)0); // Flash Logo
        esc.addPrintAndFeedLines((byte) 6);
        Vector<Byte> datas = esc.getCommand();

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);
        Utils.toast(this, getString(R.string.str_logodownload_tips));
    }
    public void btnDensity(View view) {

        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
    }
        byte n = 3;
            switch (selectedDensity) {
                case 0: n = 5;break;//max
                case 1: n = 4;break;
                case 3: n = 2;break;
                case 4: n = 1;break;//min
                default: n = 3;break;//middle
            }
        byte[] command= new byte[]{0x1F,0x1B ,0x1F ,0x41 ,0x04 ,0x06 ,0x07,n};
        Vector<Byte> datas = new Vector<Byte> (8);
        for(int i =0;i<8;i++){
            datas.add(command[i]);
        }

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);

    }
    private void initListener() {
        spDensity.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                selectedDensity = position;
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }

    public void btnPrinterState(View view) {

        if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] == null ||
                !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
            Utils.toast(this, getString(R.string.str_cann_printer));
            return;
        }
        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].queryPrinterState(); // 100402
    }


    public void btnReceiptAndLabelContinuityPrint(View view) {
        counts = Integer.parseInt(etPrintCounts.getText().toString().trim());
        sendContinuityPrint();
    }

    private void sendContinuityPrint() {
        ThreadPool.getInstantiation().addTask(new Runnable() {
            @Override
            public void run() {
                if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] != null
                        && DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
                    ThreadFactoryBuilder threadFactoryBuilder = new ThreadFactoryBuilder("MainActivity_sendContinuity_Timer");
                    ScheduledExecutorService scheduledExecutorService = new ScheduledThreadPoolExecutor(1, threadFactoryBuilder);
                    scheduledExecutorService.schedule(threadFactoryBuilder.newThread(new Runnable() {
                        @Override
                        public void run() {
                            counts--;
                            if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getCurrentPrinterCommand() == PrinterCommand.ESC) {
                                sendReceiptWithResponse();
                            } else {
 // LabelCommand.addPrint()
                                //sendLabel();
                            }
                        }
                    }), 1000, TimeUnit.MILLISECONDS);
                }
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK) {
            if(DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] != null &&
                    DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()){
                Utils.toast(MainActivity.this, "端口已经连接");
                return;
            }
            switch (requestCode) {


                case Constant.BLUETOOTH_REQUEST_CODE: {

                    String macAddress = data.getStringExtra(BluetoothDeviceList.EXTRA_DEVICE_ADDRESS);

                    new DeviceConnFactoryManager.Build()
                            .setId(id)
                            .setConnMethod(DeviceConnFactoryManager.CONN_METHOD.BLUETOOTH)
                            .setMacAddress(macAddress)
                            .build();
                    DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].openPort();
                    break;
                }
                case Constant.USB_REQUEST_CODE: {
                    usbName = data.getStringExtra(UsbDeviceList.USB_NAME);
                    UsbDevice usbDevice = Utils.getUsbDeviceFromName(MainActivity.this, usbName); // 1004

                    if (usbManager.hasPermission(usbDevice)) {
                       usbConn(usbDevice);
                    } else {
                        mPermissionIntent = PendingIntent.getBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
                        usbManager.requestPermission(usbDevice, mPermissionIntent);
                    }
                    break;
                }

                case Constant.SERIALPORT_REQUEST_CODE:

                    int baudrate = data.getIntExtra(Constant.SERIALPORTBAUDRATE, 0);

                    String path = data.getStringExtra(Constant.SERIALPORTPATH);

                    if (baudrate != 0 && !TextUtils.isEmpty(path)) {
                      new DeviceConnFactoryManager.Build()
                                .setConnMethod(DeviceConnFactoryManager.CONN_METHOD.SERIAL_PORT)
                                .setId(id)
                                .setBaudrate(baudrate)
                                .setSerialPort(path)
                                .build();

                        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].openPort();
                    }
                    break;
                case CONN_MOST_DEVICES:
                    id = data.getIntExtra("id", -1);
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] != null &&
                            DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].getConnState()) {
                        tvConnState.setText(getString(R.string.str_conn_state_connected) + "\n" + getConnDeviceInfo());
                    } else {
                        tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void usbConn(UsbDevice usbDevice) {
        new DeviceConnFactoryManager.Build()
                .setId(id)
                .setConnMethod(DeviceConnFactoryManager.CONN_METHOD.USB)
                .setUsbDevice(usbDevice)
                .setContext(this)
                .build();
        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].openPort(); // jar checkUsbDevicePidVid
    }




    /**
     * 发送票据
     */
    void sendReceiptWithResponse() {

        EscCommand esc = new EscCommand();
        //esc.RealtimeStatusTransmission(EscCommand.STATUS.PRINTER_OFFLINE);
        //esc.addInitializePrinter();
        esc.addPrintAndFeedPaper((byte)255);
        esc.addPrintAndFeedLines((byte) 3);

        esc.addSelectJustification(EscCommand.JUSTIFICATION.CENTER);

        esc.addSelectPrintModes(EscCommand.FONT.FONTA, EscCommand.ENABLE.OFF, EscCommand.ENABLE.ON, EscCommand.ENABLE.ON, EscCommand.ENABLE.OFF);

        esc.addText("Sample\n");
        esc.addPrintAndLineFeed();



        esc.addSelectPrintModes(EscCommand.FONT.FONTA, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF);

        esc.addSelectJustification(EscCommand.JUSTIFICATION.LEFT);

        esc.addText("Print text\n");

        esc.addText("Welcome to use our printer!\n");


        String message = "热敏票据打印机\n";
        esc.addText(message, "GB2312");
        esc.addPrintAndLineFeed();



        esc.addText("abc");
        esc.addSetHorAndVerMotionUnits((byte) 7, (byte) 0);
        esc.addSetAbsolutePrintPosition((short) 6);
        esc.addText("DEF");
        esc.addSetAbsolutePrintPosition((short) 10);
        esc.addText("Printer");
        esc.addPrintAndLineFeed();



        esc.addText("RAM Logo:\n");

        Bitmap b = BitmapFactory.decodeResource(getResources(),R.mipmap.logo2);
        esc.addRastBitImage(b,b.getWidth(),0); // RAM Logo

        esc.addText("Print code128\n");
        esc.addSelectPrintingPositionForHRICharacters(EscCommand.HRI_POSITION.BELOW);
        esc.addSetBarcodeHeight((byte) 80);
        esc.addSetBarcodeWidth((byte) 2);
        esc.addCODE128(esc.genCodeC("91810121")+esc.genCodeB("318510033"));

        esc.addPrintAndLineFeed();

        esc.addText("Print QRcode\n");

        esc.addSelectErrorCorrectionLevelForQRCode((byte) 0x31);
        esc.addSelectSizeOfModuleForQRCode((byte) 15);
        esc.addStoreQRCodeData("Welcome to our printer!");
        esc.addPrintQRCode();//
        esc.addPrintAndLineFeed();
        //esc.addSelectJustification(EscCommand.JUSTIFICATION.CENTER);
        esc.addText("Flash Logo:\n");
        esc.addPrintNvBitmap((byte)1, (byte)0); // Flash Logo

        esc.addGeneratePlus(LabelCommand.FOOT.F5, (byte) 255, (byte) 255);
        esc.addGeneratePlus(LabelCommand.FOOT.F2, (byte) 255, (byte) 255);
        //esc.addPrintAndFeedLines((byte) 8);
        esc.addText("Completed!\r\n");
        //esc.addQueryPrinterStatus();
        esc.addPrintAndFeedLines((byte) 9);
        //esc.addInitializePrinter();
        esc.addCutAndFeedPaper((byte)0x0);
        Vector<Byte> datas = esc.getCommand();

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].sendDataImmediately(datas);

    }

    private BroadcastReceiver receiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            switch (action) {
                case ACTION_USB_PERMISSION:
                    synchronized (this) {
                        UsbDevice device = intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);
                        if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false)) {
                            if (device != null) {
                                System.out.println("permission ok for device " + device);
                                usbConn(device);
                            }
                        } else {
                            System.out.println("permission denied for device " + device);
                        }
                    }
                    break;

                case ACTION_USB_DEVICE_DETACHED:
                case BluetoothDevice.ACTION_ACL_DISCONNECTED:
                    mHandler.obtainMessage(CONN_STATE_DISCONN).sendToTarget();
                    break;
                case DeviceConnFactoryManager.ACTION_CONN_STATE:
                    int state = intent.getIntExtra(DeviceConnFactoryManager.STATE, -1);
                    int deviceId = intent.getIntExtra(DeviceConnFactoryManager.DEVICE_ID, -1);
                    switch (state) {
                        case DeviceConnFactoryManager.CONN_STATE_DISCONNECT:
                            if (id == deviceId) {
                                tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            }
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTING:
                            tvConnState.setText(getString(R.string.str_conn_state_connecting));
                            break;
                        case DeviceConnFactoryManager.CONN_STATE_CONNECTED:
                            tvConnState.setText(getString(R.string.str_conn_state_connected) + "\n" + getConnDeviceInfo());
                            break;
                        case CONN_STATE_FAILED:
                            Utils.toast(MainActivity.this, getString(R.string.str_conn_fail));
                            tvConnState.setText(getString(R.string.str_conn_state_disconnect));
                            break;
                        case DeviceConnFactoryManager.ESC_PRINTER_STATE_PAPER_ERR:

                            tvConnState.setText("State：Missing paper");
                            break;
                        case DeviceConnFactoryManager.ESC_PRINTER_STATE_ERR_OCCURS:

                            tvConnState.setText("State：Error,please check the printer.");
                            break;
                        default:
                            break;
                    }
                    break;
                case ACTION_QUERY_PRINTER_STATE:
                    if(counts >0) {
                        sendContinuityPrint();
                    }
                    break;
                default:
                    break;
            }
        }
    };

    private Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case CONN_STATE_DISCONN:
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id] != null) {
                        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].closePort(id);
                    }
                    break;
                case PRINTER_COMMAND_ERROR:
                    Utils.toast(MainActivity.this, getString(R.string.str_choice_printer_command));
                    break;
                case CONN_PRINTER:
                    Utils.toast(MainActivity.this, getString(R.string.str_cann_printer));
                    break;
                case MESSAGE_UPDATE_PARAMETER:
                    String strIp = msg.getData().getString("Ip");
                    String strPort = msg.getData().getString("Port");

                    new DeviceConnFactoryManager.Build()

                            .setConnMethod(DeviceConnFactoryManager.CONN_METHOD.WIFI)

                            .setIp(strIp)

                            .setId(id)

                            .setPort(Integer.parseInt(strPort))
                            .build();
                    threadPool = ThreadPool.getInstantiation();
                    threadPool.addTask(new Runnable() {
                        @Override
                        public void run() {
                            DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id].openPort();
                        }
                    });
                    break;
                default:
                    break;
            }
        }
    };

    @Override
    protected void onStop() {
        super.onStop();
        unregisterReceiver(receiver);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.e(TAG,"onDestroy()");
        DeviceConnFactoryManager.closeAllPort();
        if (threadPool != null) {
            threadPool.stopThreadPool();
        }
    }
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        String tag = intent.getStringExtra("EXIT_TAG");
        if (tag != null&& !TextUtils.isEmpty(tag)) {
            if ("SINGLETASK".equals(tag)) {
                finish();
            }
        }
    }

    private String getConnDeviceInfo() {
        String str = "";
        DeviceConnFactoryManager deviceConnFactoryManager = DeviceConnFactoryManager.getDeviceConnFactoryManagers()[id];
        if (deviceConnFactoryManager != null
                && deviceConnFactoryManager.getConnState()) {
            if ("USB".equals(deviceConnFactoryManager.getConnMethod().toString())) {
                str += "USB\n";
                str += "USB Name: " + deviceConnFactoryManager.usbDevice().getDeviceName();
            } else if ("WIFI".equals(deviceConnFactoryManager.getConnMethod().toString())) {
                str += "WIFI\n";
                str += "IP: " + deviceConnFactoryManager.getIp() + "\t";
                str += "Port: " + deviceConnFactoryManager.getPort();
            } else if ("BLUETOOTH".equals(deviceConnFactoryManager.getConnMethod().toString())) {
                str += "BLUETOOTH\n";
                str += "MacAddress: " + deviceConnFactoryManager.getMacAddress();
            } else if ("SERIAL_PORT".equals(deviceConnFactoryManager.getConnMethod().toString())) {
                str += "SERIAL_PORT\n";
                str += "Path: " + deviceConnFactoryManager.getSerialPortPath() + "\t";
                str += "Baudrate: " + deviceConnFactoryManager.getBaudrate();
            }
        }
        return str;
    }


}