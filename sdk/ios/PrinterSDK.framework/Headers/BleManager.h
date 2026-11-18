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
/// block
/// Block of device state changes
@property (nonatomic, copy) CentralManagerDidUpdateStateBlock blockOnCentralManagerDidUpdateState;
/// block
/// Block that disconnects the device
@property (nonatomic, copy) DisconnectBlock blockOnDisconnect;
/// block
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
 *   Turn on scanning device
 *  @param blockOnDiscoverPeripherals  Discover the device's callback
 */
- (void)scanPeripheralsWithBlock:(nullable DiscoverPeripheralsBlock)blockOnDiscoverPeripherals;

/**
 *   Cancel the scan
 */
- (void)cancelScan;

/**
 *   Connected device
 *  @param peripheral  Devices that need to be connected
 *  @param blockOnConnectedPeripheral  A callback for successful connection
 *  @param blockOnFailToConnect        Callback for connection failure
 */
- (void)connectToPeripheral:(nullable CBPeripheral *)peripheral
              withConnected:(nullable ConnectedPeripheralBlock)blockOnConnectedPeripheral
                   withFail:(nullable FailToConnectBlock)blockOnFailToConnect
                  withState:(nullable ConnectPeripheralStateBlock)blockOnConnectPeripheralState;

/**
 *   Disconnect the connected device
 *  @param  peripheral  Passing in the connected device
 */
- (void)cancelPeripheralConnection:(nullable CBPeripheral *)peripheral;

/**
 *   Gets the currently connected device
 *  @return CBPeripheral  Connected device
 */
- (CBPeripheral *)getConnectedDevice;

/**
 *   Writes data to the connected device
 *  @param  data  Written data
 *  @param  progress  The progress of writing data
 *  @param  callBack  Data returned by the device
 */
- (void)write:(NSData *_Nullable)data
     progress:(nullable WriteProgressBlock)progress
 receCallBack:(nullable ReadDataBlock)callBack;

- (void)write:(NSData *_Nullable)data;

/**
 *  Unwrite data
 */
- (void)cancelSendData;

/**
 *  Gets the status of injection device
 *  @return  Equipment state
 */
- (CBManagerState)getCentralManagerState;

@end

NS_ASSUME_NONNULL_END
