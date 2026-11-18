//
//  BluetoothTool.m
//  urovoiOSApp
//
//  Created by yhb on 2021/3/25.
//  Copyright © 2021 Noah. All rights reserved.
//

#import "BluetoothTool.h"

@implementation BluetoothTool

+ (instancetype)sharedInstance {
    static BluetoothTool *manager;
    static dispatch_once_t once;
    dispatch_once(&once, ^{
        manager = [BluetoothTool new];
    });
    return manager;
}

@end
