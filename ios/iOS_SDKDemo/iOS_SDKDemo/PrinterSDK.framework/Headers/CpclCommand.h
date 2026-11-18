//
//  CpclCommand.h
//  GSDK
//
//  Created by smarnet on 2019/2/26.
//  Copyright © 2019 Smarnet. All rights reserved.
//

#import "Command.h"
NS_ASSUME_NONNULL_BEGIN

/**
 *  @enum TextAngle
 *
 *  @discussion 文本旋转角度(Text rotation Angle)
 */
typedef NS_ENUM (NSUInteger,TextAngle) {
    TEXT_ANGLE_0, // /< 0 (The rotation Angle is 0 degrees)
    TEXT_ANGLE_90, // /< 90 (The rotation Angle is 90 degrees)
    TEXT_ANGLE_180, // /< 180 (The rotation Angle is 180 degrees)
    TEXT_ANGLE_270 // /< 270 (The rotation Angle is 270 degrees)
};

/**
 *  @enum BarcodeType
 *
 *  @discussion 条码类型(Bar code type)
 */
typedef NS_ENUM (NSUInteger,BarcodeType) {
    UPCA, // /< , 11 (Pure numbers, eleven digits)
    UPCE, // /< , 7 , 0(A pure number with seven digits. The first digit must be 0)
    EAN13, // /<EAN/JAN-13 , 12 (Save a number with 12 digits)
    EAN8, // /< A~Z , 0~9 , 44 (The content can contain A~Z capital English letters, 0~9 numbers and some special symbols, A total of 44 characters)
    CODE39, // /< A~Z , 0~9 , 44 (The content can contain A~Z capital English letters, 0~9 numbers and some special symbols, A total of 44 characters)
    CODE93, // /<Code93/Ext.93 ASCII (128 ) (All (128) ASCII characters are available)
    CODE128, // /<Code128Auto , , (It includes upper and lower case letters, numbers, common punctuation marks, and some control characters)
    CODABAR // /< , 7 (Contents include numbers and 7 special symbols)("+", "-", "*", "/", "$", ".", ": "), A, B, C, D
};

/**
 *  @enum Alignment
 *
 *  @discussion 打印方向(The print direction)
 */
typedef NS_ENUM (NSUInteger,Orientation) {
    HORIZONTAL, // /< (Horizontal print)
    VERTICAL // /< (Vertical print)
};

/**
 *  @enum QRCodeLevel
 *
 *  @discussion 二维码纠错等级(QR code error correction level)
 */
typedef NS_ENUM (NSUInteger,QRCodeLevel) {
    QRCODE_LEVEL_H, // /< (H )Extremely high reliability level (Class H)
    QRCODE_LEVEL_Q, // /< (Q )High reliability level (Q-level)
    QRCODE_LEVEL_M, // /< (M )Standard grade (M grade)
    QRCODE_LEVEL_L // /< (L )High Density Level (L Level)
};

/**
 *  @enum Mode
 *
 *  @discussion 二维码输入模式(QR code input mode)
 */
typedef NS_ENUM (NSUInteger,Mode) {
    QRCODE_INPUTMODE_A, // /< (automatic)
    QRCODE_INPUTMODE_M // /< ( )(Manual (this mode is not currently supported))
};

/**
 *  @enum Alignment
 *
 *  @discussion 对齐方式(alignment)
 */
typedef NS_ENUM (NSUInteger,Alignment) {
    ALIGNMENT_CENTER, // /< (Center to align all subsequent fields)
    ALIGNMENT_LEFT, // /< (Left align all subsequent fields)
    ALIGNMENT_RIGHT, // /< (Right-align all subsequent fields)
};

/**
 *  @enum Graphics
 *
 *  @discussion 图形(graphics)
 */
typedef NS_ENUM (NSUInteger,Graphics) {
    EXPANDED_GRAPHICS, // /< (Print the extended graph horizontally)
    COMPRESSED_GRAPHICS, // /< (Print compressed graphics laterally)
};

@interface CpclCommand:Command

 // / (Tag initialization)
 // / @param offset (Label lateral offset)
 // / @param horizontalDpi DPI (Transverse DPI)
 // / @param verticalDpi DPI (Vertical DPI)
 // / @param height (Maximum label height)
 // / @param qty (Number of Prints)
- (void)addInitializePrinterOffset:(int)offset horizontalDpi:(int)horizontalDpi verticalDpi:(int)verticalDpi height:(int)height qty:(int)qty;

 // / (Perform print)
- (void)addPrint;

 // / (Perform reverse printing)
- (void)addPOPrint;

 // / (Print text)
 // / @param angle (Text rotation Angle)
 // / @param font (The text font)
 // / @param size (Size)
 // / @param x (The horizontal starting position of the text)
 // / @param y (Vertical starting position of the text)
 // / @param content (What needs to be printed)
- (void)addTextAngle:(TextAngle)angle withFont:(int)font size:(int)size x:(int)x y:(int)y content:(NSString *)content;

 // / (Text series)
 // / @param x (The horizontal starting position of the text)
 // / @param y (Vertical starting position of the text)
- (void)addConcatText:(int)x withY:(int)y;

 // / (Tandem text content)
 // / @param font / (Font name/number)
 // / @param size (Size)
 // / @param offset (The unit value of the text is offset relative to the starting position)
 // / @param content (The text to print)
- (void)addTextFont:(int)font size:(int)size offset:(int)offset content:(NSString *)content;

 // / (Terminate text concatenation)
- (void)endConcatText;

 // / (enlarge font)
 // / @param w , 1~16(Width magnification, effective magnification is 1~16)
 // / @param h , 1~16(High magnification, effective magnification is 1~16)
- (void)addSetmagWidth:(int)w height:(int)h;

 // / (font bold)
 // / @param isBlod YES: (bold) NO:(Not bold)
- (void)addSetBlod:(BOOL)isBlod;

 // / (Print Barcode)
 // / @param orientation (Direction of the barcode)
 // / @param type (Bar code type)
 // / @param width (The unit width of a narrow strip)
 // / @param ratio (The ratio of wide bars to narrow bars)
 // / @param height (Bar code unit height)
 // / @param x (Horizontal starting position of barcode)
 // / @param y (Longitudinal starting position of the barcode)
 // / @param content (Barcode data)
- (void)addBarcodeOrientation:(Orientation)orientation type:(BarcodeType)type withWidth:(int)width ratio:(int)ratio height:(int)height x:(int)x y:(int)y content:(NSString *)content;

 // / PDF-417 (Print PDF-417 barcode)
 // / @param orientation (Direction of the barcode)
 // / @param x (Horizontal starting position of barcode)
 // / @param y (Longitudinal starting position of the barcode)
 // / @param XD , 1-32, 2(Width minimum number of points, values range from 1 to 32, default is 2)
 // / @param YD , 1-32, 6(Height minimum number of points, values range from 1 to 32, default is 6)
 // / @param C , 1-30, 3(The number of yards per line. Values range from 1 to 30. Default is 3)
 // / @param S , 0-8, 1(Error correction level. Values range from 0 to 8. Default is 1)
 // / @param content PDF-417 (PDF-417 barcode data)
- (void)addPDF417Orientation:(Orientation)orientation x:(int)x y:(int)y XD:(int)XD YD:(int)YD C:(int)C S:(int)S content:(NSString *)content;

/// DATAMATRIX
 // / @param x (Horizontal starting position of barcode)
 // / @param y (Longitudinal starting position of the barcode)
 // / @param H , 0- , 3-8(Bar code point density, the value range is 0- label range, generally set to 3-8)
 // / @param content (barcode data)
- (void)addDatamatrixWithX:(int)x y:(int)y H:(int)H content:(NSString *)content;


/// Used to mark a bar code with the same data that was used when it was created, eliminating the need to annotate the bar code with a separate text command. The text is in the middle below the bar code
 // / @param font (The font number to use when commenting the barcode)
 // / @param offset (The unit offset of the text distance bar code)
- (void)addBarcodeTextFont:(int)font offset:(int)offset;

 // / BARCODE-TEXT (Disable the barcode-text command effect)
- (void)addBarcodeTextOff;

 // / (Initialize QR code parameters)
 // / @param orientation (The print direction)
 // / @param x (Lateral starting position)
 // / @param y (Longitudinal starting position)
 // / @param m QR Code ,1 2, 2(QR Code specification number,1 or 2, recommended as 2)
 // / @param u / 1-32, 6(The unit width/unit height of the module is 1-32, default is 6)
- (void)addQRCodeVertical:(Orientation)orientation x:(int)x y:(int)y m:(int)m u:(int)u;

 // / (Print QR code)
 // / @param level (Error correction level)
 // / @param mask , 0-8, mask<0||mask>8 , (Mask, mask valid range is 0 to 8, mask < 0 | | mask > 8 omit mask, to the next bit machine processing)
 // / @param mode (The input mode)
 // / @param content (QR code content)
- (void)addQRCodeLevel:(QRCodeLevel)level mask:(int)mask mode:(Mode)mode content:(NSString *)content;

 // / (Print QR code)
 // / @param level (Error correction level)
 // / @param content (QR code content)
- (void)addQRCodeLevel:(QRCodeLevel)level content:(NSString *)content;

 // / QR Code (End the QR Code instruction)
- (void)endQRCode;

 // / (Print rectangular)
 // / @param x0 X (The top left x-coordinate)
 // / @param y0 Y (The top left y-coordinate)
 // / @param x1 X (The lower right x-coordinate)
 // / @param y1 Y (The lower right y-coordinate)
 // / @param width (The unit width of a line that forms a rectangle)
- (void)addBoxX0:(int)x0 y0:(int)y0 x1:(int)x1 y1:(int)y1 width:(int)width;

 // / (Print lines)
 // / @param x0 X (The x-coordinate of the starting point)
 // / @param y0 Y (The y-coordinate of the starting point)
 // / @param x1 X (The x-coordinate of the termination point)
 // / @param y1 Y (The y-coordinate of the termination point)
 // / @param width (The unit width of a line)
- (void)addLineX0:(int)x0 y0:(int)y0 x1:(int)x1 y1:(int)y1 width:(int)width;

 // / INVERSE-LINE LINE . INVERSE-LINE , . , / . INVERSE-LINE
/// The inverse-line command has the same syntax as the LINE command. The black areas of previously created objects that are within the areas defined by the inverse-line command are redrawn as white, and the white areas are redrawn as black. These objects can include text, bar codes, and/or graphics. Inverse-line does not work on objects created after it, even if they are in the overlay area of the command
 // / @param x0 X (The x-coordinate of the starting point)
 // / @param y0 Y (The y-coordinate of the starting point)
 // / @param x1 X (The x-coordinate of the termination point)
 // / @param y1 Y (The y-coordinate of the termination point)
 // / @param height (Height of the color inversion area)
- (void)addInverseLineX0:(int)x0 y0:(int)y0 x1:(int)x1 y1:(int)y1 height:(int)height;

 // / (The graphics command prints the bitmap graph)
 // / @param graphics (Graphic printing mode)
 // / @param width (Width of the figure)
 // / @param x (Lateral starting position)
 // / @param y (Longitudinal starting position)
 // / @param image (Image)
 // / @param threshold 0: 1-255: (Binarization threshold 0: automatic 1-255: manual)
- (void)addImageGraphics:(Graphics)graphics width:(int)width x:(int)x y:(int)y image:(UIImage *)image withThreshold:(int)threshold;

 // / (Use the alignment command to control the alignment of fields)
 // / @param alignment (alignment)
 // / @param end . , , ; , ( )
/// End point of alignment. If no arguments are entered, the alignment command uses the width of the print head for landscape printing; For portrait printing, the alignment command will use zero (header)
- (void)addJustification:(Alignment)alignment end:(int)end;

 // / (Field alignment)
 // / @param alignment (alignment)
- (void)addJustification:(Alignment)alignment;

 // / (The printer assumes that the page width is the full width of the printer)
 // / @param width (The unit width of the page)
-(void)addPageWidth:(int)width;

 // / (Set print speed)
 // / @param level (Speed level)
- (void)addSpeed:(int)level;

 // / (Set the buzzer)
 // / @param time , 1/8 (The duration of buzzing is increased by 1/8 of a second)
- (void)addBeep:(int)time;

 // / (Query printer status)
- (void)queryPrinterStatus;

@end

NS_ASSUME_NONNULL_END
