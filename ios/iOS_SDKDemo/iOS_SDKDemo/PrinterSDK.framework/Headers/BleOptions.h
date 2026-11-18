//
//  BleOptions.h
//  CodekSDK
//
//  Created by yhb on 2021/3/19.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface BleOptions : NSObject
#pragma mark - 属性
/*!
 *  扫描参数 The scanning parameters
 *  @param              An optional dictionary specifying options for the scan.
 *  @see                centralManager:scanForPeripheralsWithServices
 *  @see                CBCentralManagerScanOptionAllowDuplicatesKey :忽略同一个Peripheral端的多个发现事件被聚合成一个发现事件
 *  @see                CBCentralManagerScanOptionSolicitedServiceUUIDsKey
 */
@property (nonatomic, copy) NSDictionary *scanForPeripheralsWithOptions;

/*!
*   连接设备的参数 Parameters of the connected device
*   @param              An optional dictionary specifying connection behavior options.
*   @see                centralManager:didConnectPeripheral:
*   @see                centralManager:didFailToConnectPeripheral:error:
*   @seealso            CBConnectPeripheralOptionNotifyOnConnectionKey
*   @seealso            CBConnectPeripheralOptionNotifyOnDisconnectionKey
*   @seealso            CBConnectPeripheralOptionNotifyOnNotificationKey
*/
@property (nonatomic, copy) NSDictionary *connectPeripheralWithOptions;
/*!
 *  扫描参数 The scanning parameters
 *  @param serviceUUIDs A list of <code>CBUUID</code> objects representing the service(s) to scan for.
 *  @see                centralManager:scanForPeripheralsWithServices
 */
@property (nonatomic, copy) NSArray *scanForPeripheralsWithServices;

// [peripheral discoverServices:self.discoverWithServices];
@property (nonatomic, copy) NSArray *discoverWithServices;

// [peripheral discoverCharacteristics:self.discoverWithCharacteristics forService:service];
@property (nonatomic, copy) NSArray *discoverWithCharacteristics;
/*!
 * 连接设备超时时间，默认5s
 * The timeout time of connected device is 5s by default
 */
@property (nonatomic, assign) float connectTimeout;
/*!
 * 一个数据包大小，默认150
 * One packet size, default 150
 */
@property (nonatomic, assign) int dataPageSize;


#pragma mark - 构造方法
- (instancetype)initWithscanForPeripheralsWithOptions:(NSDictionary *)scanForPeripheralsWithOptions
                         connectPeripheralWithOptions:(NSDictionary *)connectPeripheralWithOptions;

- (instancetype)initWithscanForPeripheralsWithOptions:(NSDictionary *)scanForPeripheralsWithOptions
                         connectPeripheralWithOptions:(NSDictionary *)connectPeripheralWithOptions
                       scanForPeripheralsWithServices:(NSArray *)scanForPeripheralsWithServices
                                 discoverWithServices:(NSArray *)discoverWithServices
                          discoverWithCharacteristics:(NSArray *)discoverWithCharacteristics;


@end

NS_ASSUME_NONNULL_END
