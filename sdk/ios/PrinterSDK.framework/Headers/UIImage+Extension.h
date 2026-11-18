//
//  UIImage+Extension.h
//  ImageDither
//
//  Created by zwwuchn on 8/5/19.
//  Copyright © 2019 zwwuchn. All rights reserved.
//

#import <UIKit/UIKit.h>
NS_ASSUME_NONNULL_BEGIN

@interface UIImage (Extension)
///  Image dithering
/// @param reverse  Whether to take the
/// @return  Process the finished picture
- (UIImage *)dither:(BOOL)reverse;

@end

NS_ASSUME_NONNULL_END
