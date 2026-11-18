//
//  CpclViewController.m
//  iOS_SDKDemo
//
//  Created by yhb on 2021/4/7.
//

#import "CpclViewController.h"

@interface CpclViewController ()

@end

@implementation CpclViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    self.title = @"Cpcl";
}

- (IBAction)textCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addPageWidth:576];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:100 qty:1];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:10 y:10 content:@"Test text input"];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)barCodeCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:100 qty:1];
    [command addPageWidth:576];
    [command addBarcodeOrientation:HORIZONTAL type:CODE128 withWidth:1 ratio:1 height:55 x:10 y:10 content:@"JT0000543899225"];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)qrCodeCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:100 qty:1];
    [command addPageWidth:576];
    [command addQRCodeVertical:HORIZONTAL x:20 y:20 m:2 u:2];
    [command addQRCodeLevel:(QRCODE_LEVEL_H) content:@"http://weixin.qq.com/r/fCk7I112345TrRNA93xu"];
    [command endQRCode];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)imageCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:300 qty:1];
    [command addPageWidth:576];
    [command addImageGraphics:(COMPRESSED_GRAPHICS) width:580 x:10 y:10 image:[UIImage imageNamed:@"456"] withThreshold:0];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)pdf417CommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:100 qty:1];
    [command addPageWidth:576];
    [command addPDF417Orientation:(HORIZONTAL) x:10 y:10 XD:2 YD:10 C:3 S:1 content:@"123456789"];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)datamatrixCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:100 qty:1];
    [command addPageWidth:576];
    [command addDatamatrixWithX:10 y:10 H:5 content:@"123456789"];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)cpclCommandAction:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:1000 qty:1];
    [command addPageWidth:576];
    [command addLineX0:10 y0:6 x1:572 y1:6 width:2];
    [command addLineX0:10 y0:110 x1:572 y1:110 width:1];
    [command addLineX0:10 y0:192 x1:572 y1:192 width:2];
    [command addLineX0:10 y0:251 x1:572 y1:251 width:1];
    [command addLineX0:10 y0:407 x1:462 y1:407 width:2];
    [command addLineX0:10 y0:526 x1:462 y1:526 width:1];
    [command addLineX0:84 y0:563 x1:462 y1:563 width:2];
    [command addLineX0:10 y0:600 x1:572 y1:600 width:1];
    [command addLineX0:10 y0:734 x1:572 y1:734 width:2];
    [command addLineX0:10 y0:868 x1:572 y1:868 width:1];
    [command addLineX0:10 y0:979 x1:572 y1:979 width:2];
    [command addLineX0:10 y0:6 x1:10 y1:979 width:2];
    [command addLineX0:84 y0:251 x1:84 y1:600 width:2];
    [command addLineX0:210 y0:526 x1:210 y1:600 width:2];
    [command addLineX0:292 y0:192 x1:292 y1:251 width:2];
    [command addLineX0:336 y0:526 x1:336 y1:600 width:2];
    [command addLineX0:462 y0:251 x1:462 y1:600 width:2];
    [command addLineX0:572 y0:6 x1:572 y1:979 width:2];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:298 y:84 content:@"服务热线：400 820 1666"];
    [command addSetmagWidth:2 height:2];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:4 size:0 x:160 y:14 content:@"快递样张"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:48 y:128 content:@"150 450-00 000"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:4 size:0 x:116 y:206 content:@"中山"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:299 y:199 content:@"打印时间："];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:415 y:199 content:@"2020-10-06"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:415 y:225 content:@"10:22:40 1/1"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:36 y:300 content:@"收"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:258 content:@"罗小福 13632566635"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:295 content:@"广东省 珠海市香洲区珠海大道"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:332 content:@"自定义速递"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:36 y:459 content:@"寄"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:414 content:@"熊小斌 13726299557"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:448 content:@"广东省 中山市坦洲镇七村果子街"];
    [command addSetmagWidth:5 height:5];
    [command addSetBlod:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:91 y:436 content:@"450-00"];
    [command addSetBlod:0];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:102 y:533 content:@"代收货款"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:228 y:533 content:@"到付运费"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:354 y:533 content:@"寄件重量"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:367 y:570 content:@"0.20KG"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_270) withFont:24 size:0 x:491 y:340 content:@"JT0000543899225"];
    [command addBarcodeOrientation:VERTICAL type:CODE128 withWidth:1 ratio:1 height:55 x:499 y:585 content:@"JT0000543899225"];
    [command addBarcodeOrientation:HORIZONTAL type:CODE128 withWidth:2 ratio:1 height:70 x:36 y:614 content:@"JT0000543899225"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:151 y:696 content:@"JT0000543899225"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:513 y:696 content:@"1/1"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:20 y:750 content:@"备注:"];
    [command addQRCodeVertical:HORIZONTAL x:24 y:882 m:2 u:2];
    [command addQRCodeLevel:(QRCODE_LEVEL_H) content:@"http://weixin.qq.com/r/fCk7I112345TrRNA93xu"];
    [command endQRCode];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:121 y:882 content:@"签收人/时间"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:55 size:0 x:121 y:920 content:@"您的签收代表您已验收此包裹，并确认商品信息无误,包裹完好"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:55 size:0 x:121 y:950 content:@"、没有划痕、破损等表面质量问题"];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:330 y:830 content:@"打印:2次"];
    [command addLineX0:480 y0:825 x1:560 y1:825 width:2];
    [command addLineX0:480 y0:860 x1:560 y1:860 width:2];
    [command addLineX0:480 y0:825 x1:480 y1:860 width:2];
    [command addLineX0:560 y0:825 x1:560 y1:860 width:2];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:485 y:830 content:@"已检视"];
    [command addPOPrint];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)FontCommand:(id)sender {
    CpclCommand *command = [CpclCommand new];
    [command addInitializePrinterOffset:0 horizontalDpi:200 verticalDpi:200 height:1000 qty:1];
    [command addPageWidth:576];
    [command addSetmagWidth:1 height:1];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:0 x:10 y:10 content:@"Font0 12*12 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:1 x:10 y:30 content:@"Font0 20*12 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:2 x:10 y:55 content:@"Font0 20*20 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:3 x:10 y:85 content:@"Font0 32*20 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:4 x:10 y:115 content:@"Font0 36*20 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:5 x:10 y:140 content:@"Font0 32*36 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:6 x:10 y:175 content:@"Font0 42*46 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:55 x:10 y:215 content:@"Font0 55*55 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:0 size:26 x:10 y:270 content:@"Font0 16*16 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:1 size:0 x:10 y:300 content:@"Font1 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:2 size:0 x:10 y:330 content:@"Font2 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:3 size:0 x:10 y:360 content:@"Font3 20*20 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:4 size:0 x:10 y:390 content:@"Font4 32*32 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:4 size:3 x:10 y:420 content:@"Font4 48*48 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:4 size:4 x:10 y:460 content:@"Font4 64*48 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:5 size:0 x:10 y:508 content:@"Font5 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:7 size:0 x:10 y:538 content:@"Font7 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:8 size:0 x:10 y:568 content:@"Font8 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:20 size:0 x:10 y:598 content:@"Font20 16*16 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:0 x:10 y:620 content:@"Font24 24*24 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:24 size:3 x:10 y:646 content:@"Font24 48*48 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:28 size:0 x:10 y:694 content:@"Font28 28*28 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:55 size:0 x:10 y:728 content:@"Font55 16*16 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:55 size:3 x:10 y:750 content:@"Font55 32*32 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:56 size:0 x:10 y:780 content:@"Font56 32*32 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:57 size:0 x:10 y:820 content:@"Font57 12*12 中文ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:88 size:0 x:10 y:845 content:@"Font88 11*11 Je suis désolé"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:88 size:32 x:10 y:860 content:@"Font88 32*32 Je suis désolé"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:89 size:0 x:10 y:900 content:@"Font89 24*24 한국어(KS숫자)ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:90 size:0 x:10 y:930 content:@"Font90 24*24 繁體中文(大五碼)ABCabc"];
    [command addTextAngle:(TEXT_ANGLE_0) withFont:91 size:0 x:10 y:960 content:@"Font91 24*24 繁體中文(大五碼)ABCabc"];
    [command addPrint];
    [Manager.bleManager write:[command getCommand]];
}

@end
