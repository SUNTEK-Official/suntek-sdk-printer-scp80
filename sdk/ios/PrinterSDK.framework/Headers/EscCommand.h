//
//  EscCommand.h
//  Gprinter
//
//  Created by smarnet on 14/12/22.
//  Copyright (c) 2014 Smarnet. All rights reserved.
//

#import "Command.h"

@interface EscCommand : Command

/// UPCA
/// Print UPCA barcode
/// @param content 0-911
/// The data range is 0-9 and the length is 11 bits
- (void)addUPCAtest:(NSString *)content;

/// Add Text
/// @param text  Text
- (void)addText:(NSString *)text;

/// 0x1b,0x40
/// Printer initialization, must be the first print command 0x1b,0x40
- (void)addInitializePrinter;

/// 8
/// Print and move the number of lines of paper, the default is 8 lines, after printing the content to send
/// @param n  Lines
- (void)addPrintAndFeedLines:(int)n;

/// Line feed
- (void)addPrintAndLineFeed;

/// 0x1B 0x21 n(0-255)n
/// Set the print mode, 0x1B 0x21N (0-255), and set the character print mode according to the value of n
/// @param n n00000000(0X0),10001000(0X88)00001000(0X08)10000000(0X80)
/// The binary default for parameter n is 00000000(0X0),10001000(0X88) for underscore and bold, 00001000(0X08) for bold, and 10000000(0X80) for underscore
- (void)addPrintMode:(int)n;

/// 0
/// Set the international character set. Default is US 0
/// @param n
/// Character set number
- (void)addSetInternationalCharacterSet:(int)n;

/// 90°0
/// Sets whether the character is rotated by 90°. Default is 0
/// @param n
/// Whether the rotation
- (void)addSet90ClockWiseRotatin:(int)n;

/// Set Alignment
/// @param n 0,1,2
/// 0 left aligned,1 middle aligned,2 right aligned
- (void)addSetJustification:(int)n;

/// Open the box
/// @param m  Money box pin number
/// @param t1  High level time
/// @param t2  Low level time
- (void)addOpenCashDawer:(int)m :(int)t1 :(int)t2;

///  buzzer
/// @param m  Alarm lights and beep times
/// @param t  Time
/// @param n  way
- (void)addSound:(int)m :(int)t :(int)n;

/// 30
/// Set the line spacing, do not set, the printer defaults to 30
/// @param n
/// Line spacing height, including text
- (void)addLineSpacing:(int)n;

///  /
/// Set upside-down mode to select/deselect upside-down print mode.
/// @param n 0 0 ≤n ≤255 n0n1n
/// The default value is 0, 0 ≤n ≤255. When the lowest value of n is 0, cancel the inverted printing mode. When the lowest order of n is 1, choose the inverted printing mode. Only the lowest order of n is valid;
- (void)addSetUpsideDownMode:(int)n;

/// 2n=0x11
/// Sets character magnification to no magnification and 2 times magnification, n=0x11
/// @param n n=width | height 0 ≤n ≤255 1 ≤  ≤81 ≤  ≤8[] 0 2 4 7
/// N = width | magnification, height width 0 n 255 or less or less (1 longitudinal magnification 8, 1 or less or less lateral or less with multiple 8) or less [description] with 0 to 2 choose height of characters, four to seven choice character width
- (void)addSetCharcterSize:(int)n;

/// Set anti-white mode, black and white anti-print mode
/// @param n n0n1n
/// Whether to reverse white or not. When the lowest order of n is 0, cancel the reverse printing mode. When the lowest order of n is 1, the reverse print mode is selected. Only the lowest order of n is valid;
- (void)addSetReverseMode:(int)n;

/// Printer real-time status request
/// @param n 1 ≤n ≤4
/// PRINTER_STATUS  1
/// PRINTER_OFFLINE 2
/// PRINTER_ERROR   3
/// PRINTER_PAPER   4
- (void)queryRealtimeStatus:(int)n;

/// Set up paper cutting after paper walking
/// @param n
/// Paper distance
- (void)addCutPaperAndFeed:(int)n;

/// paper cut
/// @param m 01
/// 0 is a full cut, 1 is a half cut
- (void)addCutPaper:(int)m;

/// HRI
/// Set bar code identifiable characters and select the printing position of HRI characters
/// @param n 0, 48   1, 49   2, 50   3, 51
/// Identifiable character position, 0, 48 do not print 1, 49 bar code above 2, 50 bar code below 3, 51 bar code on and below the print
- (void)addSetBarcodeHRPosition:(int)n;

/// HRI
/// Set bar code character type and select HRI font to use
/// @param n 0 0,48  ASCII 12 × 24  1,49  ASCII 9 × 17
/// Fixed as 0,48 standard ASCII characters (12 × 24) 1,49 compressed ASCII characters (9 × 17)
-(void)addSetBarcodeHRIFont:(int)n;

/// Set the bar code height
/// @param n  n40
/// Height bar code height is n points, the default is 40
-(void)addSetBarcodeHeight:(int)n;

/// Set the bar code unit width. Use the printer's internal default value
/// @param n  2 ≤n ≤6
/// Barcode width 2 ≤n ≤6
-(void)addSetBarcodeWidth:(int)n;

/// EAN13
/// Print the EAN13 code
/// @param content 0-912
/// The data range is 0-9 and the length is 12 bits
-(void)addEAN13:(NSString *)content;

/// EAN8
/// Print EAN8 code
/// @param content 0-97
/// The data range is 0-9 and the length is 7 bits
-(void)addEAN8:(NSString *)content;

/// UPCA
/// Print UPCA barcode
/// @param content 0-911
/// The data range is 0-9 and the length is 11 bits
-(void)addUPCA:(NSString *)content;

/// ITF14
/// Print the ITF14 barcode
/// @param content  0-9
/// Data range 0-9 even number of data lengths
-(void)addITF:(NSString *)content;

/// CODE39
/// Print Code39 barcode
/// @param content 0-9 A-Z SP $ % + - . / * /
/// Data range 0-9 A-Z SP $ % + - . / , * for (start/end character)
-(void)addCODE39:(NSString *)content;

/// CODE128 1D 6B 49 nnCODEB:  {B , 0x7B, 0x42
/// Print CODE128 code The command format is 1D 6B 49 N, where N is the length of all subsequent printed strings, including the declaration of the character set. The default CODEB character set is: {B, 0x7B, 0x42
/// @param charset CODEB {A  {B  {CcharsetB contentcontent
/// The CODEB character set has {A {B {C, and the charset default value is B. In fact, A character set can be inserted into A content. A content can have multiple character sets defined.
/// @param content 0x00-0x7f
/// Data range 0x00-0x7F
-(void)addCODE128:(char)charset :(NSString *)content;

/// CODE128{A {B {C58mm1415 17 19 1D 6B 49 nnCODEB:  {B , 0x7B, 0x42
/// Printing Code128 code supports mixed {A {B {C printing. It supports 58mm paper printing of odd digits larger than 14 digits. For example, 15, 17, and 19 digits are printed in the command format 1D, 6B, 49 N, where N is the length of all subsequent printed strings, including character set declarations. The default CODEB character set is: {B, 0x7B, 0x42
/// @param height  The barcode height
/// @param width  Bar code width
/// @param data 0x00-0x7f16 Data range 0x00-0x7F, all hexadecimal
-(void)addCODE128ABC:(int)height :(int)width :(NSData*)data;

/// addNVLOGO
/// @param n n1<=n<=20(N is the ordinal number, 1<=n<=20)
/// @param m m0(M defaults to 0)
-(void)addNVLOGO:(int)n :(int)m;

/*
  Hexadecimal code
 1D 76 30 m xL xH yL yH d1...dk
 0 ≤ m ≤ 3, 48 ≤ m ≤ 51
 0 ≤ xL ≤ 255
 0 ≤ xH ≤ 255
 0 ≤ yL ≤ 255
 0 ≤ yH ≤ 255
 0 ≤ d ≤ 255
 k = ( xL + xH × 256) × ( yL + yH × 256) ( k ≠ 0)
 */
/// ESC Add an ESC bitmap
/// @param m  0, 48  1, 49  2, 50  3, 51
/// Mode 0, 48 Normal 1, 49 wide 2, 50 high 3, 51 wide and high
/// @param xL xL + xH × 256
/// Denotes the number of bytes of horizontal bitmap (XL + XH × 256)
/// @param xH xL + xH × 256
/// Denotes the number of bytes of horizontal bitmap (XL + XH × 256)
/// @param yL yL + yH × 256
/// Represents the number of bitmap points in the vertical direction (YL + YH × 256)
/// @param yH yL + yH × 256
/// Represents the number of bitmap points in the vertical direction (YL + YH × 256)
/// @param data  Image data
-(void)addESCBitmapwithM:(int)m withxL:(int)xL withxH:(int)xH withyL:(int)yL withyH:(int) yH withData:(NSData*) data;

///  Print raster bitmap
/// @param image  Image
/// @param width  Image width
-(void)addOriginrastBitImage:(UIImage *)image width:(int)width;

///  Print raster bitmap
/// @param image  Image
-(void)addOriginrastBitImage:(UIImage *)image;

/*
 ESCQRCode
 1QRCode
 2
 3QRCode
 4QRCode
 
 Under the correct ESC instruction, the printing process of QRCODE is four steps:
 1. Set the size of QRCODE; (may be omitted)
 2. Set the error correction level; (Generally do not need to set, ignore)
 3. Store the corresponding text information of QRCODE in the printer cache; (Must be)
 4. Send the print QRCODE command.
 
 */
/// QRCode
/// Set QRCode size (can be omitted)
/// @param cn cn=49
/// @param fn fn=67
/// @param n n=3
-(void)addQRCodeSizewithpL:(int)pL withpH:(int)pH withcn:(int)cn withyfn:(int)fn withn:(int)n;

/// Set error correction level (generally do not need to set, ignore)
/// @param cn cn=49
/// @param fn fn=69
/// @param n 48≤n≤51 n=48
-(void)addQRCodeLevelwithpL:(int)pL withpH:(int)pH withcn:(int)cn withyfn:(int)fn withn:(int)n;

/// QRCode
/// Put the corresponding text information of QRCode into the printer cache (must have)
/// @param cn cn=49
/// @param fn fn=80
/// @param m m=48
/// @param data data
-(void)addQRCodeSavewithpL:(int)pL withpH:(int)pH withcn:(int)cn withyfn:(int)fn withm:(int)m withData:(NSData*)data;

/// QRCode
/// Send the print QRCODE command
/// @param cn cn = 49
/// @param fn fn = 81
/// @param m m = 48
-(void)addQRCodePrintwithpL:(int)pL withpH:(int)pH withcn:(int)cn withyfn:(int)fn withm:(int)m;

///  Sets whether or not a Chinese font is present
/// @param n
/// Whether to double the width, whether to double the height, whether to underline
-(void)addSetKanjiFontMode:(int)n;

///  Set Chinese character valid
-(void)addSelectKanjiMode;

///  Set Chinese characters to underline
/// @param n  n
/// Select or deselect the underlined character based on the value of n
-(void)addSetKanjiUnderLine:(int)n;

///  Invalid setting of Chinese character
-(void)addCancelKanjiMode;

///  Sets the right spacing of characters
/// @param n  Span length
-(void)addSetCharacterRightSpace:(int)n;

/// Sets the left and right spacing of Chinese characters
/// @param n1  0 ≤ n1 ≤ 255 (The left space 0 ≤ n1 ≤ 255)
/// @param n2  0 ≤ n2 ≤ 255 (The right spacing 0 ≤ n2 ≤ 255)
-(void)addSetKanjiLefttandRightSpace:(int)n1 :(int)n2;

///  Set the bold mode
/// @param n 0 ≤ n ≤ 255 (Whether the bold 0 ≤ n ≤ 255)
-(void)addTurnEmphasizedModeOnOrOff:(int)n;

///  Set the weighting mode
/// @param n  0 ≤ n ≤ 255(Whether increasing 0 ≤ n ≤ 255)
-(void)addTurnDoubleStrikeOnOrOff:(int)n;

///  Sets horizontal and vertical unit distances
/// @param x  The level of the unit
/// @param y  Vertical units
-(void)addSetHorAndVerMotionUnitsX:(int)x Y:(int)y;

/// Sets the absolute print location
/// @param n  Distance from the starting printing position
-(void)addSetAbsolutePrintPosition:(int)n;

///  Set the print width
/// @param width  Print width
-(void)addSetPrintingAreaWidth:(int)width;

/// The ticket is doubled by default (restart does not invalidate)
/// @param n1      01 (Width defaults to 0 and doubled to 1)
/// @param n2      01 (Height defaults to 0 and doubled to 1)
/// @param n3   01 (The width of a character defaults to 0 and is doubled to 1)
/// @param n4   01 (The character high defaults to 0 and doubles to 1)
-(void)receiptDoubleHeightOrDefaultPrintN1:(int)n1 N2:(int)n2 N3:(int)n3 N4:(int)n4;

///  Set the default code page
/// @param n  n0-1016-3250-89
/// The parameter range of code page n is (0-10, 16-32, 50-89)
-(void)setDefaultCodePage:(int)n;

///  Set baud rate
/// @param baudRate  Baud rate
-(void)setBaudRate:(int)baudRate;

///  Query printer power.
/// <p>3132 33 35 </p>
/// 31 (low power); 32 (medium electric quantity); 33 (high power); 35 (charging)
-(void)queryElectricity;

///  Print the self-check page
-(void)addSelfTest;

@end

