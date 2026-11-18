//
//  Command.h
//  GSDK
//
//  Created by smarnet on 2019/2/26.
//  Copyright © 2019 Smarnet. All rights reserved.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

/**
 *  @enum TextAngle
 *
 *  @discussion 文本旋转角度(Text rotation Angle)
 */
typedef NS_ENUM (NSUInteger,PrinterMode) {
    TSC_COMMAND, // /<TSC (TSC Command Type)
    ESC_COMMAND, // /<ESC (ESC Command Type)
    CPCL_COMMAND, // /<CPCL (CPCL Command Type)
};

@interface Command : NSObject

@property(nonatomic, strong) NSMutableData *mCommandData;


 // / @return
- (NSData *)getCommand;


 // / @param str
- (void)addStrToCommand:(NSString *)str;


 // / @param str
 // / @param encoding
- (void)addStrToCommand:(NSString *)str encoding:(NSStringEncoding)encoding;


 // / @param image
 // / @return
- (UIImage *)convertGreyImgByFloyd:(UIImage *)image;

 // / data command
 // / @param data
- (void)addNSDataToCommand:(NSData*) data;

 // / . , getCommand
 // / @param mode
 // / @return
- (NSData *)swithPrinterMode:(PrinterMode)mode;

 // / . , getCommand
 // / @param name
 // / @return
- (NSData *)addChangeBluetoothName:(NSString *)name;

 // / PIN ( ). , getCommand
 // / @param code . code.length=4. 0-9
 // / @return PIN
- (NSData *)addChangeBluetoothPinCode:(NSString *)code;

@end

NS_ASSUME_NONNULL_END
