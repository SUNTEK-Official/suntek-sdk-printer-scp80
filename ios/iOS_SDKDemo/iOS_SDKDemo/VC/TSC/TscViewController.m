//
//  TscViewController.m
//  iOS_SDKDemo
//
//  Created by yhb on 2021/4/7.
//

#import "TscViewController.h"

@interface TscViewController ()

@end

@implementation TscViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    self.title = @"Tsc";
}

- (IBAction)textCommandAction:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:40 :20];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    [command addTextwithX:10 withY:10 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"Test text input"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)barCodeCommandAction:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:40 :20];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    [command add1DBarcode:10 :10 :@"128" :104 :0 :0 :3 :4 :@"DD2328609493"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)qrCodeCommandAction:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:40 :20];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    [command addQRCode:10 :10 :@"L" :4 :@"A" :0 :@"Bar code content"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)imageCommandAction:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:40 :20];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    UIImage *image = [UIImage imageNamed:@"123"];
    [command addBitmapwithX:10 withY:10 withMode:0 withWidth:80 withImage:image];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)tscCommandAction:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:75 :128];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    [command addBar:0 :120 :600 :2];
    [command addBar:0 :240 :600 :2];
    [command addBar:0 :400 :600 :2];
    [command addBar:0 :560 :600 :2];
    [command addBar:0 :640 :600 :2];
    [command addBar:0 :720 :600 :2];
    [command addBar:0 :816 :600 :2];
    [command addBar:0 :848 :600 :2];
    [command addBar:0 :888 :600 :2];
    [command addBar:40 :400 :2 :240];
    [command addBar:160 :640 :2 :80];
    [command addBar:360 :640 :2 :80];
    [command addBar:40 :720 :2 :96];
    [command addBox:456 :904 :560 :944 :2];
    [command addTextwithX:16 withY:96 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"*第93次打印：20-10-06 11:49"];
    [command addTextwithX:200 withY:48 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"到付:10元"];
    [command addTextwithX:80 withY:152 withFont:@"TSS32.BF2" withRotation:0 withXscal:2 withYscal:2 withText:@"574-402-00-001"];
    [command addTextwithX:160 withY:368 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"DD23 2860 9493 "];
    [command addTextwithX:0 withY:464 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"收"];
    [command addTextwithX:56 withY:424 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"张盼盼 15088965523"];
    [command addTextwithX:56 withY:464 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"福建省厦门市湖里区厦门市湖里区高崎南"];
    [command addTextwithX:56 withY:494 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"十二路2-10号"];
    [command addTextwithX:0 withY:588 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"寄"];
    [command addTextwithX:56 withY:576 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"邵娟 15060759932"];
    [command addTextwithX:56 withY:608 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"福建省厦门市湖里区福建省厦门市湖里区高崎南十二路8艾德航空工业园8号楼4-5层"];
    [command addTextwithX:32 withY:648 withFont:@"TSS32.BF2" withRotation:0 withXscal:2 withYscal:2 withText:@"402"];
    [command addTextwithX:368 withY:656 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"签收人:"];
    [command addTextwithX:368 withY:688 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"日期:"];
    [command addTextwithX:8 withY:752 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"服"];
    [command addTextwithX:8 withY:778 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"务"];
    [command addTextwithX:56 withY:728 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"寄件码: null"];
    [command addTextwithX:336 withY:728 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"运费: 10.00元"];
    [command addTextwithX:56 withY:760 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"取件员: 时明明"];
    [command addTextwithX:336 withY:760 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"运输方式: "];
    [command addTextwithX:56 withY:792 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"数量: 1"];
    [command addTextwithX:336 withY:792 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"重量:1 KG"];
    [command addTextwithX:8 withY:820 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"品名:茶叶"];
    [command addTextwithX:8 withY:860 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"备注:"];
    [command addTextwithX:464 withY:912 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"已验视"];
    [command addTextwithX:360 withY:952 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"DD23 2860 9493 "];
    [command addTextwithX:360 withY:992 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"发件网点:760030"];
    [command add1DBarcode:40 :256 :@"128" :104 :0 :0 :3 :4 :@"DD2328609493"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)FontCommand:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:80 :40];
    [command addDirection:0];
    [command addCls];
    [command addReference:0 :0];
    [command addTextwithX:0 withY:20 withFont:@"TSS16.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"TSS16.BF2 简体中文16*16(GB码)"];
    [command addTextwithX:0 withY:50 withFont:@"TSS20.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"TSS20.BF2 简体中文20*20(GB码)"];
    [command addTextwithX:0 withY:90 withFont:@"TST24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"TST24.BF2 繁體中文24*24(大五碼)"];
    [command addTextwithX:0 withY:130 withFont:@"TSS24.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"TSS24.BF2 简体中文24*24(GB码)"];
    [command addTextwithX:0 withY:170 withFont:@"K" withRotation:0 withXscal:1 withYscal:1 withText:@"K 한국어24*24Font(KS숫자)"];
    [command addTextwithX:0 withY:210 withFont:@"SCN.TTF" withRotation:0 withXscal:1 withYscal:1 withText:@"SCN.TTF 简体中文24*24(GB码)"];
    [command addTextwithX:0 withY:250 withFont:@"USER.TTF" withRotation:0 withXscal:1 withYscal:1 withText:@"USER.TTF Je suis désolé24*24(UTF-8)"];
    [command addTextwithX:0 withY:290 withFont:@"TSS32.BF2" withRotation:0 withXscal:1 withYscal:1 withText:@"TSS32.BF2 简体中文32*32(GB码)"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)FontCommand1:(id)sender {
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:80 :60];
    [command addDirection:0];
    [command addCls];
    [command addReference:0 :0];
    [command addTextwithX:0 withY:20 withFont:@"1" withRotation:0 withXscal:1 withYscal:1 withText:@"1 8*12dot ABCabc"];
    [command addTextwithX:0 withY:50 withFont:@"2" withRotation:0 withXscal:1 withYscal:1 withText:@"2 12*20dot ABCabc"];
    [command addTextwithX:0 withY:90 withFont:@"3" withRotation:0 withXscal:1 withYscal:1 withText:@"3 16*24dot ABCabc"];
    [command addTextwithX:0 withY:130 withFont:@"4" withRotation:0 withXscal:1 withYscal:1 withText:@"4 24*32dot ABCabc"];
    [command addTextwithX:0 withY:170 withFont:@"5" withRotation:0 withXscal:1 withYscal:1 withText:@"5 32*48dot ABCabc"];
    [command addTextwithX:0 withY:230 withFont:@"6" withRotation:0 withXscal:1 withYscal:1 withText:@"6 14*19dot OCR-B ABCabc"];
    [command addTextwithX:0 withY:270 withFont:@"7" withRotation:0 withXscal:1 withYscal:1 withText:@"7 21*27dot OCR-B ABCabc"];
    [command addTextwithX:0 withY:310 withFont:@"8" withRotation:0 withXscal:1 withYscal:1 withText:@"8 14*25dot OCR-A ABCabc"];
    [command addTextwithX:0 withY:350 withFont:@"9" withRotation:0 withXscal:1 withYscal:1 withText:@"9 9*17dot ABCabc"];
    [command addTextwithX:0 withY:390 withFont:@"10" withRotation:0 withXscal:1 withYscal:1 withText:@"10 12*24dot ABCabc"];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
}
- (IBAction)ImageCommand1Action:(id)sender {
    CIImage *beginImage = [CIImage imageWithCGImage:[UIImage imageNamed:@"111"].CGImage];
    CIFilter * filter = [CIFilter filterWithName:@"CIColorControls"];
    [filter setValue:beginImage forKey:kCIInputImageKey];
    [filter setValue:[NSNumber numberWithFloat:0.20f] forKey:kCIInputSaturationKey];
    [filter setValue:[NSNumber numberWithFloat:0.75f] forKey:kCIInputBrightnessKey];
    [filter setValue:[NSNumber numberWithFloat:1.75f] forKey:kCIInputContrastKey];
    CIImage *outputImage = [filter outputImage];
    CIContext *context = [CIContext contextWithOptions:nil];
    CGImageRef cgimg = [context createCGImage:outputImage fromRect:[outputImage extent]];
    UIImage *newImg = [UIImage imageWithCGImage:cgimg];
    CGImageRelease(cgimg);
    
    newImg = [Utils imageWithScaleImage:newImg andScaleWidth:560];
    
    TscCommand *command = [[TscCommand alloc]init];
    [command addSize:73 :110];
    [command addDirection:0];
    [command addReference:0 :0];
    [command addPeel:@"OFF"];
    [command addTear:@"OFF"];
    [command addCodepage:@"437"];
    [command addCls];
    [command addBitmapwithX:0 withY:0 withMode:3 withWidth:560 withImage:[newImg dither:NO]];
    [command addPrint:1 :1];
    [Manager.bleManager write:[command getCommand]];
    
    
}

@end
