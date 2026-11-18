package com.gpsdk.demo;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.Matrix;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.EncodeHintType;
import com.google.zxing.MultiFormatWriter;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;

import java.util.Hashtable;


public class MaxCardManager
{
    private static MaxCardManager instance;
    

    private static final int IMAGE_WIDTH = 30;
    
    private MaxCardManager()
    {
    }
    
    public static MaxCardManager getInstance()
    {
        if (instance == null)
        {
            instance = new MaxCardManager();
        }
        return instance;
    }


    
    public Bitmap create2DCode(Activity activity, String str, int w, int h, Bitmap mBitmap)
        throws WriterException
    {

        //        Bitmap mBitmap = ((BitmapDrawable)activity.getResources().getDrawable(logo)).getBitmap();

        Matrix m = new Matrix();
        float sx = (float)2 * IMAGE_WIDTH / mBitmap.getWidth();
        float sy = (float)2 * IMAGE_WIDTH / mBitmap.getHeight();
        m.setScale(sx, sy);

        mBitmap = Bitmap.createBitmap(mBitmap, 0, 0, mBitmap.getWidth(), mBitmap.getHeight(), m, true);
        Hashtable hints = new Hashtable();
        hints.put(EncodeHintType.CHARACTER_SET, "UTF-8");
        
        BitMatrix matrix = new MultiFormatWriter().encode(str, BarcodeFormat.QR_CODE, w, h, hints);//360   360
        int width = matrix.getWidth();
        int height = matrix.getHeight();

        int halfW = width / 2;
        int halfH = height / 2;
        
        int[] pixels = new int[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
 // mBitmap
                if (mBitmap != null && x > halfW - IMAGE_WIDTH && x < halfW + IMAGE_WIDTH && y > halfH - IMAGE_WIDTH
                    && y < halfH + IMAGE_WIDTH)
                {
                    pixels[y * width + x] = mBitmap.getPixel(x - halfW + IMAGE_WIDTH, y - halfH + IMAGE_WIDTH);
                }
                else if (matrix.get(x, y))
                {
                    pixels[y * width + x] = 0xff000000;
                }
            }
        }
        Bitmap bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_4444);
 // bitmap, api
        bitmap.setPixels(pixels, 0, width, 0, 0, width, height);
        return bitmap;
    }
    
    /**
     * 用字符串生成二维码
     */
    public Bitmap create2DCode(String str, int w, int h)
        throws WriterException
    {
        Hashtable hints = new Hashtable();
        hints.put(EncodeHintType.CHARACTER_SET, "UTF-8");

        BitMatrix matrix = new MultiFormatWriter().encode(str, BarcodeFormat.QR_CODE, w, h,hints);//240 240
        int width = matrix.getWidth();
        int height = matrix.getHeight();

        int[] pixels = new int[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (matrix.get(x, y))
                {
                    pixels[y * width + x] = 0xffAAAAAA;
                }
                //wrj1
                else{
                    pixels[y * width + x] = 0xffffffff;
                }
                //wrj1
            }
        }
//        int[] argb = new int[width * height];
//
//        for (int i = 0; i < argb.length; i++) {
//
//            argb[i] = (255 << 24) | (argb[i] & 0x00FFFFFF);
//
//        }

            Bitmap bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
 // bitmap, api
        bitmap.setPixels(pixels, 0, width, 0, 0, width, height);
        return bitmap;
    }
}
