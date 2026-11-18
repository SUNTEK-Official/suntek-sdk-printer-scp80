//
//  Utils.h
//  GSDK
//
//  Created by smarnet on 2018/6/22.
 // Copyright © 2018 Smarnet. All rights reserved
//

#import <UIKit/UIKit.h>
#import "CpclCommand.h"

@interface Utils : NSObject

 // / (Image Zoom)
 // / @param image (The original image)
 // / @param width (The target width)
+ (UIImage *)imageWithScaleImage:(UIImage *)image andScaleWidth:(int)width;

 // / ESC (ESC instruction picture data)
 // / @param image (Image)
+ (NSData *)printEscData:(UIImage *)image;

 // / TSC (TSC instruction picture data)
 // / @param image (Image)
+ (NSData *)printTscData:(UIImage *)image;

 // / Tsc (TSC instruction picture data)
 // / @param image (Image)
 // / @param mode 0: 3: (mode 0:Non-compacting 3:compression)
+ (NSData *)printTscData:(UIImage *)image andMode:(int)mode;

 // / CPCL (CPCL instruction picture data)
 // / @param image (Image)
+ (NSData *)printCpclData:(UIImage *)image;

 // / CPCL (CPCL instruction picture data)
 // / @param image (Image)
 // / @param threshold 0: 1-255: (Binarization threshold 0: automatic 1-255: manual)
+ (NSData *)printCpclData:(UIImage *)image withThreshold:(int)threshold;

 // / - (Image binarization - Specify Thresholds)
 // / @param image (Image)
 // / @param threshold 0-255 (threshold 0-255)
 // / @param reverse (Black and white and reverse)
+ (UIImage *)binaryzation:(UIImage *)image withThreshold:(int)threshold withReverse:(BOOL)reverse;

 // / - (Image binarization - Adaptive thresholds)
 // / @param image (Image)
 // / @param reverse (Black and white and reverse)
+ (UIImage *)binaryzationAutoThreshold:(UIImage *)image withReverse:(BOOL)reverse;

 // / (Image graying processing)
 // / @param image (Image)
+ (UIImage *)grayImage:(UIImage *)image;


/// Image processing. This method is used to process the picture to be printed, the picture will be grayscale and dither processing, to retain the picture information is more complete
 // / @param img (Image)
+ (UIImage *)convertGreyImgByFloyd:(UIImage *)img;

/// BarcodeType -> String
/// @param type BarcodeType
+ (NSString *)barcodeTypeToStr:(BarcodeType)type;

/// Alignment -> String
/// @param alignment Alignment
+ (NSString *)alignmentToStr:(Alignment)alignment;

/// TextAngle -> String
/// @param angle TextAngle
+ (NSString *)textAlignmentToStr:(TextAngle)angle;

/// QRCodeLevel -> String
/// @param level QRCodeLevel
+ (NSString *)qrcodeLevelToStr:(QRCodeLevel)level;

/// Mode -> String
/// @param mode Mode
+ (NSString *)qrcodeInputModeToStr:(Mode)mode;

/// Orientation -> String
/// @param orientation orientation
+ (NSString *)orientationToStr:(Orientation)orientation;

 // / (Checks if the string is a number)
 // / @param str (String)
+ (BOOL)checkStringIsNumber:(NSString *)str;

/// Graphics -> String
/// @param graphics Graphics
+ (NSString *)graphicsToStr:(Graphics)graphics;

 // / (screenshot)
 // / @param views (Objective view)
+ (UIImage *)view2Image:(UIView *)views;

 // / (Change the orientation of the picture)
 // / @param image (Image)
 // / @param orientation (orientation)
+ (UIImage *)image:(UIImage *)image rotation:(UIImageOrientation)orientation;

@end
