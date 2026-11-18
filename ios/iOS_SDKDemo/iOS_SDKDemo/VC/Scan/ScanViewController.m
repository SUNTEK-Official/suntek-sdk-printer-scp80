//
//  ScanViewController.m
//  iOS_SDKDemo
//
//  Created by yhb on 2021/4/7.
//

#import "ScanViewController.h"

@interface ScanViewController ()<UITableViewDelegate, UITableViewDataSource>

@property (nonatomic, strong) NSMutableArray *dataArray;

@property (weak, nonatomic) IBOutlet UITableView *tableView;


@end

@implementation ScanViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    self.title = @"Scan";
    [self initBluetooth];
    
    self.dataArray = @[].mutableCopy;
    
    [self setupUI];
}
#pragma mark - UI
- (void)setupUI {
    {
        self.tableView.delegate = self;
        self.tableView.dataSource = self;
        [self.tableView registerClass:[UITableViewCell class] forCellReuseIdentifier:NSStringFromClass([UITableViewCell class])];
    }
}

#pragma mark - Bluetooth
- (void)initBluetooth {
    WeakSelf(self);
    BleOptions *options = [BleOptions new];{
        options.scanForPeripheralsWithOptions = @{CBCentralManagerScanOptionAllowDuplicatesKey:@YES};
    }
    Manager.bleManager.bleOptions = options;
    [Manager.bleManager setFilterOnDiscoverPeripherals:^BOOL(NSString * _Nonnull peripheralName, NSDictionary * _Nonnull advertisementData, NSNumber * _Nonnull RSSI) {
        if (peripheralName.length) {
            return YES;
        }
        return NO;
    }];
    [Manager.bleManager cancelScan];
    if ([Manager.bleManager getCentralManagerState] == CBManagerStatePoweredOn) {
        [self startScane];
    } else {
        [Manager.bleManager setBlockOnCentralManagerDidUpdateState:^(CBCentralManager * _Nonnull central) {
            switch (central.state) {
                case CBManagerStateUnsupported:
                    Log(@"The platform/hardware doesn't support Bluetooth Low Energy.");
                    break;
                case CBManagerStateUnauthorized:
                    Log(@"The app is not authorized to use Bluetooth Low Energy.");
                    break;
                case CBManagerStatePoweredOff:
                    Log(@"Bluetooth is currently powered off.");
                    break;
                case CBManagerStatePoweredOn:
                    [weakself startScane];
                    Log(@"Bluetooth power on");
                    break;
                case CBManagerStateUnknown:

                    break;
                case CBManagerStateResetting:
                    
                    break;
            }
        }];
    }
}

- (void)startScane {
    WeakSelf(self);
    [Manager.bleManager scanPeripheralsWithBlock:^(CBCentralManager * _Nonnull central, CBPeripheral * _Nonnull peripheral, NSDictionary * _Nonnull advertisementData, NSNumber * _Nonnull RSSI) {
        if (![weakself.dataArray containsObject:peripheral]) {
            [weakself.dataArray addObject:peripheral];
            [weakself.tableView reloadData];
        }
    }];
}

- (void)dealloc
{
    [Manager.bleManager cancelScan];
}

#pragma mark - UITableViewDelegate
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    WeakSelf(self);
    void(^block)(void) = ^{
        CBPeripheral *peripheral = weakself.dataArray[indexPath.row];
        [Manager.bleManager connectToPeripheral:peripheral withConnected:^(CBCentralManager * _Nonnull central, CBPeripheral * _Nonnull peripheral) {
            
        } withFail:^(CBCentralManager * _Nonnull central, CBPeripheral * _Nonnull peripheral, NSError * _Nonnull error) {
            
        } withState:^(ConnectState state) {
            switch (state) {
                case CONNECT_STATE_CONNECTED:
                {
                    [SVProgressHUD showSuccessWithStatus:@"Connected"];
                }
                    break;
                case CONNECT_STATE_CONNECTING:
                {
                    [SVProgressHUD showWithStatus:@"Connecting..."];
                }
                    break;
                case NOT_FOUND_DEVICE:
                {
                    [SVProgressHUD showWithStatus:@"Device not found"];
                }
                    break;
                case CONNECT_STATE_TIMEOUT:
                {
                    [SVProgressHUD showErrorWithStatus:@"Connection timeout"];
                }
                    break;
                case CONNECT_STATE_FAILT:
                {
                    [SVProgressHUD showErrorWithStatus:@"Connection fail"];
                }
                    break;
                case CONNECT_STATE_DISCONNECT:
                {
                    [SVProgressHUD showErrorWithStatus:@"Disconnect"];
                }
                    break;
            }
        }];

    };
    if ([Manager.bleManager isConnected]) {
        [Manager.bleManager cancelPeripheralConnection:[Manager.bleManager getConnectedDevice]];
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(0.5 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
            block();
        });
    } else {
        block();
    }
    [tableView deselectRowAtIndexPath:indexPath animated:false];
    
}

#pragma mark - UITableViewDataSource
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return self.dataArray.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:NSStringFromClass([UITableViewCell class]) forIndexPath:indexPath];
    CBPeripheral *peripheral = self.dataArray[indexPath.row];
    cell.textLabel.text = [NSString stringWithFormat:@"%@-%@", peripheral.name, peripheral.identifier.UUIDString];
    cell.textLabel.adjustsFontSizeToFitWidth = YES;
    return cell;
}

@end
