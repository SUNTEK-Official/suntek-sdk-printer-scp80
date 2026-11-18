//
//  BluetoothTool.h
//  urovoiOSApp
//
//  Created by yhb on 2021/3/25.
//  Copyright © 2021 Noah. All rights reserved.
//

#import <Foundation/Foundation.h>
NS_ASSUME_NONNULL_BEGIN

#define Manager [BluetoothTool sharedInstance]

@interface BluetoothTool : NSObject


@property (nonatomic, strong) BleManager *bleManager;

+ (instancetype)sharedInstance;

@end

NS_ASSUME_NONNULL_END
