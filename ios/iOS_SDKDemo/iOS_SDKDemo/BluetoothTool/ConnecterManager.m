//
//  ConnecterManager.m
//  GSDK
//
 // Created by on 2018/6/21
 // Copyright © 2018 Smarnet. All rights reserved
//

#import "ConnecterManager.h"

@interface ConnecterManager()
@property(nonatomic,copy)ConnectDeviceState connecterState;
@property(nonatomic,copy)UpdateState updateState;
@property(nonatomic,strong)NSTimer *connectTimeoutTimer;
@end

@implementation ConnecterManager

static ConnecterManager *manager;
static dispatch_once_t once;

+(instancetype)sharedInstance {
    dispatch_once(&once, ^{
        manager = [[ConnecterManager alloc]init];
    });
    return manager;
}

- (void)attempDealloc {
   once = 0;
   manager = nil;
}

/**
 *  方法说明：连接指定ip和端口号的网络设备
 *  @param ip 设备的ip地址
 *  @param port 设备端口号
 *  @param connectState 连接状态
 *  @param callback 读取数据接口
 */
-(void)connectIP:(NSString *)ip port:(int)port connectState:(void (^)(ConnectState state))connectState callback:(void (^)(NSData *data))callback {
    if([self.connectTimeoutTimer isValid]){
        [self.connectTimeoutTimer invalidate];
    }
    self.connectTimeoutTimer = [NSTimer scheduledTimerWithTimeInterval:5.0f target:self selector:@selector(connectTimeout) userInfo:nil repeats:NO];
    
    dispatch_async(dispatch_get_global_queue(0, 0), ^{
        self.state = connectState;
        [self updateConnectState];
        if (self.ethernetConnecter == nil) {
            self.currentConnMethod = ETHERNET;
            [self initConnecter:self.currentConnMethod];
        }
        [self.ethernetConnecter connectIP:ip port:port connectState:self.connecterState callback:callback];
    });
}

-(void)connectTimeout {
    NSLog(@"connectTimeout");
    if (!_isConnected) {
        NSLog(@"connectTimeout 断开连接");
//        [self close];
    }
}

/**
 *  方法说明: 更新连接状态
 */
-(void)updateConnectState {
    WEAKSELF
    self.connecterState = ^(ConnectState state) {
        switch (state) {
            case CONNECT_STATE_CONNECTED:
                Log(@"连接成功");
                if (weakSelf.connectTimeoutTimer !=nil && [self.connectTimeoutTimer isValid]) {
                    [weakSelf.connectTimeoutTimer invalidate];
                }
                if (weakSelf.isQueryPrinterCommandType) {
                    [weakSelf checkEscOrTscPrinter:weakSelf.state];
                } else {
                    weakSelf.isConnected = YES;
                    weakSelf.state(state);
                }
                break;
            case CONNECT_STATE_DISCONNECT:
                Log(@"断开连接");
                weakSelf.state(state);
                weakSelf.isConnected = NO;
                weakSelf.type = UNKNOWN;
                break;
            default:
                weakSelf.state(state);
                break;
        }
        [[NSNotificationCenter defaultCenter]postNotification:[NSNotification notificationWithName:CONNECT_NOTIFICATION object:nil]];
    };
    
}

//-(void)reset{
//    self.isConnected = NO;
//    self.type = UNKNOWN;
//}

-(void)parameterInitialize {
    self.type = UNKNOWN;
    self.bleConnecter = nil;
    self.ethernetConnecter = nil;
    self.connecter = nil;
    self.isConnected = NO;
}

-(void)updateBluetoothState {
    WEAKSELF;
    self.updateState = ^(NSInteger state) {
        switch (state) {
            case CBCentralManagerStatePoweredOff:
                //                weakself.isConnected = NO;
                [weakSelf close];
                //                weakself.state(CONNECT_STATE_DISCONNECT);
                weakSelf.updateCenterBluetoothState(state);
                break;
            default:
                weakSelf.updateCenterBluetoothState(state);
                break;
        }
    };
}

-(void)checkEscOrTscPrinter:(void(^)(ConnectState state))connectState{
    self.type = UNKNOWN;
    Log(@"send tsc");
    unsigned char tsc[] = {0x1b,0x21,0x3f};
    [self write:[NSData dataWithBytes:tsc length:sizeof(tsc)] receCallBack:^(NSData *data) {
        Log(@"tsc rece data -> %@",data.description);
        if (data.length > 0) {
            self.type = TSC;
            self.isConnected = YES;
            connectState(CONNECT_STATE_CONNECTED);
        }
    }];
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(1.5f * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
        if (self.type == UNKNOWN) {
            unsigned char esc[] = {0x10,0x04,0x02};
            Log(@"send esc");
            [self write:[NSData dataWithBytes:esc length:sizeof(esc)] receCallBack:^(NSData *data) {
                Log(@"esc rece data -> %@",data.description);
                if (data.length > 0) {
                    self.type = ESC;
                    self.isConnected = YES;
                    connectState(CONNECT_STATE_CONNECTED);
                }
            }];
            dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(1.5f * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
                if(self.type == UNKNOWN) {
                    Log(@"没有收到返回值，强制断开连接");
                    [self close];
                    connectState(CONNECT_STATE_FAILT);
                }
            });
        }
    });
}

/**
 *  方法说明：扫描外设
 *  @param serviceUUIDs 需要发现外设的UUID，设置为nil则发现周围所有外设
 *  @param options  其它可选操作
 *  @param discover 发现的设备
 */
-(void)scanForPeripheralsWithServices:(nullable NSArray<CBUUID *> *)serviceUUIDs options:(nullable NSDictionary<NSString *, id> *)options discover:(void(^_Nullable)(CBPeripheral *_Nullable peripheral,NSDictionary<NSString *, id> *_Nullable advertisementData,NSNumber *_Nullable RSSI))discover {
    self.isScanning = YES;
    [_bleConnecter scanForPeripheralsWithServices:serviceUUIDs options:options discover:discover];
}

/**
 *  方法说明：更新蓝牙状态
 *  @param state 蓝牙状态
 */
-(void)didUpdateState:(void(^)(NSInteger state))state {
    self.updateCenterBluetoothState = state;
    [self updateBluetoothState];
    if (_bleConnecter == nil) {
        self.currentConnMethod = BLUETOOTH;
        [self initConnecter:self.currentConnMethod];
    }
    [_bleConnecter didUpdateState:self.updateState];
}

-(void)initConnecter:(ConnectMethod)connectMethod {
    self.isConnected = NO;
    switch (connectMethod) {
        case BLUETOOTH:
            _bleConnecter = [BLEConnecter new];
            _bleConnecter.readTotalCallback = self.readTotalCallback;
            _connecter = _bleConnecter;
            break;
        case ETHERNET:
            _ethernetConnecter = [EthernetConnecter new];
            _connecter = _ethernetConnecter;
            break;
        default:
            break;
    }
}

/**
 *  方法说明：停止扫描
 */
-(void)stopScan {
    self.isScanning = NO;
    [_bleConnecter stopScan];
}

/**
 *  连接
 */
-(void)connectPeripheral:(CBPeripheral *)peripheral options:(nullable NSDictionary<NSString *,id> *)options timeout:(NSUInteger)timeout connectBlack:(void(^_Nullable)(ConnectState state)) connectState{
    self.peripheral = peripheral;
    self.state = connectState;
    [self updateConnectState];
    if (self.peripheral) {
        [_bleConnecter connectPeripheral:self.peripheral options:options timeout:timeout connectBlack:self.connecterState];
    }
    
}

/**
 *  方法说明: 指定外设UUID连接
 *  @param identifier        外设UUID
 *  @param time              设置超时时间（0 < time）时间单位秒(s)
 *  @param connectState      连接状态
 */
-(void)connectPeripheralFormUUID:(CBUUID *_Nullable)identifier timeout:(NSUInteger)time connectState:(void(^)(ConnectState state))connectState {
    self.state = connectState;
    [self updateConnectState];
    [_bleConnecter connectPeripheralFormUUID:identifier timeout:time connectState:connectState];
}


-(void)connectPeripheral:(CBPeripheral * _Nullable)peripheral options:(nullable NSDictionary<NSString *,id> *)options {
    self.peripheral = peripheral;
    if (self.peripheral) {
        [_bleConnecter connectPeripheral:self.peripheral options:options];
    }
}

-(void)write:(NSData *_Nullable)data progress:(void(^_Nullable)(NSUInteger total,NSUInteger progress))progress receCallBack:(void (^_Nullable)(NSData *_Nullable))callBack {
    [_bleConnecter write:data progress:progress receCallBack:callBack];
}

-(void)write:(NSData *)data receCallBack:(void (^)(NSData *))callBack {
#ifdef DEBUG
    Log(@"[ConnecterManager] write:receCallBack:");
#endif
    _bleConnecter.writeProgress = nil;
    [_connecter write:data receCallBack:callBack];
}

-(void)write:(NSData *)data {
#ifdef DEBUG
    Log(@"[ConnecterManager] write:");
#endif
    _bleConnecter.writeProgress = nil;
    [_connecter write:data];
}

-(void)close {
    [_connecter close];
}

- (BOOL)isSending {
    return _bleConnecter.isSending;
}

- (void)cancel {
    [_bleConnecter cancel];
}

@end
