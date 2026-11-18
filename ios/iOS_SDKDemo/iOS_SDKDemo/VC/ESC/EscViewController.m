//
//  EscViewController.m
//  iOS_SDKDemo
//
//  Created by yhb on 2021/4/7.
//

#import "EscViewController.h"

@interface EscViewController ()

@end

@implementation EscViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    self.title = @"Esc";
}

- (IBAction)textCommandAction:(id)sender {
    EscCommand *command = [[EscCommand alloc]init];
    [command addInitializePrinter];
    [command addPrintAndFeedLines:5];

    [command addSetJustification:1];
    [command addPrintAndLineFeed];
    [command addPrintMode:0];
    [command addText:@"Test text input"];
    [command addPrintAndFeedLines:5];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)barCodeCommandAction:(id)sender {
    EscCommand *command = [[EscCommand alloc]init];
    [command addInitializePrinter];
    [command addPrintAndFeedLines:5];
    [command addSetBarcodeWidth:2];
    [command addSetBarcodeHeight:60];
    [command addSetBarcodeHRPosition:2];
    [command addCODE128:'B' : @"ABC1234567890"];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)qrCodeCommandAction:(id)sender {
    EscCommand *command = [[EscCommand alloc]init];
    [command addInitializePrinter];
    [command addPrintAndFeedLines:5];
    NSString *content = @"Gprinter";
    int pL,pH,k;
    k = (int)content.length+3;
    pH = k / 256;
    pL = k - pH;
    [command addQRCodeSizewithpL:0 withpH:0 withcn:0 withyfn:0 withn:5];
    [command addQRCodeSavewithpL:pL withpH:pH withcn:0x31 withyfn:0x50 withm:0x30 withData:[content dataUsingEncoding:NSUTF8StringEncoding]];
    [command addQRCodePrintwithpL:0 withpH:0 withcn:0 withyfn:0 withm:0];
    [command addPrintAndLineFeed];
    
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)imageCommandAction:(id)sender {
    EscCommand *command = [[EscCommand alloc]init];
    [command addInitializePrinter];
    [command addPrintAndFeedLines:5];
    UIImage *image = [UIImage imageNamed:@"123"];
    [command addOriginrastBitImage:image];
    [command addPrintAndFeedLines:5];
    [Manager.bleManager write:[command getCommand]];
}

- (IBAction)escCommandAction:(id)sender {
    EscCommand *command = [[EscCommand alloc]init];
    [command addInitializePrinter];
    [command addPrintAndFeedLines:5];
    [command addSetJustification:1];
    [command addPrintMode: 0|8|16|32];
    [command addText:@"Print text\n"];
    [command addPrintAndLineFeed];
    [command addPrintMode: 0];
    [command addText:@"Welcome to use Smarnet printer!"];
    [command addPrintAndLineFeed];
    [command addSetJustification:0];
    [command addText:@"Smarnet"];
    [command addSetHorAndVerMotionUnitsX:7 Y:0];
    [command addSetAbsolutePrintPosition:6];
    [command addText:@"network"];
    [command addSetAbsolutePrintPosition:10];
    [command addText:@"printer"];
    [command addPrintAndLineFeed];
    NSString *content = @"Gprinter";
    [command addQRCodeSizewithpL:0 withpH:0 withcn:0 withyfn:0 withn:5];
    [command addQRCodeSavewithpL:0x0b withpH:0 withcn:0x31 withyfn:0x50 withm:0x30 withData:[content dataUsingEncoding:NSUTF8StringEncoding]];
    [command addQRCodePrintwithpL:0 withpH:0 withcn:0 withyfn:0 withm:0];
    [command addPrintAndLineFeed];
    
    [command addSetBarcodeWidth:2];
    [command addSetBarcodeHeight:60];
    [command addSetBarcodeHRPosition:2];
    [command addCODE128:'B' : @"ABC1234567890"];
    
    [command addPrintAndLineFeed];
    
    UIImage *image = [UIImage imageNamed:@"123"];
    [command addOriginrastBitImage:image];
    [command addPrintAndFeedLines:5];
    
    [Manager.bleManager write:[command getCommand]];
}


@end
