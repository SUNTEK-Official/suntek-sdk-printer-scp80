package com.gpsdk.demo;


import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.EditText;

import com.printer.sdk.command.EscCommand;

import java.util.Vector;



public class SerialTest  extends AppCompatActivity {

 // PortHandler message
    private static final int PRINT_TEST = 1000;
    private static final int CONN_STATE_DISCONN = 1010;
    private EditText etBr = null;
    private EditText etPort = null;

    public static final String ID = "id";
    private int deviceId = -1;


    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_serial_test);
        InitView();

        deviceId =  getIntent().getIntExtra(ID,-1);
        Utils.toast(SerialTest.this,"Serial Port ID:"+Integer.toString(deviceId));
    }
    private void InitView(){
        getSupportActionBar().hide();
        etBr = (EditText) findViewById(R.id.etBr);
        etPort = (EditText) findViewById(R.id.etPort);
    }
    public void btnOpenSerial(View view) {
        try{
            if(DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId] != null &&
                    DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId].getConnState()){
                Utils.toast(SerialTest.this, "Serial port connected.");
                return;
            }
            int baud =  Integer.parseInt(etBr.getText().toString());
            String address = etPort.getText().toString();
                new DeviceConnFactoryManager.Build()

                        .setConnMethod(DeviceConnFactoryManager.CONN_METHOD.SERIAL_PORT)
                        .setId(deviceId)

                        .setBaudrate(baud)

                        .setSerialPort(address)
                        .build();

            DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId].openPort();
        }
        catch(Exception e){
            Utils.toast(SerialTest.this,e.getMessage().toString());
        }
    }
    public void btnTest(View view) {
        Message message = Message.obtain();
        message.what = PRINT_TEST;
        PortHandler.sendMessage(message);
    }

    private Handler PortHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case PRINT_TEST:
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId] == null ||
                            !DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId].getConnState()) {
                        Utils.toast(SerialTest.this, getString(R.string.str_cann_printer));
                        return;
                    }
                    printTest();
                    break;
                case CONN_STATE_DISCONN:
                    if (DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId] != null) {
                        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId].closePort(deviceId);
                        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId] = null;
                    }
                    break;
                default:
                    break;
            }
        }

    };
    public int printTest(){
        EscCommand esc = new EscCommand();
        esc.addInitializePrinter();
        esc.addPrintAndFeedLines((byte) 3);

        esc.addSelectPrintModes(EscCommand.FONT.FONTA, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF, EscCommand.ENABLE.OFF);

        esc.addSelectJustification(EscCommand.JUSTIFICATION.LEFT);

        esc.addText("data："+"\n"+" "+"\n");

        esc.addText("================================\n");
        esc.addPrintAndLineFeed();
        int j =0;
        for(int i= 0;i<10;i++) {
            esc.addText("This is a serial test." + "\n");
            j++;
        }
        esc.addPrintAndLineFeed();
        Vector<Byte> datas = esc.getCommand();

        DeviceConnFactoryManager.getDeviceConnFactoryManagers()[deviceId].sendDataImmediately(datas);
        return  j;
    }


}
