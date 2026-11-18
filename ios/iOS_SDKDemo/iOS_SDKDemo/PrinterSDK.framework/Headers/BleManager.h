//
//  BleManager.h
//  CodekSDK
//
//  Created by yhb on 2021/3/17.
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import "BleCallback.h"
#import "BleOptions.h"

NS_ASSUME_NONNULL_BEGIN

#define NotificationAtCentralManagerDidUpdateState @"NotificationAtCentralManagerDidUpdateState"
#define NotificationAtDidConnectPeripheral @"NotificationAtDidConnectPeripheral"
#define NotificationAtDidDisconnectPeripheral @"NotificationAtDidDisconnectPeripheral"
#define NotificationAtDidWriteValueForCharacteristic @"NotificationAtDidWriteValueForCharacteristic"
#define NotificationAtDidUpdateValueForCharacteristic @"NotificationAtDidUpdateValueForCharacteristic"
#define NotificationAtPeripheralStateChange @"NotificationAtPeripheralStateChange"

@interface BleManager : NSObject<CBCentralManagerDelegate,CBPeripheralDelegate>
 // / block
/// Block of device state changes
@property (nonatomic, copy) CentralManagerDidUpdateStateBlock blockOnCentralManagerDidUpdateState;
 // / block
/// Block that disconnects the device
@property (nonatomic, copy) DisconnectBlock blockOnDisconnect;
 // / block
/// This block is going to be called when all the data comes back
@property (nonatomic, copy) ReadDataBlock readTotalCallback;

/// Discover the device name rule
@property (nonatomic, copy) FilterOnDiscoverPeripherals filterOnDiscoverPeripherals;


/// Sets Bluetooth connection properties
@property (nonatomic, strong) BleOptions *bleOptions;

/// Whether it's already connected
@property (nonatomic, assign) BOOL isConnected;

/// Whether you are scanning
@property (nonatomic, assign) BOOL isScanning;

/// Whether it is being sent
@property (nonatomic, assign) BOOL isSending;

/**
 *  开启扫描设备 Turn on scanning device
 *  @param blockOnDiscoverPeripherals 发现设备的回调 Discover the device's callback
 */
- (void)scanPeripheralsWithBlock:(nullable DiscoverPeripheralsBlock)blockOnDiscoverPeripherals;

/**
 *  取消扫描 Cancel the scan
 */
- (void)cancelScan;

/**
 *  连接设备 Connected device
 *  @param peripheral 需要连接的设备 Devices that need to be connected
 *  @param blockOnConnectedPeripheral 连接成功的回调 A callback for successful connection
 *  @param blockOnFailToConnect       连接失败的回调 Callback for connection failure
 */
- (void)connectToPeripheral:(nullable CBPeripheral *)peripheral
              withConnected:(nullable ConnectedPeripheralBlock)blockOnConnectedPeripheral
                   withFail:(nullable FailToConnectBlock)blockOnFailToConnect
                  withState:(nullable ConnectPeripheralStateBlock)blockOnConnectPeripheralState;

/**
 *  断开已连接的设备 Disconnect the connected device
 *  @param  peripheral 传入已连接的设备 Passing in the connected device
 */
- (void)cancelPeripheralConnection:(nullable CBPeripheral *)peripheral;

/**
 *  获取当前连接的设备 Gets the currently connected device
 *  @return CBPeripheral 已连接的设备 Connected device
 */
- (CBPeripheral *)getConnectedDevice;

/**
 *  往连接设备中写入数据 Writes data to the connected device
 *  @param  data 写入的数据 Written data
 *  @param  progress 写入数据的进度 The progress of writing data
 *  @param  callBack 设备返回的数据 Data returned by the device
 */
- (void)write:(NSData *_Nullable)data
     progress:(nullable WriteProgressBlock)progress
 receCallBack:(nullable ReadDataBlock)callBack;

- (void)write:(NSData *_Nullable)data;

/**
 *  取消写入数据
 *  Unwrite data
 */
- (void)cancelSendData;

/**
 *  获取注设备状态
 *  Gets the status of injection device
 *  @return 设备状态 Equipment state
 */
- (CBManagerState)getCentralManagerState;

@end

NS_ASSUME_NONNULL_END
