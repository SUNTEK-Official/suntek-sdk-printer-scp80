//
//  TscCommand.h
//  Gprinter
//
//  Created by smarnet on 2018/6/15.
 // Copyright (c) 2018 Smarnet. All rights reserved
//

#import "Command.h"
/**
 *  @enum Response
 *
 *  @discussion 自动返回状态功能(Automatic return status function)
 */
typedef NS_ENUM (NSUInteger,Response){
    OFF     = 0, // /< (Turn off auto-return status)
    ON      = 1, // /< , (Turn on the function of automatic return status, and return once after each print)
    BATCH   = 2, // /< , (Turn on the automatic return function, and return once after printing)
};

@interface TscCommand : Command

 // / (Sets the width and height of the label dimensions)
 // / @param width (The label width)
 // / @param height (The label height)
- (void)addSize:(int) width :(int)height;

 // / mm(Sets the label gap size The unit of mm)
 // / @param m (The length of the gap)
 // / @param n (The interval offset)
- (void)addGapWithM:(int) m withN:(int)n;

 // / (Sets the label origin coordinates)
 // / @param x (abscissa)
 // / @param y (ordinate)
- (void)addReference:(int) x :(int)y;

 // / (Set print speed)
 // / @param speed (The printing speed)
- (void)addSpeed:(int)speed;

 // / (Set the print concentration)
 // / @param density (The concentration of)
- (void)addDensity:(int)density;

 // / (Set the print direction)
 // / @param direction (The direction of)
- (void)addDirection:(int) direction;

 // / (Clear the print buffer)
- (void)addCls;

 // / (Draws text on the label)
 // / @param x (abscissa)
 // / @param y (ordinate)
 // / @param font (Font type)
 // / @param rotation (Rotation Angle)
 // / @param xScal (Lateral magnification)
 // / @param yScal (Longitudinal magnification)
 // / @param text (Literal string)
- (void)addTextwithX:(int)x withY:(int)y withFont:(NSString*)font withRotation:(int)rotation withXscal:(int)xScal withYscal:(int)yScal withText:(NSString*) text;

 // / (Draws text on the label)
 // / @param x (abscissa)
 // / @param y (ordinate)
 // / @param font (Font type)
 // / @param rotation (Rotation Angle)
 // / @param xScal (Lateral magnification)
 // / @param yScal (Longitudinal magnification)
 // / @param text (Literal string)
 // / @param encoding (Character encoding)
- (void)addTextwithX:(int)x withY:(int)y withFont:(NSString*)font withRotation:(int)rotation withXscal:(int)xScal withYscal:(int)yScal withText:(NSString *)text encoding:(NSStringEncoding)encoding;

 // / (Print pictures)
 // / @param x (The horizontal starting position of the lattice image)
 // / @param y (The vertical starting position of the lattice image)
 // / @param width , byte (The width of the image, expressed in byte)
 // / @param height , (dot) (The height of the image, expressed as dot (dot))
 // / @param mode (Image rendering mode)
/// 0 OVERWRITE
/// 1 OR
/// 2 XOR
 // / @param data (Image data)
- (void)addBitmapwithX:(int)x withY:(int)y withWidth:(int)width withHeight:(int)height withMode:(int)mode withData:(NSData *)data;

 // / (Print pictures)
 // / @param x (The horizontal starting position of the lattice image)
 // / @param y (The vertical starting position of the lattice image)
 // / @param mode (Image rendering mode)
/// 0 OVERWRITE
/// 1 OR
/// 2 XOR
 // / @param width (Image width)
 // / @param image (A picture that needs to be printed)
- (void)addBitmapwithX:(int)x withY:(int)y withMode:(int)mode withWidth:(int)width withImage:(UIImage *)image;

 // / (Print pictures)
 // / @param x (The horizontal starting position of the lattice image)
 // / @param y (The vertical starting position of the lattice image)
 // / @param mode (Image rendering mode)
/// 0 OVERWRITE
/// 1 OR
/// 2 XOR
 // / @param image (A picture that needs to be printed)
- (void)addBitmapwithX:(int)x withY:(int)y withMode:(int)mode withImage:(UIImage *)image;

 // / (Draws a one-dimensional bar code on the label)
 // / @param x (abscissa)
 // / @param y (ordinate)
 // / @param barcodeType (Bar code type)
 // / @param height , 40(Bar code height, default is 40)
 // / @param readable , 0: , 1: ( , 0: , 1: )
 // / @param rotation , , , 0 , 90 90 , 180 180 , 270 270
/// Rotation Angle, Bar Code Rotation Angle, Clockwise, 0 does not rotate, 90 rotates 90 degrees Clockwise, 180 rotates 180 degrees Clockwise, 270 rotates 270 degrees Clockwise
 // / @param narrow 2, bar , (dot) (Default value 2, narrow bar width, represented by dot (dot))
 // / @param wide 4, bar , (dot) (Default value 4, wide bar width, expressed as dot (dot))
 // / @param content (Bar code content)
- (void)add1DBarcode:(int)x :(int)y :(NSString *)barcodeType :(int)height :(int)readable :(int)rotation :(int)narrow :(int)wide :(NSString *)content;

 // / QRCode (Draw QRCode QR code on the label)
 // / @param x (abscissa)
 // / @param y (ordinate)
 // / @param ecclever QRCODE , L, L 7%,M 15%,Q 25%,H 30%(Select QRCode error correction level, default is L, L is 7%,M is 15%,Q is 25%,H is 30%)
 // / @param cellWidth 1~10, 4(QR code width 1~10, the default is 4)
 // / @param mode A, A Auto,M Manual(The default is A, A is Auto, and M is Manual)
 // / @param rotation , 0, QRCode , , 0 , 90 90 , 180 180 , 270 270 ()
/// Rotation Angle, Bar Code Rotation Angle, Clockwise, 0 does not rotate, 90 rotates 90 degrees Clockwise, 180 rotates 180 degrees Clockwise, 270 rotates 270 degrees Clockwise
 // / @param content (Bar code content)
- (void)addQRCode:(int)x :(int)y :(NSString *)ecclever :(int)cellWidth :(NSString *)mode :(int)rotation :(NSString *)content;

 // / (Perform print)
 // / @param m (Specifies the number of copies to be printed)
 // / @param n (Number of duplicate prints per label)
- (void)addPrint:(int)m :(int)n;

 // / TSC , cls (Send some TSC fixed commands before CLS commands)
- (void)addComonCommand;

 // / , (Print the self-check page, print the test page)
- (void)addSelfTest;

 // / (Query Printer Type)
- (void)queryPrinterType;

 // / (Set printer stripping mode)
 // / @param peel ON/OFF (Whether open)
- (void)addPeel:(NSString *)peel;

 // / (Set the printer to tear mode)
 // / @param tear ON/OFF (Whether open)
- (void)addTear:(NSString *)tear;

 // / (Sets whether the cutter is valid)
 // / @param cutter OFF/pieces(Whether open)
- (void)addCutter:(NSString *)cutter;

 // / (Sets whether half cutting of the cutter is effective)
 // / @param cutter (Whether open)
- (void)addPartialCutter:(NSString *)cutter;

 // / (Set the buzzer)
 // / @param level (frequency)
 // / @param interval ms(Time ms)
- (void)addSound:(int)level :(int)interval;

 // / ,CASHDRAWER m,t1,t2(Open the cash box command,CASHDRAWER m,t1,t2)
 // / @param m m 0, 48 2 1, 49 5
/// Cashbox No. M 0,48 Pins of Cashbox Socket 2, 1,49 Pins of Cashbox Socket 5
 // / @param t1 0 ≤ t1 ≤ 255 t1 t2 m
/// The high level time 0 ≤ T1 ≤ 255 outputs the moneybox opening pulse set by T1 and T2 to the pin specified by M
 // / @param t2 0 ≤ t2 ≤ 255 t1 t2 m
/// Low level time 0 ≤ T2 ≤ 255 Output from the money box set by T1 and T2 opening pulse to pin specified by M
- (void)addCashdrawer:(int)m :(int)t1 :(int)t2;

 // / , (Draw a black block on the label and draw a line)
 // / @param x (Initial abscissa)
 // / @param y (Initial ordinate)
 // / @param width , (dot) (Line width, expressed as dot (dot))
 // / @param height , (dot) (The height of the line is indicated by a dot (dot))
- (void)addBar:(int)x :(int)y :(int)width :(int)height;

 // / (Draws a rectangle on the label)
 // / @param xStart (Initial abscissa)
 // / @param yStart (Initial ordinate)
 // / @param xEnd (Terminal abscissa)
 // / @param yEnd (Terminal ordinate)
 // / @param lineThickness , (dot) (The thickness or width of a rectangular frame line, denoted by a dot (dot))
- (void)addBox:(int)xStart :(int)yStart :(int)xEnd :(int)yEnd :(int)lineThickness;

 // / (Query printer status)
- (void)queryPrinterStatus;

 // / ( , )(Prints the specified area backwards (black to white, white to black))
 // / @param xStart (Initial abscissa)
 // / @param yStart (Initial abscissa)
 // / @param width X , dot (The width in the direction of the X coordinate, in dot)
 // / @param height Y , dot (The height in the Y direction, in dot)
- (void)addReverse:(int)xStart :(int)yStart :(int)width :(int)height;


/// When the printer finishes printing, it returns to the state automatically. Can be used to achieve continuous printing function
 // / @param response (Automatic return state)
- (void)addQueryPrinterStatus:(Response)response;

 // / (Query Printer Power)
- (void)queryElectricity;


/// Set the height of the black label and define the extra length of the label after printing
 // / @param m (0≤m≤1(inch), 0≤m≤25.4(mm))(Height of black mark (0≤m≤1(inch), 0≤m≤25.4(mm)))
 // / @param n n≤ (inch mm)(Extra paper length N ≤ label paper length (inch or mm))
- (void)addBLine:(int)m :(int)n;

 // / Add a code page
 // / @param codepage Character set representation
- (void)addCodepage:(NSString *)codepage;

@end
