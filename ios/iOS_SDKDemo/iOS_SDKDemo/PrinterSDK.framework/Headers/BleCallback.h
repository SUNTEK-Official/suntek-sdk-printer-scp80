//
//  BleCallback.h
//  CodekSDK
//
//  Created by yhb on 2021/3/19.
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

NS_ASSUME_NONNULL_BEGIN

/**
 *  @enum ConnectState
 *  
 *  @discussion 连接状态(Connection status)
 */
typedef NS_ENUM (NSUInteger,ConnectState) {
    NOT_FOUND_DEVICE, // /< (Device not found)
    CONNECT_STATE_DISCONNECT, // /< (Disconnect)
    CONNECT_STATE_CONNECTING, // /< (Connecting)
    CONNECT_STATE_CONNECTED, // /< (Connected)
    CONNECT_STATE_TIMEOUT, // /< (Connection timeout)
    CONNECT_STATE_FAILT // /< (Connection fail)
};

@interface BleCallback : NSObject
 // / block
/// Callbacks for device state changes
typedef void (^CentralManagerDidUpdateStateBlock)(CBCentralManager *central);
 // / block
/// Find the device's callback
typedef void (^DiscoverPeripheralsBlock)(CBCentralManager *central, CBPeripheral *peripheral, NSDictionary *advertisementData, NSNumber *RSSI);
 // / block
/// A callback for successful connection of the device
typedef void (^ConnectedPeripheralBlock)(CBCentralManager *central, CBPeripheral *peripheral);
 // / block
/// A callback that failed to connect the device
typedef void (^FailToConnectBlock)(CBCentralManager *central, CBPeripheral *peripheral, NSError *error);
 // / block
/// A callback to the process of connecting the device
typedef void (^ConnectPeripheralStateBlock)(ConnectState state);
 // / block
/// A callback that disconnects the device
typedef void (^DisconnectBlock)(CBCentralManager *central, CBPeripheral *peripheral, NSError *error);
 // / block
/// A callback to the progress of writing data
typedef void (^ReadDataBlock)(NSData * _Nullable data);
 // / block
/// A callback to read device data
typedef void (^WriteProgressBlock)(NSUInteger total, NSUInteger progress);
#pragma mark - 过滤器Filter
 // / peripherals
/// Discover the Peripherals rule
typedef BOOL (^FilterOnDiscoverPeripherals)(NSString *peripheralName, NSDictionary *advertisementData, NSNumber *RSSI);

@end

NS_ASSUME_NONNULL_END
