//
//  BleOptions.h
//  CodekSDK
//
//  Created by yhb on 2021/3/19.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface BleOptions : NSObject
/*!
 *   The scanning parameters
 *  @param              An optional dictionary specifying options for the scan.
 *  @see                centralManager:scanForPeripheralsWithServices
 *  @see                CBCentralManagerScanOptionAllowDuplicatesKey :Peripheral
 *  @see                CBCentralManagerScanOptionSolicitedServiceUUIDsKey
 */
@property (nonatomic, copy) NSDictionary *scanForPeripheralsWithOptions;

/*!
*    Parameters of the connected device
*   @param              An optional dictionary specifying connection behavior options.
*   @see                centralManager:didConnectPeripheral:
*   @see                centralManager:didFailToConnectPeripheral:error:
*   @seealso            CBConnectPeripheralOptionNotifyOnConnectionKey
*   @seealso            CBConnectPeripheralOptionNotifyOnDisconnectionKey
*   @seealso            CBConnectPeripheralOptionNotifyOnNotificationKey
*/
@property (nonatomic, copy) NSDictionary *connectPeripheralWithOptions;
/*!
 *   The scanning parameters
 *  @param serviceUUIDs A list of <code>CBUUID</code> objects representing the service(s) to scan for.
 *  @see                centralManager:scanForPeripheralsWithServices
 */
@property (nonatomic, copy) NSArray *scanForPeripheralsWithServices;

// [peripheral discoverServices:self.discoverWithServices];
@property (nonatomic, copy) NSArray *discoverWithServices;

// [peripheral discoverCharacteristics:self.discoverWithCharacteristics forService:service];
@property (nonatomic, copy) NSArray *discoverWithCharacteristics;
/*!
 * 5s
 * The timeout time of connected device is 5s by default
 */
@property (nonatomic, assign) float connectTimeout;
/*!
 * 150
 * One packet size, default 150
 */
@property (nonatomic, assign) int dataPageSize;


- (instancetype)initWithscanForPeripheralsWithOptions:(NSDictionary *)scanForPeripheralsWithOptions
                         connectPeripheralWithOptions:(NSDictionary *)connectPeripheralWithOptions;

- (instancetype)initWithscanForPeripheralsWithOptions:(NSDictionary *)scanForPeripheralsWithOptions
                         connectPeripheralWithOptions:(NSDictionary *)connectPeripheralWithOptions
                       scanForPeripheralsWithServices:(NSArray *)scanForPeripheralsWithServices
                                 discoverWithServices:(NSArray *)discoverWithServices
                          discoverWithCharacteristics:(NSArray *)discoverWithCharacteristics;


@end

NS_ASSUME_NONNULL_END
