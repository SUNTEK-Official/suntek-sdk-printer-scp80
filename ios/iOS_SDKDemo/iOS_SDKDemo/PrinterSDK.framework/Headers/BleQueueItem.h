//
//  BleQueueItem.h
//  CodekSDK
//
//  Created by yhb on 2021/3/22.
//

#import <Foundation/Foundation.h>
#import "BleCallback.h"

NS_ASSUME_NONNULL_BEGIN

@interface BleQueueItem : NSObject

@property (nonatomic, copy)  NSData * _Nullable data;

@property (nonatomic, copy) __nullable WriteProgressBlock progress;

@property (nonatomic, copy) __nullable ReadDataBlock read;

@property (nonatomic, assign) BOOL processing;

@end

NS_ASSUME_NONNULL_END
